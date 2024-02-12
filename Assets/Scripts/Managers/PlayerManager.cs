using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> m_players = new List<PlayerInput>();
    [SerializeField] private List<Transform> m_startingPoints;

    [Header("Player 1")]
    [SerializeField] private Transform m_player1StartingPoint;
    [SerializeField] private LayerMask m_player1LayerMask;

    [Header("Player 2")]
    [SerializeField] private Transform m_player2StartingPoint;
    [SerializeField] private LayerMask m_player2LayerMask;
    

    //References
    private PlayerInputManager m_playerInputManager;

    public static PlayerManager Instance;

    private void AddPlayer(PlayerInput player)
    {
        //Keeps a reference of the spawned player
        m_players.Add(player);
        player.DeactivateInput();

        if(m_players.Count == 2) {
            Debug.Log("Both players have joined.");
        }
    }

    private void RemovePlayer(PlayerInput player)
    {
        
    }

    public void SpawnPlayers()
    {
        if(m_players.Count != 2)    
        {
            Debug.LogError("Must need two players to spawn!");
            return;
        }
        //Spawn each player at their respective spawn point.
        m_players[0].transform.position = m_startingPoints[0].position;
        m_players[1].transform.position = m_startingPoints[1].position;
        //Label each player for collision
        m_players[0].gameObject.layer = LayerMask.NameToLayer("Player1");
        m_players[1].gameObject.layer = LayerMask.NameToLayer("Player2");
        m_players[0].transform.GetComponent<PlayerController>().SetEnemyLayer(m_player2LayerMask);
        m_players[1].transform.GetComponent<PlayerController>().SetEnemyLayer(m_player1LayerMask);
        foreach(PlayerInput player in m_players)
        {
            player.ActivateInput();
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

        m_playerInputManager =  GetComponent<PlayerInputManager>();
        
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
