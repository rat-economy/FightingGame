using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private readonly List<Transform> m_player = new();
    private readonly List<PlayerInput> m_playerInputs = new();
    private readonly List<PlayerController> m_playerControllers = new();

    [Header("Player 1")]
    [SerializeField] private Transform m_player1StartingPoint;
    [SerializeField] private LayerMask m_player1LayerMask;

    [Header("Player 2")]
    [SerializeField] private Transform m_player2StartingPoint;
    [SerializeField] private LayerMask m_player2LayerMask;
    

    //References
    private PlayerInputManager m_playerInputManager;

    public static PlayerManager Instance;

    private void AddPlayer(PlayerInput playerInput)
    {
        //Keeps a reference of the spawned player
        var transform = playerInput.transform;
        var playerController = playerInput.transform.GetComponent<PlayerController>();

        m_player.Add(transform);
        m_playerInputs.Add(playerInput);
        m_playerControllers.Add(playerController);

        //playerController.DisableInput();

        if(m_playerInputs.Count == 2) {
            Debug.Log("Both players have joined.");
        }
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

    public void SpawnPlayers()
    {
        if(m_playerInputs.Count != 2)    
        {
            Debug.LogError("Must need two players to spawn!");
            return;
        }
        //Spawn each player at their respective spawn point.
        m_player[0].position = m_player1StartingPoint.position;
        m_player[1].position = m_player2StartingPoint.position;
        //Label each player for collision detection
        m_player[0].gameObject.layer = LayerMask.NameToLayer("Player1");
        m_player[1].gameObject.layer = LayerMask.NameToLayer("Player2");
        m_playerControllers[0].SetEnemyLayer(m_player2LayerMask);
        m_playerControllers[0].SetEnemyLayer(m_player1LayerMask);
        foreach(var player in m_playerControllers)
        {
            player.EnableInput();
        }
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
            Destroy(this.gameObject);
        }

        m_playerInputManager = GetComponent<PlayerInputManager>();
        
    }

    private void OnEnable()
    {
        m_playerInputManager.onPlayerJoined += AddPlayer;
        m_playerInputManager.onPlayerLeft += RemovePlayer;
    }
    
    private void OnDisable()
    {
        m_playerInputManager.onPlayerJoined -= AddPlayer;
        m_playerInputManager.onPlayerLeft -= RemovePlayer;
    }
}
