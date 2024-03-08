using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    private PlayerController Status;
    private CharacterAttribute M;

    protected override void Awake()
    {
        //Debug.Log("DING!");
        base.Awake();
        Status = GetComponent<PlayerController>();
        M = Status.Attributes;    
    }

    private void Start()
    {
        Stand();
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
        if (Status.IsJumping || Status.IsMoving || Status.IsBlocking) return;
        Status.IsCrouching = true;
        m_animator.SetBool("isCrouching", Status.IsCrouching);
        m_standCollider.enabled = false;
        m_crouchCollider.enabled = true;
        audioManager.PlaySoundOnce(M.S_Crouch);
    }

    public override void Stand()
    {
        Status.IsCrouching = false;
        m_animator.SetBool("isCrouching", Status.IsCrouching);
        m_standCollider.enabled = true;
        m_crouchCollider.enabled = false;
    }

    //TODO: FIND A WAY TO MOVE THIS INTO Move()
    private void Update()
    {
        if( Status.IsMoving )   m_rigidbody.velocity = new Vector2(Status.MovementVect.x * M.MoveSpeed, m_rigidbody.velocity.y);
    }

    public override void Move()
    {
        if (Status.MovementVect.x == 0 || Status.IsBlocking || Status.IsCrouching) return;
        Status.IsMoving = true;
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
        Status.IsMoving = false;
        m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
        m_animator.SetBool("isMoving", Status.IsMoving);
        audioManager.StopSound(M.S_Moving);
    }
}
