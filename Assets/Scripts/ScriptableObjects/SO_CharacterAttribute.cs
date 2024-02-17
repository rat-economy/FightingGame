using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Character Attribute"))]
public class CharacterAttribute : ScriptableObject
{
    [Header("Player Stats")]
    public float MoveSpeed = 3.0f;
    public float JumpSpeed = 7.0f;
    public float MaxHealth = 100f;

    [Header("Light Attack")]
    public float LightDamage = 10f;
    public float LightWindup = 0f;
    public float LightCooldown = 0f;

    [Header("Heavy Attack")]
    public float HeavyDamage = 10f;
    public float HeavyWindup = 0f;
    public float HeavyCooldown = 0f;

    [Header("Special Attack")]
    public float SpecialDamage = 10f;
    public float SpecialWindup = 0f;
    public float SpecialCooldown = 0f;

    [Header("Sound Effects")]
    public Sound S_Jump;
    public Sound S_Crouch;
    public Sound S_Moving;
    public Sound S_Light;
    public Sound S_Heavy;
    public Sound S_Special;
    public Sound S_Block;
    public Sound S_Hurt;
    public Sound S_Land;
    public List<Sound> S_Voicelines;
}