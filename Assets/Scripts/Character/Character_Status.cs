using UnityEngine;
using UnityEngine.InputSystem;

public class Character_Status : MonoBehaviour
{
    public CharacterAttribute Attributes;

    [HideInInspector] public bool IsJumping { get; set; }
    [HideInInspector] public bool IsCrouching { get; set; }
    [HideInInspector] public bool IsMoving { get; set; }
    [HideInInspector] public float CurrentHealth { get; private set; }
    [HideInInspector] public int EnemyLayer { get; set; }


    private AudioManager audioManager;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;

    public void RecieveDamage(float damage)
    {
        // if (m_PlayerController.isDashing) return;
        CurrentHealth -= damage;

        //Cancel what the player is doing when they recieve damage
        StopAllCoroutines();
        if (CurrentHealth <= 0)
        {
            m_animator.SetTrigger("Death");
            m_player.Disable();

            //INGNORE COLLISIONS EXCEPT FOR GROUND
        }
        else
        {
            m_animator.SetTrigger("Hurt");
            audioManager.PlaySoundOnce(Attributes.s_hurt);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && IsJumping)
        {
            IsJumping = false;
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

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        m_inputAsset = PlayerInput.actions;
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_player = m_inputAsset.FindActionMap("Player");

        CurrentHealth = Attributes.MaxHealth;
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    protected void OnEnable()
    {
        m_player.Enable();
    }

    protected void OnDisable()
    {
        m_player.Disable();
    }
}
