using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Player Attributes"))]
public class PlayerAttributes : ScriptableObject
{
    [Header("Player Stats")]
    public float moveSpeed = 1.0f;
    public float jumpSpeed = 7.0f;

    [Header("Sound Effects")]
    public Sound s_jump;
    public Sound s_crouch;
    public Sound s_moving;
    public Sound s_light;
    public Sound s_heavy;
    public Sound s_special;
    public Sound s_block;
    public List<Sound> s_voicelines;
}