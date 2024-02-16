using UnityEngine;

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
}
