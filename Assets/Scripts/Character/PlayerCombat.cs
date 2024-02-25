using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    public override void Light(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(M.S_Light);
        m_animator.SetTrigger("LightAttack");

        StopAttack();
        m_attackCoroutine = StartCoroutine(
            C_Attack(M.LightDamage, M.LightWindup, M.LightCooldown, AttackType.L)
        );
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
