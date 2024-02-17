using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterCombat : MonoBehaviour
{
    //References
    protected AudioManager audioManager;
    protected Animator m_animator;

    [Header("Attack Variables")]
    [SerializeField] protected Transform m_attackPoint;
    [SerializeField, Range(0f, 2f)] protected float m_attackRadius;

    protected virtual void Awake()
    {
        audioManager = AudioManager.Instance;
        m_animator = GetComponent<Animator>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(m_attackPoint.position, m_attackRadius);
    }

    public abstract void Light(InputAction.CallbackContext context);

    public abstract IEnumerator C_Attack(float damage, float windup, float cooldown);

    public abstract void Heavy(InputAction.CallbackContext context);

    public abstract void Special(InputAction.CallbackContext context);

    public abstract void Block(InputAction.CallbackContext context);
}
