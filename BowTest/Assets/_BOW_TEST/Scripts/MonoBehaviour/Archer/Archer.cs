using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using System;

[RequireComponent(typeof(ArcherAnimationHandler))]
[RequireComponent(typeof(RagdollController))]
public class Archer : MonoBehaviour
{
    public event Action OnStaminaChangedEvent;

    [Header("=== STAMINA ===")]
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _staminaRecoverySpeed;
    [SerializeField] private float _staminaWasteSpeed;

    [Header("=== TARGETS DETECTION ===")]
    [SerializeField] private float _targetsCheckRate;
    [SerializeField] private LayerMask _obstacleLayers;

    [Header("=== MOVING ===")]
    [SerializeField] private float _rotationSpeed;

    [Header("=== SHOOTING ===")]
    [SerializeField] private float _maxShootRange;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _recoveryDelay;
    [SerializeField] private List<BowTightnessPhase> _bowTightnessPhases;
    [SerializeField] private float _maxArrowDeviation;

    [Header("=== ARROW ===")]
    [SerializeField] private Arrow _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPoint;

    [Inject] private InputHandler _inputHandler;
    [Inject] private CameraShake _cameraShake;
    [Inject] private GameStateController _gameStateController;
    [Inject] private TargetsHolder _targetsHolder;
    [Inject] private BowTightnessIndicator _bowTightnessIndicator;
    [Inject] private DiContainer _diContainer;
    private ArcherAnimationHandler _animationHandler;
    private RagdollController _ragdollController;
    private WaitForSeconds _shootWait;
    private WaitForSeconds _recoveryWait;
    private WaitForSeconds _targetsCheckWait;
    private Coroutine _playingShootRoutine;
    private Target _selectedTarget;
    BowTightnessPhase _maxTightnessPhase;
    private float _stamina;
    private bool _isCanShoot;
    private bool _isAiming;

    public bool IsDead { get; private set; }
    public float Stamina
    {
        get => _stamina;
        set
        {
            if (value < 0) value = 0;
            else if (value > MaxStamina) value = MaxStamina;

            _stamina = value;
            OnStaminaChangedEvent?.Invoke();
        }
    }
    public float MaxStamina => _maxStamina;

    private void Awake()
    {
        _animationHandler = GetComponent<ArcherAnimationHandler>();
        _ragdollController = GetComponent<RagdollController>();

        _shootWait = new WaitForSeconds(_shootDelay);
        _recoveryWait = new WaitForSeconds(_recoveryDelay);
        _targetsCheckWait = new WaitForSeconds(1 / _targetsCheckRate);
        _maxTightnessPhase = _bowTightnessPhases.OrderBy(phase => phase.TimePeriod.MaxValue).Last();

        _bowTightnessIndicator.Init(_bowTightnessPhases);

        _inputHandler.OnPressedEvent += TryStartShooting;
    }

    private void Start()
    {
        Stamina = MaxStamina;
        _isCanShoot = true;
        StartCoroutine(SelectNearestVisibleTargetRoutine());
    }

    public void TryDead()
    {
        if (IsDead) return;

        IsDead = true;
        TryStopShooting();
        _ragdollController.EnableRagdoll();
        _cameraShake.Shake();
        _gameStateController.Lose();
    }

    private void TryStartShooting()
    {
        if (IsDead) return;
        if (_isCanShoot == false) return;

        _playingShootRoutine = StartCoroutine(ShootingRoutine());
    }

    private void TryStopShooting()
    {
        if (_playingShootRoutine == null) return;

        StopCoroutine(_playingShootRoutine);
        _playingShootRoutine = null;
        _animationHandler.StopAimAnimation();
        _bowTightnessIndicator.TryHide();
        _isAiming = false;
    }

