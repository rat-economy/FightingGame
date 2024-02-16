using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuObj;
    [SerializeField] private GameObject fighterSelectObj;

    public void PlayButton()
    {
        mainMenuObj.SetActive(false);
        fighterSelectObj.SetActive(true);
    }
}
