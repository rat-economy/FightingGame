using System.Collections;
using UnityEngine;

public enum GameState
{
    SINGLEPLAYER,
    MULTIPLAYER
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState state;

    private PlayerManager playerManager;
    private UM_InGame uiManager;

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

    //Called on the player selection screen
    public void InitializeRound()
    {
        //Transition to ingame scene
        //Setup loading screen
        //Initialize the player prefabs into player manager
        //Call spawnsingleplayer() / spawnbothplayers()
        //Remove loading screen
        StartCoroutine(C_StartRound());
    }

    //Called once in game.
    public IEnumerator C_StartRound()
    {
        StartCoroutine(uiManager.Countdown());
        yield return new WaitForSeconds(m_countdown);
        playerManager.EnableInputs();
    }

}