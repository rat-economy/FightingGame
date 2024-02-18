using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [SerializeField] private GameObject mainMenuObj;
    [SerializeField] private GameObject fighterSelectObj;
    [SerializeField] private GameObject levelSelectObj;

    void Start()
    {
        Instance = this;
    }

    public void PlayButton()
    {
        mainMenuObj.SetActive(false);
        fighterSelectObj.SetActive(true);
    }

    public void MoveToLevelSelect()
    {
        fighterSelectObj.SetActive(false);
        levelSelectObj.SetActive(true);
    }
}
