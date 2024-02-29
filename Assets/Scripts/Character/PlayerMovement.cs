using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    private PlayerController Status;
    private CharacterAttribute M;

    protected override void Awake()
    {
        base.Awake();
        Status = GetComponent<PlayerController>();
        M = Status.Attributes;
    }

    public override void Jump() //W - Keyboard, Analog Stick Up - Controller 
    {
        m_animator.SetTrigger("Jump");
        audioManager.PlaySoundOnce(M.S_Jump);
        m_rigidbody.velocity += new Vector2(0, M.JumpSpeed);
    }

    public override void Land()
    {
        audioManager.PlaySoundOnce(M.S_Land);
    }

    public override void Crouch() //S - Keyboard, Analog Stick Down - Controller 
    {
        audioManager.PlaySoundOnce(M.S_Crouch);
        transform.localScale = new Vector3(1f, 0.5f, 1f);
        transform.localPosition -= new Vector3(0f, 0.1f, 0f);
    }

    public override void Stand()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.localPosition += new Vector3(0f, 0.1f, 0f);
    }

    //TODO: FIND A WAY TO MOVE THIS INTO Move()
    private void Update()
    {
        m_rigidbody.velocity = new Vector2(Status.MovementVect.x * M.MoveSpeed, m_rigidbody.velocity.y);
    }

    public override void Move()
    {
        m_animator.SetBool("isMoving", Status.IsMoving);
        audioManager.PlaySoundLooped(M.S_Moving);

        //Flips the player if they turn
        if (Status.MovementVect.x > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (Status.MovementVect.x < 0)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public override void StopMove()
    {
        m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
        m_animator.SetBool("isMoving", Status.IsMoving);
        audioManager.StopSound(M.S_Moving);
    }
}
