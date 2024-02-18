using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState state;

    private UM_InGame uiManager;

    public List<Character> m_characters; //Pretty sure we could move this to FighterSelect.cs
    public static Character p1_selectedCharacter;
    public static Character p2_selectedCharacter;
    public static bool isTwoPlayer;

    public List<Level> m_levels;
    public static Level selectedLevel;

    private readonly int m_countdown = 3;

    private PlayerManager playerManager;
    private LevelManager levelManager;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        uiManager = UM_InGame.Instance;
    }

    private void Start()
    {
        
    }

    //I have to do this cause Unity is a terrible game engine
    public void StartInitializeRound(bool isTwoPlayer)
    {
        StartCoroutine(InitializeRound(isTwoPlayer));
    }

    //Called in the main menu scene
    private IEnumerator InitializeRound(bool isTwoPlayer)
    {
        Debug.Log("Ding!");
        //Transition to ingame scene
        SceneManager.LoadScene("MainScene");

        //TODO: Setup loading screen

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Dong!");

        //Initialize the player prefabs into player manager
        playerManager = FindObjectOfType<PlayerManager>();
        levelManager = FindObjectOfType<LevelManager>();
        uiManager = UM_InGame.Instance;
        Debug.Log(playerManager.gameObject.name);
        if (isTwoPlayer == false)
        {
            playerManager.SpawnSinglePlayer();
        }
        else
        {
            playerManager.SpawnBothPlayers();
        }
        
        //Call spawnsingleplayer() / spawnbothplayers()
        levelManager.SetupBackground();

        //Remove loading screen
        StartCoroutine(C_StartRound());
    }

    //Called once in game scene
    public IEnumerator C_StartRound()
    {
        uiManager.StartCountdown();
        yield return new WaitForSeconds(m_countdown);
        playerManager.EnableInputs();
    }
}