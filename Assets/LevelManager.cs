using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bgImage;

    public void SetupBackground()
    {
        bgImage.sprite = GameManager.selectedLevel.levelArt;
    }
}
