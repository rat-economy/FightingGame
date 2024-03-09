using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState state;

    private UM_InGame uiManager;
    private AudioManager audioManager;

    public List<Character> m_characters; //Pretty sure we could move this to FighterSelect.cs
    public static Character p1_selectedCharacter;
    public static Character p2_selectedCharacter;
    public static bool isTwoPlayer;

    public List<Level> m_levels;
    public static Level selectedLevel;

    private PlayerManager playerManager;
    private LevelManager levelManager;

    [SerializeField] Sound m_announcerCountdown;
    [SerializeField] Sound m_announcerVersus;

    private int playerOneWins = 0;
    private int playerTwoWins = 0;    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        uiManager = UM_InGame.Instance;
    }

    private void Start()
    {

    }

    //I have to do this cause Unity is a terrible game engine
    public void StartInitializeRound()
    {
        StartCoroutine(InitializeRound());
    }

    //Called in the main menu scene
    private IEnumerator InitializeRound()
    {
        //Transition to ingame scene
        

        //TODO: Setup loading screen
        yield return StartCoroutine(FindObjectOfType<Transition>().LoadGame());

        //Initialize the player prefabs into player manager
        playerManager = FindObjectOfType<PlayerManager>();
        levelManager = FindObjectOfType<LevelManager>();
        uiManager = UM_InGame.Instance;
        if (isTwoPlayer == false)
        {
            playerManager.SetOnePlayerInputToKeyboard();
            playerManager.SetPlayerPrefabs(p1_selectedCharacter.prefab, p2_selectedCharacter.prefab);
            playerManager.SpawnPlayers();
        }
        else
        {
            playerManager.SetTwoPlayerInputToTwoControllers();
            //playerManager.SetTwoPlayerInputToKeyboardAndController();
            playerManager.SetPlayerPrefabs(p1_selectedCharacter.prefab, p2_selectedCharacter.prefab);
            playerManager.SpawnPlayers();
        }

        

        //Call spawnsingleplayer() / spawnbothplayers()
        levelManager.SetupBackground();

        //Remove loading screen
        StartCoroutine(C_StartRound());
    }

    //Called once in game scene
    public IEnumerator C_StartRound()
    {
        if (audioManager == null)
        {
            Debug.Log("This is needed");
            audioManager = AudioManager.Instance;
        }
        AudioManager.Instance.PickFightSong();

        //Character Splash Screen
        uiManager.ShowVersusScreen();

        //Delay to Initialze stuff in scene
        yield return new WaitForSeconds(Constants.ANNOUNCER_DELAY); //TODO: REFACTOR

        //TODO: REFACTOR PLAY AUDIO IN UI MANAGER
        yield return StartCoroutine(audioManager.C_PlaySoundAndWait(p1_selectedCharacter.announcerLine1));
        yield return StartCoroutine(audioManager.C_PlaySoundAndWait(m_announcerVersus));
        yield return StartCoroutine(audioManager.C_PlaySoundAndWait(p2_selectedCharacter.announcerLine2));
        uiManager.HideVersusScreen();

        //Delay between Versus and Countdown
        yield return new WaitForSeconds(Constants.ANNOUNCER_DELAY*0.5f);

        //Countdown Timer
        StartCoroutine(audioManager.C_PlaySoundAndWait(m_announcerCountdown));
        yield return StartCoroutine(uiManager.C_Countdown());

        state = GameState.INGAME;
        playerManager.EnableInputs();
    }

    public void OnPlayerDeath(bool playerOneWonRound)
    {
        state = GameState.ROUNDEND;
        playerManager.DisableInputs();

        if (playerOneWonRound == true)
        {
            playerOneWins++;
            if (playerOneWins == Constants.WINS_NEEDED)
            {
                EndMatch(true, false);
                return;
            }
        }
        else if (playerOneWonRound == false)
        {
            playerTwoWins++;
            if (playerTwoWins == Constants.WINS_NEEDED)
            {
                EndMatch(true, true);
                return;
            }
        }
        audioManager.StopFightMusic(); //TODO: STOP
        StartInitializeRound();
    }

    public void EndMatch(bool matchEndInWin, bool playerTwoWonMatch)
    {
        playerOneWins = 0;
        playerTwoWins = 0;
        AudioManager.Instance.StopFightMusic();
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ResolveTie()
    {
        if (playerManager.m_player1.PlayerController.Attributes.MaxHealth > playerManager.m_player2.PlayerController.Attributes.MaxHealth)
        {
            OnPlayerDeath(true);
        }
        else
        {
            OnPlayerDeath(false);
        }
    }
}