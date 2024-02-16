using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private CharacterAttribute my;
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Vector2 m_moveDirection;

    //Booleans
    private bool _isJumping;
    private bool _isCrouching;
    private bool _isMoving;

    //References
    private AudioManager audioManager;

    [Header("Attack Variables")]
    [SerializeField] private Transform m_attackPoint;
    [SerializeField, Range(0f, 2f)] private float m_attackRadius;
    
    //TODO UPDATE ENEMYLATER TO BE IN THE STATUS SCRIPT
    [HideInInspector] public int m_enemyLayer;

    [Header("Input Variables")]
    private InputActionAsset m_inputAsset;
    private InputActionMap m_player;
    public PlayerInput PlayerInput {get; private set;}

    private InputAction i_move;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        audioManager = AudioManager.Instance;

        PlayerInput = GetComponent<PlayerInput>();
        m_inputAsset = PlayerInput.actions;
        m_rigidbody = GetComponent<Rigidbody2D>(); 
        m_animator = GetComponent<Animator>();
        m_player = m_inputAsset.FindActionMap("Player");

    }


    private void Light(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(my.s_light);
        m_animator.SetTrigger("LightAttack");

        StartCoroutine(C_Attack(my.lightDamage, my.lightWindup, my.lightCooldown));
    }


    private IEnumerator C_Attack(float damage, float windup, float cooldown)
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

    private void OnEnable()
    {
        i_move = m_player.FindAction("Move");

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
