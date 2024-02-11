using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> m_players = new List<PlayerInput>();
    [SerializeField] private List<Transform> m_startingPoints;
    [SerializeField] private List<LayerMask> m_playerLayers;

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
        }
        //For each player, spawn them at their respective spawn point.
        m_players[0].transform.position = m_startingPoints[0].position;
        m_players[1].transform.position = m_startingPoints[1].position;
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
