using System.Collections;
using UnityEngine;

public class UM_InGame : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject[] m_coundownGraphics;

    private PlayerManager playerManager;
    public static UM_InGame Instance {get; private set;}

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

    }

    public IEnumerator Countdown()
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
        m_coundownGraphics[2].SetActive(false);
        m_coundownGraphics[3].SetActive(true);
        yield return new WaitForSeconds(1);
        m_coundownGraphics[3].SetActive(false);
        yield return null;
    }

    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
