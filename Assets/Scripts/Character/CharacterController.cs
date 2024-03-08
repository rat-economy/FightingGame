using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterCombat))]
public class CharacterController : MonoBehaviour
{
    public CharacterAttribute Attributes;

    [HideInInspector] public bool IsMoving { get; set; }
    [HideInInspector] public bool IsBlocking { get; set; }
    [HideInInspector] public float CurrentHealth { get; private set; }
    [HideInInspector] public Vector3 Direction {get; set;}
    [HideInInspector] public int EnemyLayer { get; set; }
    [HideInInspector] public MovementAxis MovementAxis { get; private set; }
    [HideInInspector] public Vector2 MovementVect { get; private set; }
    [HideInInspector] public PlayerManager PlayerManager{ get; set; }

    [HideInInspector] public bool IsJumping {get; set;}
    [HideInInspector] public bool IsCrouching {get; set;}

    private AudioManager audioManager;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private CharacterMovement m_characterMovement;
    private CharacterCombat m_characterCombat;

    [HideInInspector] public bool isPlayerTwo = false;

    //Returns true if stance is broken
    //Returns false if stance is good
    public bool RecieveDamage(Attack attack)
    {
        PlayerManager.SetDirection();
        //Make a check for damage type
        //Light - Doesn't break stance, takes minimal damage
        //Heavy - Breaks stance, takes minimal damage 
        //Combo Finisher - Breaks stance, takes moderate damage
        if (IsBlocking)
        {
            if (attack.isComboFinisher)
            {
                CurrentHealth -= attack.Damage * Constant.COMBO_BLOCK_DAMAGE_MULTIPLIER;
            }
            else if (attack.InputType == InputType.L)
            {
                CurrentHealth -= attack.Damage * Constant.LIGHT_BLOCK_DAMAGE_MULTIPLIER;
            }
            else if (attack.InputType == InputType.H)
            {
                CurrentHealth -= attack.Damage * Constant.HEAVY_BLOCK_DAMAGE_MULTIPLIER;
            }
        }
        else 
        {
            CurrentHealth -= attack.Damage;
        }

        UM_InGame.Instance.UpdateHealthBar(CurrentHealth, isPlayerTwo);

        Debug.Log(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            StopAllCoroutines();
            m_animator.SetTrigger("Death");
            GameManager.Instance.OnPlayerDeath(isPlayerTwo);
            return true;
        }

        if (attack.isComboFinisher)
        {
            StartCoroutine(BreakStance(Constant.COMBO_KNOCKBACK_FORCE, Direction, Constant.COMBO_KNOCKBACK_DURATION));
        }
        else if (attack.InputType == InputType.L)
        {
            if(IsBlocking)
            {
                Knockback(Constant.LIGHT_KNOCKBACK_FORCE, Direction);
            }
            else
            {
                StartCoroutine(BreakStance(Constant.LIGHT_KNOCKBACK_FORCE, Direction, Constant.LIGHT_KNOCKBACK_DURATION));
            }
        }
        else if (attack.InputType == InputType.H)
        {
            StartCoroutine(BreakStance(Constant.HEAVY_KNOCKBACK_FORCE, Direction, Constant.HEAVY_KNOCKBACK_DURATION));
        }

        if (IsBlocking)
        {
            return false;
        }
        return true;
    }

    private IEnumerator BreakStance(float knockbackForce, Vector3 direction, float duration)
    {
        DisableInputInGame();
        m_characterMovement.Stand();
        m_characterMovement.StopMove();
        Knockback(knockbackForce, direction);
        m_characterCombat.StopAttack();
        m_characterCombat.ClearInputBuffer();
        
        m_animator.SetTrigger("Hurt");
        audioManager.PlaySoundOnce(Attributes.S_Hurt);
        yield return new WaitForSeconds(Attributes.HurtTime);
        EnableInputInGame();
        yield return null;
    }

    private void Knockback(float knockbackForce, Vector3 direction)
    {
        m_rigidbody.AddForce(knockbackForce * direction);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && IsJumping)
        {
            IsJumping = false;
        }
    }

    public void DisableInputInGame()
    {
        if(GameManager.state == GameState.INGAME)
        {
            m_player.Disable();
        }
    }

    public void EnableInputInGame()
    {
        if (GameManager.state == GameState.INGAME)
        {
            m_player.Enable();
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
        m_inputAsset = null;
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

    private void OnStartMove(InputAction.CallbackContext context)
    {
        MovementVect = i_move.ReadValue<Vector2>();

        if (MovementVect.y < -1 * Constant.CONTROLLER_DEADZONE && !IsCrouching && !IsJumping && !IsBlocking)
        {
            IsCrouching = true;
            m_characterMovement.Crouch();
        }
        else if (MovementVect.y > -1 * Constant.CONTROLLER_DEADZONE && IsCrouching)
        {
            IsCrouching = false;
            m_characterMovement.Stand();
        }

        //Check if already in the air
        if (MovementVect.y > Constant.CONTROLLER_DEADZONE && !IsJumping && !IsCrouching && !IsBlocking)
        {
            IsJumping = true;
            m_characterMovement.Jump();
        }
        m_characterMovement.Move();
    }

    private void OnStopMove(InputAction.CallbackContext context)
    {
        MovementVect = Vector2.zero;
        if(IsCrouching && !IsJumping)
        {
            IsCrouching = false;
            m_characterMovement.Stand();
        }
        m_characterMovement.StopMove();
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
        m_player.FindAction("Block").canceled += m_characterCombat.Unblock;
        
        i_move.performed += OnStartMove;
        i_move.performed += UpdateMovementAxis;
        i_move.canceled += OnStopMove;
        i_move.Enable();
        m_player.Enable();
    }

    protected void OnDisable()
    {
        m_player.FindAction("Light").performed -= m_characterCombat.Light;
        m_player.FindAction("Heavy").performed -= m_characterCombat.Heavy;
        m_player.FindAction("Special").performed -= m_characterCombat.Special;
        m_player.FindAction("Block").performed -= m_characterCombat.Block;
        m_player.FindAction("Block").canceled -= m_characterCombat.Unblock;

        i_move.performed -= OnStartMove;
        i_move.performed -= UpdateMovementAxis;
        i_move.canceled -= OnStopMove;
        i_move.Disable();
        m_player.Disable();
    }
}