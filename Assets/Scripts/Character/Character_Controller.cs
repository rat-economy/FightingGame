using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Character_Controller : MonoBehaviour
{
    [SerializeField] protected float c_deadzone = 0.3f;

    protected Character_Status Status;
    protected CharacterAttribute M;

    protected Rigidbody2D m_rigidbody;
    protected Animator m_animator;
    protected Vector2 m_moveDirection;

    protected AudioManager audioManager;

    protected abstract void Jump();
    protected abstract void Crouch();
    protected abstract void UnCrouch();
    protected abstract void Move(InputAction.CallbackContext context);
    protected abstract void StopMove(InputAction.CallbackContext context);

    void Update()
    {
        //Get input and update rigidbody velocity to match
        m_moveDirection = i_move.ReadValue<Vector2>();
        m_rigidbody.velocity = new Vector2(m_moveDirection.x * M.MoveSpeed, m_rigidbody.velocity.y);

        m_animator.SetBool("isMoving", Status.IsMoving);
        m_animator.SetBool("isJumping", Status.IsJumping);

        //Flips the player
        if (m_rigidbody.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (m_rigidbody.velocity.x < 0)
        {
            transform.rotation = Quaternion.identity;
        }

        //Check isJumping
        if (m_moveDirection.y > c_deadzone && !Status.IsJumping && !Status.IsCrouching)
        {
            Status.IsJumping = true;
            Jump();
        }

        //Check isCrouching
        if (m_moveDirection.y < -1 * c_deadzone && !Status.IsCrouching && !Status.IsJumping)
        {
            Status.IsCrouching = true;
            Crouch();
        }
        else if (m_moveDirection.y > -1 * c_deadzone && Status.IsCrouching)
        {
            Status.IsCrouching = false;
            UnCrouch();
        }
    }

    /*
       UNITY NEW INPUT SYSTEM INITIALIZATION CODE
   */
    [Header("Input Variables")]
    private InputActionAsset m_inputAsset;
    private InputActionMap m_player;
    public PlayerInput PlayerInput { get; private set; }

    protected InputAction i_move;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        m_inputAsset = PlayerInput.actions;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_player = m_inputAsset.FindActionMap("Player");

        Status = GetComponent<Player_Status>();
        M = Status.Attributes;
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    private void OnEnable()
    {
        i_move = m_player.FindAction("Move");

        i_move.performed += Move;
        i_move.canceled += StopMove;

        m_player.Enable();

    }

    private void OnDisable()
    {
        m_player.Disable();
    }
}
