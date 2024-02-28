using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private readonly List<Transform> m_players = new();
    private readonly List<PlayerInput> m_playerInputs = new();
    private readonly List<PlayerController> m_playerControllers = new();

    [Header("Player 1")]
    [SerializeField] private Transform p1_startingPoint;
    [SerializeField] private LayerMask p1_layerMask;
    [SerializeField] private GameObject p1_prefab;

    [Header("Player 2")]
    [SerializeField] private Transform p2_startingPoint;
    [SerializeField] private LayerMask p2_layerMask;
    [SerializeField] private GameObject p2_prefab;

    [SerializeField] private GameObject m_dummyPrefab;
    

    //References
    private PlayerInputManager _playerInputManager;
    private GameManager _gameManager;

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

    //Run before SPAWNING the players
    public void InitializePlayers(GameObject p1, GameObject p2)
    {

    }

    public void SpawnBothPlayers()
    {
        if (m_players.Count >= 1)
        {
            Debug.LogError("Alread spawned players!");
            return;
        }
        // if(m_playerInputs.Count < 2)    
        // {
        //     Debug.LogError("Must need two players to spawn!");
        //     return;
        // }

        Instantiate(GameManager.p1_selectedCharacter.prefab);
        Instantiate(GameManager.p2_selectedCharacter.prefab);
        
        //Spawn each player at their respective spawn point.
        m_players[0].position = p1_startingPoint.position;
        m_players[1].position = p2_startingPoint.position;
        //Label each player for collision detection
        m_players[0].gameObject.layer = LayerMask.NameToLayer("Player1");
        m_players[1].gameObject.layer = LayerMask.NameToLayer("Player2");
        m_playerControllers[0].SetEnemyLayer(p2_layerMask);
        m_playerControllers[1].SetEnemyLayer(p1_layerMask);
        DisableInputs();
        StartCoroutine(_gameManager.C_StartRound());
    }

    public void SpawnSinglePlayer()
    {
        if(m_players.Count == 1)
        {
            Debug.LogError("Already spawned a player!");
            return;
        }
        Instantiate(GameManager.p1_selectedCharacter.prefab);
        Instantiate(m_dummyPrefab, p2_startingPoint.position, Quaternion.identity);
        m_players[0].position = p1_startingPoint.position;
        m_players[0].gameObject.layer = LayerMask.NameToLayer("Player1");
        m_playerControllers[0].SetEnemyLayer(p2_layerMask);
        DisableInputs();
        StartCoroutine(_gameManager.C_StartRound());
    }

    private void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;
        _playerInputManager.onPlayerLeft += RemovePlayer;

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
        _playerInputManager.onPlayerJoined -= AddPlayer;
        _playerInputManager.onPlayerLeft -= RemovePlayer;
    }
}
