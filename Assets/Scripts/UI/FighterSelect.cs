using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSelect : MonoBehaviour
{
    [SerializeField] private GameObject selectionFrame;
    [SerializeField] private GameObject selectionButton;


    bool isTwoPlayer = false; //To test 2 player: change to true
    bool currPlayer = false;
    int currentlySelectedFighter;

    Vector2[] frameLocationPerCharacter = new Vector2[] {new Vector2(-375, 125), new Vector2(-125, 125), new Vector2(125, 125), new Vector2(375, 125), new Vector2(-375, -125), new Vector2(-125, -125), new Vector2(125, -125), new Vector2(375, -125)};

    public void ClickFighter(int nameIndex)
    {
        currentlySelectedFighter = nameIndex;

        selectionFrame.SetActive(true);
        selectionButton.SetActive(true);

        selectionFrame.GetComponent<RectTransform>().anchoredPosition = frameLocationPerCharacter[nameIndex];
    }

    public void FighterSelectButton()
    {
        CharacterName name = (CharacterName)currentlySelectedFighter;
        SelectCharacter(name, currPlayer);

        if (currPlayer == false)
        {
            currPlayer = true;
        }
        else
        {
            GameManager.isTwoPlayer = isTwoPlayer;
            MoveToLevelSelect();
            MainMenuManager.Instance.MoveToLevelSelect();
            //GameManager.Instance.StartInitializeRound(isTwoPlayer); //Passes in false for now, so always single player. Can change later.
        }
    }
    //Move to playerselect script
    private void SelectCharacter(CharacterName name, bool player)
    {
        selectionFrame.SetActive(false);
        selectionButton.SetActive(false);

        Character c = FindCharacter(name);
        if (player == false)
        {
            Debug.Log("Assigning Player 1: " + name);
            GameManager.p1_selectedCharacter = c;
        }
        else
        {
            Debug.Log("Assigning Player 2: " + name);
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

    private void MoveToLevelSelect()
    {

    }
}
