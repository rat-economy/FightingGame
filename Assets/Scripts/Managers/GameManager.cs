using UnityEngine;

public enum GameState
{

}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState state;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}