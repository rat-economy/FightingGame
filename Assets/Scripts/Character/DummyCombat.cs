using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DummyCombat : CharacterCombat
{
    public override void Light(InputAction.CallbackContext context)
    {
        
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

    public override void Unblock(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public override void Combo(InputType inputType)
    {
        throw new System.NotImplementedException();
    }
}
