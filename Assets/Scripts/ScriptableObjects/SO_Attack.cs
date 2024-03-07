using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Attack"))]
public class Attack : ScriptableObject
{
    public float Damage;
    [Range(1, 20)] public int WindupFrames;
    [Range(1, 20)] public int CooldownFrames;

    public float WindupTime { get { return WindupFrames * Constant.SEC_PER_FRAME; }}
    public float CooldownTime { get { return CooldownFrames * Constant.SEC_PER_FRAME; }}

    public AttackType AttackType;
    public InputType InputType;
    public AnimationName AnimationName;
    public bool isComboFinisher;
    public List<InputType> ComboInput;
    public Sound Sound;
}