using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState state;

    private PlayerManager playerManager;
    private UM_InGame uiManager;

    public List<Character> m_characters; //Pretty sure we could move this to FighterSelect.cs
    public static Character p1_selectedCharacter;
    public static Character p2_selectedCharacter;

    private readonly int m_countdown = 3;

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
    }

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        uiManager = UM_InGame.Instance;
    }

    //Called in the main menu scene
    public void InitializeRound()
    {
        //Transition to ingame scene
        SceneManager.LoadScene("MainScene");

        //TODO: Setup loading screen

        //Initialize the player prefabs into player manager
        
        //Call spawnsingleplayer() / spawnbothplayers()

        //Remove loading screen
        StartCoroutine(C_StartRound());
    }

    //Called once in game scene
    public IEnumerator C_StartRound()
    {
        StartCoroutine(uiManager.Countdown());
        yield return new WaitForSeconds(m_countdown);
        playerManager.EnableInputs();
    }
}