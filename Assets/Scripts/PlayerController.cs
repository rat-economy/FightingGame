using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 1.0f;
    [SerializeField] private float m_jumpSpeed = 7.0f;

    private bool _isJumping;
    private bool _isCrouching;
    private bool _isMoving;

    [Header("Controller Variables")]
    [SerializeField, Range(0f, 1f)] private float c_deadzone = 0.3f;

    private PlayerInput m_playerInput;
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Vector2 m_moveDirection;
    private SpriteRenderer m_spriteRenderer;

    void Update()
    {
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

    private void Jump()
    {
        m_animator.SetTrigger("Jump");
        m_rigidbody.velocity += new Vector2(0, m_jumpSpeed);
    }

    private void Crouch()
    {
        transform.localScale = new Vector3(1f, 0.5f, 1f);
        transform.localPosition -= new Vector3(0f, 0.5f, 0f);
    }

    private void UnCrouch()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition += new Vector3(0f, 0.5f, 0f);
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


    [Header("Input Variables")]
    private InputAction i_move;
    private InputAction i_light;
    private InputAction i_heavy;
    private InputAction i_special;
    private InputAction i_block;

    private void OnEnable()
    {

        i_move = m_playerInput.Player.Move;
        i_light = m_playerInput.Player.Light;
        i_heavy = m_playerInput.Player.Heavy;
        i_special = m_playerInput.Player.Special;
        i_block = m_playerInput.Player.Block;

        i_move.Enable();
        i_light.Enable();
        i_heavy.Enable();
        i_special.Enable();
        i_block.Enable();

        m_playerInput.Player.Move.performed += context => {
            m_moveDirection = i_move.ReadValue<Vector2>();
            _isMoving = true;
        };
        m_playerInput.Player.Move.canceled += context =>
        {
            m_moveDirection = Vector2.zero;
            _isMoving = false;
        };

        i_light.performed += Light;
        i_heavy.performed += Heavy;
        i_special.performed += Special;
        i_block.performed += Block;


    }

    private void OnDisable()
    {
        i_move.Disable();
        i_light.Disable();
        i_heavy.Disable();
        i_special.Disable();
        i_block.Disable();
    }

    void OnValidate()
    {
        m_rigidbody = GetComponent<Rigidbody2D>(); 
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        m_playerInput = new PlayerInput();
    }

    
}
