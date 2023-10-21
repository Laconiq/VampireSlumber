using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Controls controls;
    public PlayerInput playerInput;
    private float _horizontalInput;
    private Rigidbody2D _rigidbody2D;
    public float speed;
    [HideInInspector] public float currentSpeed;
    [SerializeField] private float jumpForce = 5;
    private bool _isGrounded;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private MMF_Player footstepMmfPlayer;
    [SerializeField] private MMF_Player jumpMmfPlayer;
    public void Awake()
    {
        currentSpeed = speed;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        controls = new Controls();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.performed += OnJumpPerformed;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void OnDestroy()
    {
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<Vector2>().x;
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _horizontalInput = 0f;
    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (_isGrounded)
            Jump();
    }
    private void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        jumpMmfPlayer.PlayFeedbacks();
    }
    private void Update()
    {
        _isGrounded = Physics2D.IsTouchingLayers(_rigidbody2D.GetComponent<Collider2D>(), LayerMask.GetMask("Ground"));
        _rigidbody2D.velocity = new Vector2(_horizontalInput * currentSpeed, _rigidbody2D.velocity.y);
        if (_horizontalInput > 0)
            _spriteRenderer.flipX = false;
        else if (_horizontalInput < 0)
            _spriteRenderer.flipX = true;
    }

    public void PlayFootstepFeedback()
    {
        footstepMmfPlayer.PlayFeedbacks();
    }
}
