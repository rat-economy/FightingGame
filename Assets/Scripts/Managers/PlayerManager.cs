using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Player 1")]
    [SerializeField] private Transform p1_startingPoint;
    [SerializeField] private LayerMask p1_layerMask;
    [SerializeField] private GameObject p1_prefab;
    [HideInInspector] public Actor m_player1;
    [HideInInspector] public Actor m_player2;

    [Header("Player 2")]
    [SerializeField] private Transform p2_startingPoint;
    [SerializeField] private LayerMask p2_layerMask;
    [SerializeField] private GameObject p2_prefab;

    [SerializeField] private GameObject m_dummyPrefab;
    

    //References
    private static PlayerInputManager _playerInputManager;
    private GameManager _gameManager;

    public static PlayerManager Instance;


    //Player initialization sequence
    //SetPlayerInput() -> SetPlayerPrefabs() -> SpawnPlayers()

    //Called when player selects mode in Main Menu
    public void SetOnePlayerInput(InputDevice p1Input)
    {
        m_player1.InputDevice = p1Input;        
        m_player2.InputDevice = p1Input;
    }
    
    public void SetTwoPlayerInput(InputDevice p1Input, InputDevice p2Input)
    {
        m_player1.InputDevice = p1Input;    
        m_player2.InputDevice = p2Input;
    }

    //Temporary functions, replace with an input selection screen!
    public void SetOnePlayerInputToKeyboard()
    {
        SetOnePlayerInput(Keyboard.current);
    }
    public void SetTwoPlayerInputToKeyboardAndController()
    {
        if(Gamepad.all.Count <= 1 || Keyboard.current == null)
        {
            Debug.LogError("Controller not connected! Two players isn't possible");
            return;
        }
        SetTwoPlayerInput(Keyboard.current, Gamepad.all[0]);
    }

     public void SetTwoPlayerInputToTwoControllers()
    {
        if(Gamepad.all.Count < 2)
        {
            Debug.LogError("Controller not connected! Two players isn't possible");
            return;
        }
        SetTwoPlayerInput(Gamepad.all[0], Gamepad.all[1]);
    }

    public void SetDirection()
    {
        if(m_player1.MyTransform.position.x >= m_player2.MyTransform.position.x)
        {
            m_player1.PlayerController.Direction = transform.right;
            m_player2.PlayerController.Direction = transform.right * -1;
        }
        else
        {
            m_player1.PlayerController.Direction = transform.right * -1;
            m_player2.PlayerController.Direction = transform.right;
        }
    }
    //----------------

    //Called when player locks in and starts the game
    public void SetPlayerPrefabs(GameObject p1, GameObject p2)
    {
        m_player1.Prefab = p1;
        m_player2.Prefab = p2;
    }

    //Temporaru Function before implementing lock in screen
    public void SetPlayerPrefabsFromPlayerManager()
    {
        m_player1.Prefab = p1_prefab;
        m_player2.Prefab = p2_prefab;
    }

    //Called during game initialization sequence
    public void SpawnPlayers()
    {
        m_player1.StartingPoint = p1_startingPoint;
        m_player1.LayerMask = p1_layerMask;
        m_player2.StartingPoint = p2_startingPoint;
        m_player2.LayerMask = p2_layerMask;
        m_player1.Spawn();
        m_player2.Spawn(); 
        m_player1.MyTransform.gameObject.layer = LayerMask.NameToLayer("Player1");
        m_player2.MyTransform.gameObject.layer = LayerMask.NameToLayer("Player2");
        foreach (Transform child in m_player1.MyTransform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Player1");
        }
        foreach (Transform child in m_player2.MyTransform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Player2");
        }
        m_player1.PlayerController.EnemyLayer = p2_layerMask;
        m_player2.PlayerController.EnemyLayer = p1_layerMask;
        m_player1.PlayerController.PlayerManager = this;
        m_player2.PlayerController.PlayerManager = this;
        m_player2.PlayerController.isPlayerTwo = true;
        DisableInputs();
    }

    public void DisableInputs()
    {
        m_player1.PlayerController.DisableInput();
        m_player2.PlayerController.DisableInput();
    }

    public void EnableInputs()
    {
        m_player1.PlayerController.EnableInput();
        m_player2.PlayerController.EnableInput();
    }

    private void RemovePlayer(PlayerInput player)
    {
        Debug.Log("Player Left");
    }
    
    private void Awake()
    {
        //SINGLETON CODE
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(this);

        _playerInputManager = GetComponent<PlayerInputManager>();
        m_player1 = ScriptableObject.CreateInstance<Actor>();
        m_player2 = ScriptableObject.CreateInstance<Actor>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        // //_playerInputManager.onPlayerJoined += AddPlayer;
        // _playerInputManager.onPlayerLeft += RemovePlayer;

        // InputSystem.onDeviceChange +=
        // (device, change) =>
        // {
        //     switch (change)
        //     {
        //         case InputDeviceChange.Added:
        //             Debug.Log("Device added: " + device);
        //             break;
        //         case InputDeviceChange.Removed:
        //             Debug.Log("Device removed: " + device);
        //             break;
        //         case InputDeviceChange.ConfigurationChanged:
        //             Debug.Log("Device configuration changed: " + device);
        //             break;
        //     }
        // };
    }
    
    private void OnDisable()
    {
        // //_playerInputManager.onPlayerJoined -= AddPlayer;
        // _playerInputManager.onPlayerLeft -= RemovePlayer;
    }
}