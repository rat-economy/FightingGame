using UnityEngine;
using System.Collections.Generic;



[CreateAssetMenu(menuName = ("Character Attribute"))]
public class CharacterAttribute : ScriptableObject
{
    [Header("Player Stats")]
    public float moveSpeed = 1.0f;
    public float jumpSpeed = 7.0f;
    public float maxHealth = 100f;

    [Header("Light Attack")]
    public float lightDamage = 10f;
    public float lightWindup = 0f;
    public float lightCooldown = 0f;

    [Header("Heavy Attack")]
    public float heavyDamage = 10f;
    public float heavyWindup = 0f;
    public float heavyCooldown = 0f;

    [Header("Special Attack")]
    public float specialDamage = 10f;
    public float specialWindup = 0f;
    public float specialCooldown = 0f;

    [Header("Sound Effects")]
    public Sound s_jump;
    public Sound s_crouch;
    public Sound s_moving;
    public Sound s_light;
    public Sound s_heavy;
    public Sound s_special;
    public Sound s_block;
    public Sound s_hurt;
    public List<Sound> s_voicelines;
}