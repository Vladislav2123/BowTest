using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(EnemyAnimationHandler))]
[RequireComponent(typeof(EnemyStatesController))]
public class Enemy : NPC, IAlive
{
    private enum EnemyState { Idle, WalkAround, Attack}
    
    public event Action OnDeadEvent;
    public event Action OnHealthChangedEvent;

    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private EnemyState _originState;
    [SerializeField] private EnemyState _startState;
    [SerializeField] private float _attackDistance;

    [Header("=== Walk Around ===")]
    [SerializeField] private float _walkAroundRadius;
    [SerializeField] private Interval _walkAroundInterval;

    [Inject] private GameStateController _gameStateController;
    [Inject] private Archer _archer;
    private float _health;
    private bool _isDead;

    public Archer TargetArcher => _archer;
    public EnemyStatesController StatesController { get; private set; }
    public EnemyAnimationHandler AnimationHandler { get; private set; }
    public RagdollController RagdollController { get; private set; }
    public float AttackDistance => _attackDistance;

    public float WalkAroundRadius => _walkAroundRadius;
    public Interval WalkAroundInterval => _walkAroundInterval;
    public Vector3 StartPoint { get; private set; }

    public float MaxHealth => _maxHealth;
    public float Health
    {
        get => _health;
        set
        {
            if (value < 0) value = 0;
            _health = value;
            IsDead = _health == 0;

            OnHealthChangedEvent?.Invoke();
        }
    }
    public bool IsDead
    {
        get => _isDead;
        private set
        {
            if (_isDead == value) return;

            _isDead = value;
            if (value)
            {
                Dead();
                OnDeadEvent?.Invoke();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        StatesController = GetComponent<EnemyStatesController>();
        AnimationHandler = GetComponent<EnemyAnimationHandler>();
        RagdollController = GetComponent<RagdollController>();

        
        _gameStateController.OnGameEndedEvent += StatesController.ResetState;
        
        if (_gameStateController.IsPlaying == false &
            _startState != _originState)
            _gameStateController.OnGameStartedEvent += SetStartState;
    }

    private void Start()
    {
        StartPoint = transform.position;
        Health = MaxHealth;

        if (_gameStateController.IsPlaying == false) SetState(_originState);
        else SetStartState();
    }

    private void SetStartState()
    {
        SetState(_startState);
    }

    private void SetState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                StatesController.SetStateIdle();
                break;
            case EnemyState.Attack:
                StatesController.SetStateAttack();
                break;
            case EnemyState.WalkAround:
                StatesController.SetStateWalkAround();
                break;
        }
    }

    public void TryAttackArcher()
    {
        if (TargetArcher.IsDead) return;

        AnimationHandler.PlayAttackAnimation();
    }

    public void TryHitArcher()
    {
        if (TargetArcher.IsDead) return;

        _archer.TryDead();
    }

    private void Dead()
    {
        StatesController.ResetState();
        Movement.enabled = false;
        Movement.CharacterController.enabled = false;


        if (RagdollController != null) RagdollController.EnableRagdoll();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDistance);

        if(_originState == EnemyState.WalkAround)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(StartPoint == Vector3.zero ? transform.position : StartPoint, _walkAroundRadius);
        }
    }
}