    private IEnumerator ShootingRoutine()
    {
        _isCanShoot = false;
        _isAiming = true;
        _animationHandler.PlayAimAnimation();
        float tightTimer = 0;

        _bowTightnessIndicator.TryShow();
        _bowTightnessIndicator.SetSliderByTightTimer(tightTimer);

        yield return _shootWait;

        while(tightTimer < _maxTightnessPhase.TimePeriod.MaxValue && _inputHandler.IsPressed
            && Stamina > 0)
        {
            tightTimer += Time.deltaTime;
            _bowTightnessIndicator.SetSliderByTightTimer(tightTimer);

            yield return null;
        }

        if (tightTimer > _maxTightnessPhase.TimePeriod.MaxValue)
            tightTimer = _maxTightnessPhase.TimePeriod.MaxValue;

        LaunchArrow(tightTimer);

        _cameraShake.Shake();
        _animationHandler.StopAimAnimation();
        _bowTightnessIndicator.TryHide();

        yield return _recoveryWait;

        _isCanShoot = true;
        _playingShootRoutine = null;
        _isAiming = false;
    }
    
    private void LaunchArrow(float tightTime)
    {
        BowTightnessPhase currentPhase = _bowTightnessPhases.Find(
            phase => tightTime >= phase.TimePeriod.MinValue && tightTime <= phase.TimePeriod.MaxValue);
        
        Arrow newArrow = _diContainer.InstantiatePrefab(_arrowPrefab, _arrowSpawnPoint.position, _arrowSpawnPoint.rotation, null).GetComponent<Arrow>();
        newArrow.Launch(GetArrowTargetPoint(currentPhase), currentPhase.Damage);
        Stamina -= currentPhase.StaminaPrice;
    }

    private Vector3 GetArrowTargetPoint(BowTightnessPhase phase)
    {
        Vector3 arrowTargetPoint = Vector3.zero;
        if (_selectedTarget != null)
        {
            int side = UnityEngine.Random.value >= 0.5 ? 1 : -1;
            arrowTargetPoint = _selectedTarget.Center + Vector3.Cross(Vector3.up, _arrowSpawnPoint.forward) *
                (1 - UnityEngine.Random.Range(phase.Accuracy, _maxTightnessPhase.Accuracy)) * _maxArrowDeviation * side;
        }
        else arrowTargetPoint = _arrowSpawnPoint.position + _arrowSpawnPoint.forward * _maxShootRange;

        return arrowTargetPoint;
    }

    private IEnumerator SelectNearestVisibleTargetRoutine()
    {
        while(true)
        {
            yield return _targetsCheckWait;

            if (_targetsHolder.IsEmpty)
            {
                _selectedTarget = null;
                continue;
            }

            Target nearestTarget = null;
            float distanceToNearestTarget = float.MaxValue;

            foreach(Target target in _targetsHolder.Targets)
            {
                if (CheckTargetVisibility(target) == false) continue;

                float distanceToTarget = Vector3.Distance(transform.position, target.Center);

                if(distanceToTarget < distanceToNearestTarget)
                {
                    distanceToNearestTarget = distanceToTarget;
                    nearestTarget = target;
                }
            }

            if(distanceToNearestTarget > _maxShootRange)
            {
                _selectedTarget = null;
                continue;
            }

            _selectedTarget = nearestTarget;
        }
    }

    private bool CheckTargetVisibility(Target target)
    {
        return !Physics.Linecast(_arrowSpawnPoint.position, target.Center, _obstacleLayers);
    }

    private void Update()
    {
        TryRotateToSelectedTarget();
        TryChangeStamina();
    }

    private void TryRotateToSelectedTarget()
    {
        if (IsDead) return;

        Vector3 rotateDirection = _selectedTarget != null ? (_selectedTarget.transform.position - transform.position).normalized :
            Vector3.forward;
        rotateDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void TryChangeStamina()
    {
        if (IsDead) return;

        if (_isAiming && Stamina > 0) Stamina -= _staminaWasteSpeed * Time.deltaTime;
        else if (Stamina != MaxStamina) Stamina += _staminaRecoverySpeed * Time.deltaTime;

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _maxShootRange);
    }
}
