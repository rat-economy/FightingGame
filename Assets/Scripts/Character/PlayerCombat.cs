using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    private CharacterAttribute M;
    private PlayerController Status;

    protected override void Awake()
    {
        base.Awake();
        Status = GetComponent<PlayerController>();
        M = Status.Attributes;
    }

    public override void Light(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(M.S_Light);
        m_animator.SetTrigger("LightAttack");

        StartCoroutine(C_Attack(M.LightDamage, M.LightWindup, M.LightCooldown));
    }

    public override IEnumerator C_Attack(float damage, float windup, float cooldown)
    {
        Status.DisableInput();
        //Windup delay
        yield return new WaitForSeconds(windup);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRadius, Status.EnemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.transform.GetComponent<CharacterController>().RecieveDamage(damage);
        }

        //Cooldown delay
        yield return new WaitForSeconds(cooldown);
        Status.EnableInput();

        yield return null;
    }

    public override void Heavy(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(M.S_Heavy);
        Debug.Log("Heavy Attack Performed.");
    }

    public override void Special(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(M.S_Special);
        Debug.Log("Special Attack Performed.");
    }

    public override void Block(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(M.S_Block);
        Debug.Log("Block Attack Performed.");
    }
}
