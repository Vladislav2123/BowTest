using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("=== SPEED ===")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private float _lastTimeMove;

    private const float GRAVITY = -2;

    public CharacterController CharacterController { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public bool IsMoving => MoveDirection.sqrMagnitude > 0.1f;
    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    public void Move(Vector2 moveDirection, bool rotate = false)
    {
        if (CharacterController.enabled == false) return;

        MoveDirection = Vector3.right * moveDirection.x + Vector3.forward * moveDirection.y;

        CharacterController.Move(MoveDirection * _moveSpeed * Time.deltaTime);
        if (rotate) Rotate(MoveDirection);
        _lastTimeMove = Time.time;
    }

    public void Rotate(Vector3 rotateDirection)
    {
        if (rotateDirection == Vector3.zero) return;

        rotateDirection.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(rotateDirection);

        if (transform.rotation != lookRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (CharacterController.enabled == false) return;

        ApplyGravity();
        TryStop();
    }

    private void ApplyGravity()
    {
        CharacterController.Move(new Vector3(0, GRAVITY, 0) * Time.deltaTime);
    }

    private void TryStop()
    {
        if (MoveDirection == Vector3.zero || Time.time < _lastTimeMove + 0.1f) return;

        MoveDirection = Vector3.zero;
    }
}
