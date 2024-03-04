using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private RectTransform credits;
    [SerializeField] private float creditsSpeed;

    //These being the location the credits start and end at
    [SerializeField] private float startPos = -1000;
    [SerializeField] private float endPos = 5750;
    // Start is called before the first frame update

    private void Update()
    {
        credits.anchoredPosition = new Vector2(0, credits.anchoredPosition.y + creditsSpeed);

        if (credits.anchoredPosition.y >= endPos)
        {
            ResetCredits();
        }
    }

    public void ResetCredits()
    {
        credits.anchoredPosition = new Vector2(0, startPos);
    }
}