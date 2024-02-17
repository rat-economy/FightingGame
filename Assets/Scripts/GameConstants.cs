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

public enum MovementAxis { N, NE, E, SE, S, SW, W, NW, s }

[System.Serializable]
public struct Character {
  public GameObject prefab;
  public CharacterName name;
}
