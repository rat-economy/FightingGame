// using UnityEngine;
// using UnityEngine.InputSystem;

// public abstract class Character_Controller : MonoBehaviour
// {
//     private Character_Status m;
//     private Rigidbody2D m_rigidbody;
//     private Animator m_animator;
//     private Vector2 m_moveDirection;

//     private AudioManager audioManager;

//     protected abstract void Jump();
//     protected abstract void Crouch();
//     protected abstract void UnCrouch();
//     protected abstract void Move();
//     protected abstract void StopMove();

//     void Update()
//     {
//         //Get input and update rigidbody velocity to match
//         m_moveDirection = i_move.ReadValue<Vector2>();
//         m_rigidbody.velocity = new Vector2(m_moveDirection.x * my.moveSpeed, m_rigidbody.velocity.y);

//         m_animator.SetBool("isMoving", m.IsMoving);
//         m_animator.SetBool("isJumping", m.IsJumping);

//         //Flips the player
//         if (m_rigidbody.velocity.x > 0)
//         {
//             transform.rotation = Quaternion.Euler(new Vector3 (0,180,0));
//         }
//         else if (m_rigidbody.velocity.x < 0)
//         {
//             transform.rotation = Quaternion.identity;
//         }  

//         //Check isJumping
//         if (m_moveDirection.y > c_deadzone && !m.IsJumping && !m.IsCrouching)
//         {
//             m.IsJumping = true;
//             Jump();
//         }

//         //Check isCrouching
//         if (m_moveDirection.y < -1 * c_deadzone && !m.IsCrouching && !m.IsJumping)
//         {
//             m.IsCrouching = true;
//             Crouch();
//         }
//         else if(m_moveDirection.y > -1 * c_deadzone && m.IsCrouching)
//         {
//             m.IsCrouching = false;
//             UnCrouch();
//         }
//     }

//      /*
//         UNITY NEW INPUT SYSTEM INITIALIZATION CODE
//     */
//     [Header("Input Variables")]
//     private InputActionAsset m_inputAsset;
//     private InputActionMap m_player;
//     public PlayerInput PlayerInput {get; private set;}

//     private InputAction i_move;

//     private void Awake()
//     {
//         PlayerInput = GetComponent<PlayerInput>();
//         m_inputAsset = PlayerInput.actions;
//         m_rigidbody = GetComponent<Rigidbody2D>(); 
//         m_animator = GetComponent<Animator>();
//         m_player = m_inputAsset.FindActionMap("Player");

//         //TODO REMOVE
//         playerCombat = GetComponent<PlayerCombat>();

//         m_currentHealth = my.maxHealth;
//     }

//     private void Start()
//     {
//         audioManager = AudioManager.Instance;
    
//     }

//     private void OnEnable()
//     {
//         i_move = m_player.FindAction("Move");

//         i_move.performed += Move;
//         i_move.canceled += StopMove;

//         m_player.Enable();

//     }

//     private void OnDisable()
//     {
//         m_player.Disable();
//     }
// }
