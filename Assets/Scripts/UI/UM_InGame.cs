using UnityEditor;
using UnityEngine;

public class UM_InGame : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseMenu;

    private PlayerManager playerManager;

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

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        Debug.Log(playerManager);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
}
