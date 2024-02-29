using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Character Attribute"))]
public class CharacterAttribute : ScriptableObject
{
    [Header("Player Stats")]
    public float MoveSpeed = 3.0f;
    public float JumpSpeed = 7.0f;
    public float MaxHealth = 100f;

    [Header("Attacks")]
    public Attack Light1;
    public Attack Light2;
    public Attack Heavy1;
    public Attack Heavy2;
    public Attack[] Combos;
    

    [Header("Sound Effects")]
    public Sound S_Jump;
    public Sound S_Crouch;
    public Sound S_Moving;
    public Sound S_Block;
    public Sound S_Hurt;
    public Sound S_Land;
    public List<Sound> S_Voicelines;
}