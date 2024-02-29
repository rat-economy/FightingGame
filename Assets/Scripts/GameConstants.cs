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

public struct Constant
{
  public const int MAX_COMBO_SIZE = 3;
  public const float INPUT_BUFFER_LENGTH = 0.3f;
  public const float SEC_PER_FRAME = 0.08333333333f;
  public const float CONTROLLER_DEADZONE = 0.70f;
}

public enum MovementAxis { N, NE, E, SE, S, SW, W, NW }

public enum InputType {L, H}
public enum AttackType
{
  Melee,
  Projectile,
  MeleeDash
}

public enum AnimationName
{
  Light1,
  Light2,
  Heavy1,
  Heavy2,
  Combo1,
  Combo2
}

[System.Serializable]
public struct Character {
  public GameObject prefab;
  public CharacterName name;
  public Sprite charSplash;
  public Sound announcerLine;
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
  //In-Game UI Constants
  public static float SPLASH_COUNTDOWN = 4f;

  //In-Game Audio Constants
  public static float ANNOUNCER_DELAY = 1f;
}