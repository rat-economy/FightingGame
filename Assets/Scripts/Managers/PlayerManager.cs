using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private readonly List<Transform> m_players = new();
    private readonly List<PlayerInput> m_playerInputs = new();
    private readonly List<PlayerController> m_playerControllers = new();

    [Header("Player 1")]
    [SerializeField] private Transform m_p1StartingPoint;
    [SerializeField] private LayerMask m_p1LayerMask;
    [SerializeField] private GameObject m_p1Prefab;

    [Header("Player 2")]
    [SerializeField] private Transform m_p2StartingPoint;
    [SerializeField] private LayerMask m_p2LayerMask;
    [SerializeField] private GameObject m_p2Prefab;

    [SerializeField] private GameObject m_dummyPrefab;
    

    //References
    private PlayerInputManager m_playerInputManager;

    public static PlayerManager Instance;

    private void AddPlayer(PlayerInput playerInput)
    {
        //Keeps a reference of the spawned player
        var transform = playerInput.transform;
        var playerController = playerInput.transform.GetComponent<PlayerController>();

        m_players.Add(transform);
        m_playerInputs.Add(playerInput);
        m_playerControllers.Add(playerController);

        //playerController.DisableInput();

        // if(m_playerInputs.Count == 1)
        // {
        //     Debug.Log("Player 1 has connected.");
        // } 
        // else if(m_playerInputs.Count == 2)
        // {
        //     Debug.Log("Player 2 has connected.");
        // }
    }

    public void DisableInputs()
    {
        foreach(var player in m_playerControllers)
        {
            player.DisableInput();
        }
    }

    public void EnableInputs()
    {
        foreach(var player in m_playerControllers)
        {
            player.EnableInput();
        }

    }

    private void RemovePlayer(PlayerInput player)
    {
        Debug.Log("Player Left");
    }

    public void SpawnBothPlayers()
    {
        if (m_players.Count != 1)
        {
            Debug.LogError("Alread spawned players!");
            return;
        }
        // if(m_playerInputs.Count < 2)    
        // {
        //     Debug.LogError("Must need two players to spawn!");
        //     return;
        // }

        Instantiate(m_p1Prefab);
        Instantiate(m_p2Prefab);
        
        //Spawn each player at their respective spawn point.
        m_players[0].position = m_p1StartingPoint.position;
        m_players[1].position = m_p2StartingPoint.position;
        //Label each player for collision detection
        m_players[0].gameObject.layer = LayerMask.NameToLayer("Player1");
        m_players[1].gameObject.layer = LayerMask.NameToLayer("Player2");
        m_playerControllers[0].SetEnemyLayer(m_p2LayerMask);
        m_playerControllers[1].SetEnemyLayer(m_p1LayerMask);
        foreach(var player in m_playerControllers)
        {
            player.EnableInput();
        }
    }

    public void SpawnSinglePlayer()
    {
        if(m_players.Count == 1)
        {
            Debug.LogError("Already spawned a player!");
            return;
        }
        Instantiate(m_p1Prefab);
        Instantiate(m_dummyPrefab, m_p2StartingPoint.position, Quaternion.identity);
        m_players[0].position = m_p1StartingPoint.position;
        m_players[0].gameObject.layer = LayerMask.NameToLayer("Player1");
        m_playerControllers[0].SetEnemyLayer(m_p2LayerMask);
        m_playerControllers[0].EnableInput();
    }

    private void Awake()
    {
        //SINGLETON CODE
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_playerInputManager = GetComponent<PlayerInputManager>();
        
    }

    private void OnEnable()
    {
        m_playerInputManager.onPlayerJoined += AddPlayer;
        m_playerInputManager.onPlayerLeft += RemovePlayer;

        InputSystem.onDeviceChange +=
        (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    Debug.Log("Device added: " + device);
                    break;
                case InputDeviceChange.Removed:
                    Debug.Log("Device removed: " + device);
                    break;
                case InputDeviceChange.ConfigurationChanged:
                    Debug.Log("Device configuration changed: " + device);
                    break;
            }
        };
    }
    
    private void OnDisable()
    {
        m_playerInputManager.onPlayerJoined -= AddPlayer;
        m_playerInputManager.onPlayerLeft -= RemovePlayer;
    }
}
