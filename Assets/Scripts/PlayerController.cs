using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //TODO: Make player-dependant values into a scriptable object

    [SerializeField] private PlayerAttributes my;

    private bool _isJumping;
    private bool _isCrouching;
    private bool _isMoving;
    private float m_currentHealth;

    [Header("Controller Variables")]
    [SerializeField, Range(0f, 1f)] private float c_deadzone = 0.3f;

    [Header("Attack Variables")]
    [SerializeField] private Transform m_attackPoint;
    [SerializeField, Range(0f, 10f)] private float m_attackRadius;
    [SerializeField] private LayerMask m_enemyLayer;


    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Vector2 m_moveDirection;
    private SpriteRenderer m_spriteRenderer;

    private AudioManager audioManager;

    /*
        ANIMATION UPDATE & PLAYER INPUT PROCESSING

        TODO: Restructure code so it's not all in Update
        TODO: Update isJumping bool to be collider based, not y-velocity based
    */
    void Update()
    {
        //Get input and update rigidbody velocity to match
        m_moveDirection = i_move.ReadValue<Vector2>();
        m_rigidbody.velocity = new Vector2(m_moveDirection.x * my.moveSpeed, m_rigidbody.velocity.y);

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
        else if(m_rigidbody.velocity.y == 0 && m_moveDirection.y <= c_deadzone)
        {
            _isJumping = false;
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
        m_rigidbody.velocity += new Vector2(0, my.jumpSpeed);
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

    private void Light(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(my.s_light);
        m_animator.SetTrigger("LightAttack");

        StartCoroutine(Attack(my.lightDamage, my.lightWindup, my.lightCooldown));
    }

    private void Heavy(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(my.s_heavy);
        Debug.Log("Heavy Attack Performed.");
    }

    private void Special(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(my.s_special);
        Debug.Log("Special Attack Performed.");
    }

    private void Block(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(my.s_block);
        Debug.Log("Block Attack Performed.");
    }

    private IEnumerator Attack(float damage, float windup, float cooldown)
    {
        m_player.Disable();
        //Windup delay
        yield return new WaitForSeconds(windup);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRadius, m_enemyLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            //TODO: FIX CODE SO TAKING DAMAGE, ATTACKING, AND PLAYER CONTROLELR ARE SEPARATE SCRIPTS
            if(enemy.transform.TryGetComponent<PlayerController>(out var pc))
                enemy.transform.GetComponent<PlayerController>().RecieveDamage(damage);
            else enemy.transform.GetComponent<DummyController>().RecieveDamage(damage);
        }

        //Cooldown delay
        yield return new WaitForSeconds(cooldown);
        m_player.Enable();

        yield return null;
    }

    private void RecieveDamage(float damage)
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
            Die();
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

    private void Die()
    {

    }

    public void SetEnemyLayer(LayerMask player)
    {
        m_enemyLayer = player;
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
    public PlayerInput m_playerInput {get; private set;}

    private InputAction i_move;
    private InputAction i_light;
    private InputAction i_heavy;
    private InputAction i_special;
    private InputAction i_block;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        audioManager = AudioManager.Instance;

        m_playerInput = GetComponent<PlayerInput>();
        m_inputAsset = m_playerInput.actions;
        m_rigidbody = GetComponent<Rigidbody2D>(); 
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_player = m_inputAsset.FindActionMap("Player");

        m_currentHealth = my.maxHealth;
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