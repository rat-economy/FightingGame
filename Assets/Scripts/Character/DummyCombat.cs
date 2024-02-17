using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DummyCombat : CharacterCombat
{
    public override void Light(InputAction.CallbackContext context)
    {
        
    }

    public override IEnumerator C_Attack(float damage, float windup, float cooldown)
    {
        yield return null;
    }

    public override void Heavy(InputAction.CallbackContext context)
    {

    }

    public override void Special(InputAction.CallbackContext context)
    {

    }

    public override void Block(InputAction.CallbackContext context)
    {

    }
}
