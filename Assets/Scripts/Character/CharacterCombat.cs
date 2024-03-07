using System.Collections;
using System.Collections.Generic;
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

    protected List<InputType> m_attackBuffer = new(new InputType[Constant.MAX_COMBO_SIZE]);
    protected int m_comboStage { get { return m_attackBuffer.Count; } }
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

    public IEnumerator C_Attack(Attack attack)
    {
        Status.DisableInputInGame();

        m_animator.SetTrigger(attack.AnimationName.ToString());
        audioManager.PlaySoundOnce(attack.Sound);

        //Windup delay
        yield return new WaitForSeconds(attack.WindupTime);

        bool isHit = false;

        if(attack.AttackType == AttackType.Melee)
        {
            //Melee Attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRadius, Status.EnemyLayer);
            
            foreach (Collider2D enemy in hitEnemies)
            {
                isHit = enemy.transform.GetComponentInParent<CharacterController>().RecieveDamage(attack.Damage);
            }
        }
        
        //TODO: Add Projectile Attack
        if(attack.AttackType == AttackType.Projectile)
        {

        }

        if (attack.AttackType == AttackType.MeleeDash)
        {
            
        }

        //If hit enemy, & haven't hit combo finisher
        //Then listen for combos
        if(isHit && m_attackBuffer.Count < Constant.MAX_COMBO_SIZE)
        {
            Status.EnableInputInGame();
            yield return new WaitForSeconds(attack.CooldownTime);
        }
        else //If you missed enemy or you hit combo finisher
        //Then reset attack buffer and play cooldown
        {
            //Cooldown delay
            yield return new WaitForSeconds(attack.CooldownTime);
            Status.EnableInputInGame();
        }

        //Reset input buffer
        m_attackBuffer.Clear();
        m_attackCoroutine = null;

        yield return null;
    }

    private void PrintInputBuffer()
    {
        string combo = "";
        foreach(InputType t in m_attackBuffer)
        {
            combo += t.ToString();
        }
        Debug.Log(combo);
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

    public abstract void Heavy(InputAction.CallbackContext context);
    public abstract void Special(InputAction.CallbackContext context);
    public abstract void Block(InputAction.CallbackContext context);
    public abstract void Unblock(InputAction.CallbackContext context);
    public abstract void Combo(InputType inputType);
}