using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject selectionFrame;
    [SerializeField] private GameObject selectionButton;
    int currentlySelectedLevel;

    Vector2[] frameLocationPerLevel = new Vector2[] {new Vector2(-600, 0), new Vector2(0, 0), new Vector2(600, 0)};

    public void ClickLevel(int levelIndex)
    {
        currentlySelectedLevel = levelIndex;

        selectionFrame.SetActive(true);
        selectionButton.SetActive(true);

        selectionFrame.GetComponent<RectTransform>().anchoredPosition = frameLocationPerLevel[levelIndex];
    }

    public void LevelSelectButton()
    {
        LevelName level = (LevelName)currentlySelectedLevel;
        SelectLevel(level);

        GameManager.Instance.StartInitializeRound(GameManager.isTwoPlayer);
    }
    //Move to playerselect script
    private void SelectLevel(LevelName level)
    {
        selectionFrame.SetActive(false);
        selectionButton.SetActive(false);

        Level l = FindLevel(level);

        GameManager.selectedLevel = l;
    }

    //Move to player select script
    private Level FindLevel(LevelName level)
    {
        foreach (Level l in GameManager.Instance.m_levels)
        {
            if (l.name == level)
            {
                return l;
            }
        }
        Debug.LogError("Cannot find level. Add the level to GameManager.");
        return new Level();
    }

    //Move to player slect script
    public void SelectRandomLevels()
    {

    }
}
