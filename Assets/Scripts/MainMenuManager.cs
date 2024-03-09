using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [SerializeField] private GameObject mainMenuObj;
    [SerializeField] private GameObject fighterSelectObj;
    [SerializeField] private GameObject levelSelectObj;
    [SerializeField] private GameObject optionsObj;
    [SerializeField] private GameObject creditsObj;

    [SerializeField] private GameObject redSpray;
    [SerializeField] private GameObject blueSpray;

    void Start()
    {
        Instance = this;
        AudioManager.Instance.PlaySoundLooped(AudioManager.Instance.menuSongs[0]);
    }

    public void ShowSpray(bool showRed)
    {
        if (showRed)
        {
            redSpray.SetActive(true);
        }
        else
        {
            blueSpray.SetActive(true);
        }
    }

    public void HideSpray()
    {
        redSpray.SetActive(false);
        blueSpray.SetActive(false);
    }

    public void EnterFighterSelect(bool twoPlayer)
    {
        GameManager.isTwoPlayer = twoPlayer;

        mainMenuObj.SetActive(false);
        fighterSelectObj.SetActive(true);

        if (FighterSelect.Instance != null)
        {
            FighterSelect.Instance.ResetText();
        }
    }

    public void ExitFighterSelect()
    {
        fighterSelectObj.SetActive(false);
        mainMenuObj.SetActive(true);
    }

    public void MoveToLevelSelect()
    {
        fighterSelectObj.SetActive(false);
        levelSelectObj.SetActive(true);
    }
    public void ExitLevelSelect()
    {
        levelSelectObj.SetActive(false);
        fighterSelectObj.SetActive(true);
    }

    public void EnterOptions()
    {
        mainMenuObj.SetActive(false);
        optionsObj.SetActive(true);
    }
    public void ExitOptions()
    {
        optionsObj.SetActive(false);
        mainMenuObj.SetActive(true);
    }

    public void EnterCredits()
    {
        AudioManager.Instance.StopSound(AudioManager.Instance.menuSongs[0]);
        mainMenuObj.SetActive(false);
        creditsObj.SetActive(true);
        AudioManager.Instance.PlaySoundLooped(AudioManager.Instance.menuSongs[1]);
    }

    public void ExitCredits()
    {
        AudioManager.Instance.StopSound(AudioManager.Instance.menuSongs[1]);
        creditsObj.SetActive(false);
        mainMenuObj.SetActive(true);

        creditsObj.GetComponent<Credits>().ResetCredits();
        AudioManager.Instance.PlaySoundLooped(AudioManager.Instance.menuSongs[0]);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit(); //This will not do anything in the inspector, but will quit out of a build
    }
}
