using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterAttribute my;

    private bool _isJumping;
    private bool _isCrouching;
    private bool _isMoving;
    private float m_currentHealth;

    [Header("Controller Variables")]
    [SerializeField, Range(0f, 1f)] private float c_deadzone = 0.3f;

    [Header("Attack Variables")]
    [SerializeField] private Transform m_attackPoint;
    [SerializeField, Range(0f, 2f)] private float m_attackRadius;
    [SerializeField] private int m_enemyLayer;

    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Vector2 m_moveDirection;

    private AudioManager audioManager;

    //TODO TEAMPOORARY< REMOVE AFTER YOU MAKE CHARACTER STATUS
    private PlayerCombat playerCombat;

    /*
        ANIMATION UPDATE & PLAYER INPUT PROCESSING

        TODO: Restructure code so it's not all in Update
        TODO: Update isJumping bool to be collider based, not y-velocity based
    */
    void Update()
    {
        //Get input and update rigidbody velocity to match
        m_moveDirection = i_move.ReadValue<Vector2>();
        m_rigidbody.velocity = new Vector2(m_moveDirection.x * my.MoveSpeed, m_rigidbody.velocity.y);

        m_animator.SetBool("isMoving", _isMoving);
        m_animator.SetBool("isJumping", _isJumping);

        //Flips the player
        if (m_rigidbody.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3 (0,180,0));
        }
        else if (m_rigidbody.velocity.x < 0)
        {
            transform.rotation = Quaternion.identity;
        }  

        //Check isJumping
        if (m_moveDirection.y > c_deadzone && !_isJumping && !_isCrouching)
        {
            _isJumping = true;
            Jump();
        }

        //Check isCrouching
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
        audioManager.PlaySoundOnce(my.s_jump);
        m_rigidbody.velocity += new Vector2(0, my.JumpSpeed);
    }

    private void Crouch() //S - Keyboard, Analog Stick Down - Controller 
    {
        audioManager.PlaySoundOnce(my.s_crouch);
        transform.localScale = new Vector3(1f, 0.5f, 1f);
        transform.localPosition -= new Vector3(0f, 0.1f, 0f);
    }

    private void UnCrouch() 
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition += new Vector3(0f, 0.1f, 0f);
    }

    private void Move(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundLooped(my.s_moving);
        _isMoving = true;
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        audioManager.StopSound(my.s_moving);
        _isMoving = false;
    }

    public void RecieveDamage(float damage)
    {
        // if (m_PlayerController.isDashing) return;
        m_currentHealth -= damage;

        //Cancel what the player is doing when they recieve damage
        StopAllCoroutines();
        if (m_currentHealth <= 0)
        {
            m_animator.SetTrigger("Death");
            m_player.Disable();

            //INGNORE COLLISIONS EXCEPT FOR GROUND
        }
        else
        {
            m_animator.SetTrigger("Hurt"); 
            audioManager.PlaySoundOnce(my.s_hurt);
           
        }
    }

    public void DisableInput()
    {
        m_player.Disable();
    }

    public void EnableInput()
    {
        m_player.Enable();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _isJumping)
        {
            _isJumping = false;
        }
    }

    public void SetEnemyLayer(int player)
    {
        m_enemyLayer = player;
        playerCombat.m_enemyLayer = player;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_attackPoint.position, m_attackRadius);
    }

    /*
        UNITY NEW INPUT SYSTEM INITIALIZATION CODE
    */
    [Header("Input Variables")]
    private InputActionAsset m_inputAsset;
    private InputActionMap m_player;
    public PlayerInput PlayerInput {get; private set;}

    private InputAction i_move;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        m_inputAsset = PlayerInput.actions;
        m_rigidbody = GetComponent<Rigidbody2D>(); 
        m_animator = GetComponent<Animator>();
        m_player = m_inputAsset.FindActionMap("Player");

        //TODO REMOVE
        playerCombat = GetComponent<PlayerCombat>();

        m_currentHealth = my.MaxHealth;
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