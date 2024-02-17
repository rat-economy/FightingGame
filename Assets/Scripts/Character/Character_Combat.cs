using UnityEngine;

public class Character_Combat : MonoBehaviour
{
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
