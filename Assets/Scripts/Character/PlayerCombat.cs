using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    public override void Light(InputAction.CallbackContext context)
    {   
        if(m_comboStage >= 3)
        {
            return;
        }
        //Add input to input butter 
        m_attackBuffer.Add(InputType.L);

        //Don't do anything, there is predetermined logic for combo finisher
        if(m_comboStage == Constant.MAX_COMBO_SIZE)
        {
            Combo(InputType.L);
            return;
        }
        
        //Stop waiting for combo input
        StopAttack();

        switch(m_comboStage)
        {
            case 1:
                
                m_attackCoroutine = StartCoroutine(C_Attack(M.Light1));
                break;
            case 2:
                m_animator.SetTrigger(M.Light2.AnimationName.ToString());
                m_attackCoroutine = StartCoroutine(C_Attack(M.Light2));
                break;
        }
    }

    public override void Heavy(InputAction.CallbackContext context)
    {
        Debug.Log("Heavy Attack Performed.");
    }

    public override void Special(InputAction.CallbackContext context)
    {
        Debug.Log("Special Attack Performed.");
    }

    public override void Combo(InputType inputType)
    {
        if(m_comboStage != Constant.MAX_COMBO_SIZE)
        {
            return;
        }
        //Compares if the input buffer matches any combos
        foreach (Attack a in M.Combos)
        {
            if (a.ComboInput.SequenceEqual(m_attackBuffer))
            {
                m_animator.SetTrigger(a.AnimationName.ToString());
                m_attackCoroutine = StartCoroutine(C_Attack(a));
            }
        }

    }

    public override void Block(InputAction.CallbackContext context)
    {
        audioManager.PlaySoundOnce(M.S_Block);
        Debug.Log("Block Attack Performed.");
    }
}
