using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //TODO: Make player-dependant values into a scriptable object
    [SerializeField] private float m_moveSpeed = 1.0f;
    [SerializeField] private float m_jumpSpeed = 7.0f;

    private bool _isJumping;
    private bool _isCrouching;
    private bool _isMoving;

    [Header("Controller Variables")]
    [SerializeField, Range(0f, 1f)] private float c_deadzone = 0.3f;


    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Vector2 m_moveDirection;
    private SpriteRenderer m_spriteRenderer;

    /*
        ANIMATION UPDATE & PLAYER INPUT PROCESSING

        TODO: Restructure code so it's not all in Update
        TODO: Update isJumping bool to be collider based, not y-velocity based
    */
    void Update()
    {
        //Get input and update rigidbody velocity to match
        m_moveDirection = i_move.ReadValue<Vector2>();
        m_rigidbody.velocity = new Vector2(m_moveDirection.x * m_moveSpeed, m_rigidbody.velocity.y);

        m_animator.SetBool("isMoving", _isMoving);
        m_animator.SetBool("isJumping", _isJumping);
        if (m_rigidbody.velocity.x > 0)
        {
            m_spriteRenderer.flipX = true;
        }
        else if (m_rigidbody.velocity.x < 0)
        {
            m_spriteRenderer.flipX = false;
        }  

        if (m_moveDirection.y > c_deadzone && !_isJumping && !_isCrouching)
        {
            _isJumping = true;
            Jump();
        }
        else if(m_rigidbody.velocity.y == 0 && m_moveDirection.y <= c_deadzone)
        {
            _isJumping = false;
        }
        if (m_moveDirection.y < -1 * c_deadzone && !_isCrouching && !_isJumping)
        {
            _isCrouching = true;
            Crouch();
        }
        else if(m_moveDirection.y > -1 * c_deadzone && _isCrouching)
        {
            _isCrouching = false;
            UnCrouch();
        }
    }

    /*-----------------------
    PLAYER CONTROL METHODS
    -----------------------*/
    private void Jump() //W - Keyboard, Analog Stick Up - Controller 
    {
        m_animator.SetTrigger("Jump");
        m_rigidbody.velocity += new Vector2(0, m_jumpSpeed);
    }

    private void Crouch() //S - Keyboard, Analog Stick Down - Controller 
    {
        transform.localScale = new Vector3(1f, 0.5f, 1f);
        transform.localPosition -= new Vector3(0f, 0.5f, 0f);
    }

    private void UnCrouch() 
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition += new Vector3(0f, 0.5f, 0f);
    }

    private void Move(InputAction.CallbackContext context)
    {
        _isMoving = true;
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        _isMoving = false;
    }

    private void Light(InputAction.CallbackContext context)
    {
        m_animator.SetTrigger("LightAttack");
    }

    private void Heavy(InputAction.CallbackContext context)
    {
        Debug.Log("Heavy Attack Performed.");
    }

    private void Special(InputAction.CallbackContext context)
    {
        Debug.Log("Special Attack Performed.");
    }

    private void Block(InputAction.CallbackContext context)
    {
        Debug.Log("Block Attack Performed.");
    }

    /*
        UNITY NEW INPUT SYSTEM INITIALIZATION CODE
    */
    [Header("Input Variables")]
    private InputActionAsset m_inputAsset;
    private InputActionMap m_player;
    private PlayerInput m_playerInput;

    private InputAction i_move;
    private InputAction i_light;
    private InputAction i_heavy;
    private InputAction i_special;
    private InputAction i_block;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        m_inputAsset = GetComponent<PlayerInput>().actions;
        m_rigidbody = GetComponent<Rigidbody2D>(); 
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_player = m_inputAsset.FindActionMap("Player");
    }
    private void OnEnable()
    {
        i_move = m_player.FindAction("Move");

        i_move.performed += Move;
        i_move.canceled += StopMove;

        m_player.FindAction("Light").performed += Light;
        m_player.FindAction("Heavy").performed += Heavy;
        m_player.FindAction("Special").performed += Special;
        m_player.FindAction("Block").performed += Block;
        m_player.Enable();

    }

    private void OnDisable()
    {
        m_player.FindAction("Light").performed -= Light;
        m_player.FindAction("Heavy").performed -= Heavy;
        m_player.FindAction("Special").performed -= Special;
        m_player.FindAction("Block").performed -= Block;
        m_player.Disable();
    }
}
