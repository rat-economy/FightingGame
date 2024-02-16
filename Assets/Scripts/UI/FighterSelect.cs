using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSelect : MonoBehaviour
{
    bool currPlayer = false;
    public void FighterSelectButton(int nameIndex)
    {
        CharacterName name = (CharacterName)nameIndex;
        SelectCharacter(name, currPlayer);

        GameManager.Instance.InitializeRound();
    }
    //Move to playerselect script
    private void SelectCharacter(CharacterName name, bool player)
    {
        Character c = FindCharacter(name);
        if (player == false)
        {
            GameManager.p1_selectedCharacter = c;
        }
        else
        {
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
}
