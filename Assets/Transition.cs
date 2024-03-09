using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public Animator m_transition;
    public IEnumerator LoadGame()
    {
        m_transition.SetTrigger("START");
        yield return new WaitForSeconds(Constants.TRANSITION_TIME);
        SceneManager.LoadScene("MainScene");
        yield return new WaitForSeconds(0.1f);
    }
}
