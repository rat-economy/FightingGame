using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterCombat))]
public class CharacterController : MonoBehaviour
{
    public CharacterAttribute Attributes;

    [HideInInspector] public bool IsMoving { get; private set; }
    [HideInInspector] public float CurrentHealth { get; private set; }
    [HideInInspector] public int EnemyLayer { get; set; }
    [HideInInspector] public MovementAxis MovementAxis { get; private set; }
    [HideInInspector] public Vector2 MovementVect { get; private set; }

    private bool _isJumping;
    private bool _isCrouching;

    private AudioManager audioManager;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private CharacterMovement m_characterMovement;
    private CharacterCombat m_characterCombat;

    public void RecieveDamage(float damage)
    {
        CurrentHealth -= damage;

        //Cancel what the player is doing when they recieve damage
        StopAllCoroutines();
        if (CurrentHealth <= 0)
        {
            m_animator.SetTrigger("Death");
            DisableInput();
        }
        else
        {
            m_animator.SetTrigger("Hurt");
            audioManager.PlaySoundOnce(Attributes.S_Hurt);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && _isJumping)
        {
            _isJumping = false;
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

        m_characterCombat = GetComponent<CharacterCombat>();
        m_characterMovement = GetComponent<CharacterMovement>();

        CurrentHealth = Attributes.MaxHealth;
    }

    private void UpdateMovementAxis(InputAction.CallbackContext context)
    {
     
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    protected void OnEnable()
    {
        i_move = m_player.FindAction("Move");

        //Attack callbacks
        m_player.FindAction("Light").performed += m_characterCombat.Light;
        m_player.FindAction("Heavy").performed += m_characterCombat.Heavy;
        m_player.FindAction("Special").performed += m_characterCombat.Special;
        m_player.FindAction("Block").performed += m_characterCombat.Block;

        //Update Movement Vector
        i_move.performed += ctx =>
        { 
            MovementVect = i_move.ReadValue<Vector2>();
        };

        //Check if able to crouch
        i_move.performed += ctx =>
        {
            if (MovementVect.y < -1 * Attributes.C_Deadzone && !_isCrouching && !_isJumping)
            {
                _isCrouching = true;
                m_characterMovement.Crouch();
            }
            else if (MovementVect.y > -1 * Attributes.C_Deadzone && _isCrouching)
            {
                _isCrouching = false;
                m_characterMovement.Stand();
            }
        };

        //Check if able to jump
        i_move.performed += ctx =>
        {
            //Check if already in the air
            if (MovementVect.y > Attributes.C_Deadzone && !_isJumping && !_isCrouching)
            {
                _isJumping = true;
                m_characterMovement.Jump();
            }
        };

        //TODO: Make these into arrow functions so analog stick not constnatly called
        i_move.performed += ctx =>
        {
            IsMoving = true;
            m_characterMovement.Move();
        };

        i_move.canceled += ctx =>
        {
            MovementVect = Vector2.zero;
        };

        i_move.canceled += ctx =>
        {
            if(_isCrouching && !_isJumping)
            {
                _isCrouching = false;
                m_characterMovement.Stand();
            }
        };

        i_move.canceled += ctx =>
        {
            IsMoving = false;
            m_characterMovement.StopMove();
        };

        i_move.performed += UpdateMovementAxis;

        m_player.Enable();
    }

    protected void OnDisable()
    {
        m_player.FindAction("Light").performed -= m_characterCombat.Light;
        m_player.FindAction("Heavy").performed -= m_characterCombat.Heavy;
        m_player.FindAction("Special").performed -= m_characterCombat.Special;
        m_player.FindAction("Block").performed -= m_characterCombat.Block;
        m_player.Disable();
    }
}