using UnityEngine;

[CreateAssetMenu(menuName = ("Sound"))]
public class Sound : ScriptableObject
{
    public AudioClip m_Clip;
    [Range(0f, 1f)] public float m_Volume = 1f;
    public AudioSource m_Source;
}