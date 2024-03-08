using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UM_InGame : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject[] m_coundownGraphics;
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private GameObject statusUI;
    [SerializeField] private Image player1Image;
    [SerializeField] private Image player2Image;

    [SerializeField] private PlayerManager playerManager;

    [SerializeField] private Image player1Doodle;
    [SerializeField] private Image player2Doodle;
    [SerializeField] private Image player1HealthBar;
    [SerializeField] private Image player2HealthBar;
    [SerializeField] private TextMeshProUGUI timerText;
    public static UM_InGame Instance {get; private set;}

    private float timeRemaining;
    private bool timerRunning = false;

    public void Pause()
    {
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        playerManager.DisableInputs();
    }

    public void Resume()
    {
        m_pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        playerManager.EnableInputs();
    }

    public void Quit()
    {
        GameManager.Instance.EndMatch(false, false);
    }

    public void SetupGameUI()
    {
        StartCoroutine(VSText());
    }

    private IEnumerator Countdown()
    {
        //3
        m_coundownGraphics[0].SetActive(true);
        yield return new WaitForSeconds(1);
        //2
        m_coundownGraphics[0].SetActive(false);
        m_coundownGraphics[1].SetActive(true);
        yield return new WaitForSeconds(1);
        //1
        m_coundownGraphics[1].SetActive(false);
        m_coundownGraphics[2].SetActive(true);
        yield return new WaitForSeconds(1);
        //FIGHT
        
        yield return new WaitForSeconds(1);
        m_coundownGraphics[2].SetActive(false);
        yield return null;

        SetupStatusUI();
    }

    private IEnumerator VSText()
    {
        splashScreen.SetActive(true);
        player1Image.sprite = GameManager.p1_selectedCharacter.charSplash;
        player2Image.sprite = GameManager.p2_selectedCharacter.charSplash;

        yield return new WaitForSeconds(Constants.SPLASH_COUNTDOWN);
        splashScreen.SetActive(false);
        StartCoroutine(Countdown());
        
    }

    public void SetupStatusUI()
    {
        statusUI.SetActive(true);
        timeRemaining = Constants.ROUND_LENGTH;
        timerRunning = true;


        player1Doodle.sprite = GameManager.p1_selectedCharacter.doodle;
        player2Doodle.sprite = GameManager.p2_selectedCharacter.doodle;
    }

    public void UpdateHealthBar(float health, bool isPlayerTwo)
    {
        if (isPlayerTwo == false)
        {
            player1HealthBar.fillAmount = health / playerManager.m_player1.PlayerController.Attributes.MaxHealth;
        }
        else
        {
            player2HealthBar.fillAmount = health / playerManager.m_player1.PlayerController.Attributes.MaxHealth;
        }
    }

    private void Start()
    {
        //playerManager = PlayerManager.Instance;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        Instance = this;

        
    }

    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timeRemaining).ToString();

            if (timeRemaining <= 0)
            {
                timerRunning = false;
                Debug.Log("Times up!");
                GameManager.Instance.ResolveTie();
            }
        }
    }
    
}
