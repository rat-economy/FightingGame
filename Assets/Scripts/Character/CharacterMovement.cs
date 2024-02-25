using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterMovement : MonoBehaviour
{

    protected Rigidbody2D m_rigidbody;
    protected Animator m_animator;
    protected Vector2 m_moveDirection;

    protected AudioManager audioManager;

    public abstract void Jump();
    public abstract void Land();
    public abstract void Crouch();
    public abstract void Stand();
    public abstract void Move();
    public abstract void StopMove();

    protected virtual void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }   
}