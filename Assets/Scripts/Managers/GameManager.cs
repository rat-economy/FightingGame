using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState state;

    private PlayerManager playerManager;
    private UM_InGame uiManager;

    [SerializeField] private List<Character> m_characters;
    private Character p1_selectedCharacter;
    private Character p2_selectedCharacter;

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

        //Setup loading screen

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

    //Move to playerselect script
    private void SelectCharacter(CharacterName name, int player)
    {
        Character c = FindCharacter(name);
        if (player == 0)
        {
            p1_selectedCharacter = c;
        }
        else 
        {
            p2_selectedCharacter = c;
        }
    }

    //Move to player select script
    private Character FindCharacter(CharacterName name)
    {
        foreach(Character c in m_characters)
        {
            if(c.name == name)
            {
                return c;
            }
        }
        Debug.LogError("Cannot find character. Add the character to GameManager.");
        return new Character();
    }

    //Move to player slect script
    public void SelectRandomCharacters()
    {

    }

}

