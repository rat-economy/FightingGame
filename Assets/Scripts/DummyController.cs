using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DummyController : MonoBehaviour
{
    [SerializeField] private float m_maxHealth = 200f;
    private float m_currentHealth;

    private Animator m_animator;

    private AudioManager audioManager;
    [SerializeField] private Sound s_hurt;

    public void RecieveDamage(float damage)
    {
        // if (m_PlayerController.isDashing) return;
        m_currentHealth -= damage;

        //Cancel what the player is doing when they recieve damage
        StopAllCoroutines();
        if (m_currentHealth <= 0)
        {
            m_animator.SetTrigger("Death");
            //INGNORE COLLISIONS EXCEPT FOR GROUND
        }
        else
        {
            m_animator.SetTrigger("Hurt"); 
            audioManager.PlaySoundOnce(s_hurt);
           
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        audioManager = AudioManager.Instance;

        m_animator = GetComponent<Animator>();

        m_currentHealth = m_maxHealth;
    }
}