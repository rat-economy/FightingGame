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

public struct Combat
{
  public const int MAX_COMBO_SIZE = 3;
  public const float INPUT_BUFFER_LENGTH = 0.3f;
}

public enum MovementAxis { N, NE, E, SE, S, SW, W, NW }

public enum AttackType {L, H}

[System.Serializable]
public struct Character {
  public GameObject prefab;
  public CharacterName name;
}
