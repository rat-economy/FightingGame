using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    SINGLEPLAYER,
    MULTIPLAYER
}

public enum CharacterName
{
    CJ,
    Jay,
    William,
    Roosevelt,
    Eric,
    Dez,
    Sunny,
    Albert
}

[System.Serializable]
public struct Character {
  public GameObject prefab;
  public CharacterName name;
  public Sprite charSplash;
}


public enum LevelName
{
  Level1,
  Level2,
  Level3
}

[System.Serializable]
public struct Level {
  public Sprite levelArt;
  public LevelName name;
}

public class Constants
{
  public static float COUNTDOWN = 4f;
}