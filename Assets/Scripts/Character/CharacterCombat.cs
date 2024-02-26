using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public abstract class CharacterCombat : MonoBehaviour
{
    //References
    protected AudioManager audioManager;
    protected Animator m_animator;

    [Header("Attack Variables")]
    [SerializeField] protected Transform m_attackPoint;
    [SerializeField, Range(0f, 2f)] protected float m_attackRadius;

    protected List<AttackType> m_attackBuffer = new(new AttackType[Combat.MAX_COMBO_SIZE]);
    protected Coroutine m_inputListener;
    protected Coroutine m_attackCoroutine;

    protected CharacterAttribute M;
    protected CharacterController Status;

    protected virtual void Awake()
    {
        m_attackBuffer.Clear();
        audioManager = AudioManager.Instance;
        m_animator = GetComponent<Animator>();
        Status = GetComponent<CharacterController>();
        M = Status.Attributes;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(m_attackPoint.position, m_attackRadius);
    }

    public abstract void Light(InputAction.CallbackContext context);

    public IEnumerator C_Attack(float damage, float windup, float cooldown, AttackType type)
    {
        Status.DisableInput();
        //Add attack type to input buffer
        m_attackBuffer.Add(type);

        //Windup delay
        yield return new WaitForSeconds(windup);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRadius, Status.EnemyLayer);
        bool isHit = false;
        foreach (Collider2D enemy in hitEnemies)
        {
            isHit = enemy.transform.GetComponent<CharacterController>().RecieveDamage(damage);
        }

        //If hit enemy, & haven't hit combo finisher
        //Then listen for combos
        if(isHit && m_attackBuffer.Count != Combat.MAX_COMBO_SIZE)
        {
            Status.EnableInput();
            yield return new WaitForSeconds(Combat.INPUT_BUFFER_LENGTH);
        }
        else //If you missed enemy or you hit combo finisher
        //Then reset attack buffer and play cooldown
        {
            //Cooldown delay
            yield return new WaitForSeconds(cooldown);
            Status.EnableInput();
        }

        string combo = "";
        foreach(AttackType t in m_attackBuffer)
        {
            combo += type.ToString();
        }
        Debug.Log(combo);

        //Reset input buffer
        m_attackBuffer.Clear();
        m_attackCoroutine = null;

        yield return null;
    }

    public void StopAttack()
    {
        if (m_attackCoroutine == null)
        {
            return;
        }
        StopCoroutine(m_attackCoroutine);
        m_attackCoroutine = null;
    }

    private IEnumerator C_InputListener()
    {
        //Check if this is the last part of the combo
        if (m_attackBuffer.Count != Combat.MAX_COMBO_SIZE ) 
        {
            yield return new WaitForSeconds(Combat.INPUT_BUFFER_LENGTH);
            StopAttack();
        }
        yield return null;
    }

    public abstract void Heavy(InputAction.CallbackContext context);

    public abstract void Special(InputAction.CallbackContext context);

    public abstract void Block(InputAction.CallbackContext context);
}
