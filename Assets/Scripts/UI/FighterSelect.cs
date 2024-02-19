using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FighterSelect : MonoBehaviour
{
    public static FighterSelect Instance;

    [SerializeField] private TextMeshProUGUI selectionText;
    [SerializeField] private GameObject selectionFrame;
    [SerializeField] private GameObject selectionButton;


    bool currPlayer = false;
    int currentlySelectedFighter;

    Vector2[] frameLocationPerCharacter = new Vector2[] {new Vector2(-375, 125), new Vector2(-125, 125), new Vector2(125, 125), new Vector2(375, 125), new Vector2(-375, -125), new Vector2(-125, -125), new Vector2(125, -125), new Vector2(375, -125)};

    void Start()
    {
        Instance = this;
        ResetText();
    }
    public void ClickFighter(int nameIndex)
    {
        currentlySelectedFighter = nameIndex;

        EnableSelectionObjs(true);

        selectionFrame.GetComponent<RectTransform>().anchoredPosition = frameLocationPerCharacter[nameIndex];
    }

    public void FighterSelectButton()
    {
        CharacterName name = (CharacterName)currentlySelectedFighter;
        SelectCharacter(name, currPlayer);

        if (currPlayer == false)
        {
            currPlayer = true;

            if (GameManager.isTwoPlayer == false)
            {
                selectionText.text = "Choose Your Opponent!";
            }
            else
            {
                selectionText.text = "Player Two\nChoose Your Fighter!";
            }
        }
        else
        {
            MainMenuManager.Instance.MoveToLevelSelect();
        }
    }
    //Move to playerselect script
    private void SelectCharacter(CharacterName name, bool player)
    {
        EnableSelectionObjs(false);

        Character c = FindCharacter(name);
        if (player == false)
        {
            //Debug.Log("Assigning Player 1: " + name);
            GameManager.p1_selectedCharacter = c;
        }
        else
        {
            //Debug.Log("Assigning Player 2: " + name);
            GameManager.p2_selectedCharacter = c;
        }
    }

    //Move to player select script
    private Character FindCharacter(CharacterName name)
    {
        foreach (Character c in GameManager.Instance.m_characters)
        {
            if (c.name == name)
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

    public void ResetFighterSelect(bool exitMenu)
    {
        currPlayer = false;
        currentlySelectedFighter = 0;
        EnableSelectionObjs(false);

        if (exitMenu)
        {
            MainMenuManager.Instance.ExitFighterSelect();
        }
    }

    private void EnableSelectionObjs(bool status)
    {
        selectionFrame.SetActive(status);
        selectionButton.SetActive(status);
    }

    public void ResetText()
    {
        if (GameManager.isTwoPlayer)
        {
            selectionText.text = "Player One\nChoose Your Fighter!";
        }
        else
        {
            selectionText.text = "Choose Your Fighter!";
        }
    }
}
