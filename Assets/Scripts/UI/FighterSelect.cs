using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FighterSelect : MonoBehaviour
{
    public static FighterSelect Instance;

    [SerializeField] private TextMeshProUGUI selectionText;
    
    [SerializeField] private GameObject currSelectionFrame;
    [SerializeField] private GameObject p1SelectionFrame;
    [SerializeField] private GameObject p2SelectionFrame;

    [SerializeField] private GameObject selectionButton;

    [SerializeField] private Image p1SelectionImage;
    [SerializeField] private GameObject[] p1Namecards;
    [SerializeField] private Image p2SelectionImage;
    [SerializeField] private GameObject[] p2Namecards;
    


    bool currPlayer = false;
    int currentlySelectedFighter;

    //Vector2[] frameLocationPerCharacter = new Vector2[] {new Vector2(-450, 225), new Vector2(-150, 225), new Vector2(150, 225), new Vector2(450, 225), new Vector2(-450, -150), new Vector2(-150, -150), new Vector2(150, -150), new Vector2(450, -150)};

    bool[] charSplashFlipped = {true, false};
    void Start()
    {
        Instance = this;
        ResetText();
    }
    public void ClickFighter(int nameIndex)
    {
        currentlySelectedFighter = nameIndex;

        EnableSelectionObjs(true);

        

        Image selectionPortrait;
        
        if (currPlayer == false)
        {
           selectionPortrait = p1SelectionImage; 
           foreach (GameObject namecard in p1Namecards)
            {
                namecard.SetActive(false);
            }
           p1Namecards[nameIndex].SetActive(true);
        }
        else
        {
            selectionPortrait = p2SelectionImage;
            foreach (GameObject namecard in p2Namecards)
            {
                namecard.SetActive(false);
            }
           p2Namecards[nameIndex].SetActive(true);
        }

        if (selectionPortrait.gameObject.activeSelf == false)
        {
            selectionPortrait.gameObject.SetActive(true);
        }

        selectionPortrait.sprite = FindCharacter((CharacterName)nameIndex).charSplashCrop;

        if (charSplashFlipped[nameIndex])
        {
            selectionPortrait.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            selectionPortrait.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        }

        //currSelectionFrame.GetComponent<RectTransform>().anchoredPosition = frameLocationPerCharacter[nameIndex];
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

            //p1SelectionFrame.SetActive(false);
            //p2SelectionFrame.SetActive(true);
        }
        else
        {
            //p2SelectionFrame.SetActive(false);
            //p1SelectionFrame.SetActive(true);
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
        //currSelectionFrame.SetActive(status);
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
