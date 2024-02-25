using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [Range(0, 0.3f)][SerializeField] private float m_MovementSmoothing = 0.05f; // насколько нужно сгладить движение
    [SerializeField] private bool m_AirControl = false; // может ли игрок управляться в воздухе или нет
    [SerializeField] private GrapplingRope m_GrapplingRope;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private Animator _anim;

    [Header("Jump")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float m_JumpForce = 8f; // сила прыжка
    [SerializeField] private bool isGrounded;
    private bool canJump;

    [Header("Dust particles")]
    [SerializeField] private ParticleSystem footParticles;

    // параметры времени, в течении которого прыжок может прыгать после того, как сошел с платформы
    private float hangCounter;
    private float hangTime = 0.1f;

    // параметры времени, которое фиксирует нажатие на кнопку прыжка, для того чтобы прыгнуть сразу после преземления 
    private float jumpBufferLenght = 0.1f;
    private float jumpBufferCount;


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        
    }
    private void Update()
    {
        CheckIfCanJump();
        Jump();    
    }

    private void FixedUpdate()
    {
        ChechSurroundings();
    }


    private void Jump()
    {
        if (canJump)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
        }

    }
    private void HangTimer()
    {
        if (isGrounded)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }
    }

    private void CheckIfCanJump()
    {
        HangTimer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCount = jumpBufferLenght;
            if (m_GrapplingRope.enabled)
            {
                m_GrapplingRope.GrapplingGun.NotGrapple();
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);
            }
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        if (hangCounter > 0f && m_Rigidbody2D.velocity.y <= 1.8f && jumpBufferCount > 0)
        {
            canJump = true;
            jumpBufferCount = 0;
        }
        else
        {
            canJump = false;
        }
    }

    private void ChechSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }



    public void Move(float move)
    {
        var footEmission = footParticles.emission;
        // управление персонажем только если он на земле или в воздухе, но включено управление в воздухе(airControl)
        if (isGrounded && !m_GrapplingRope.enabled || m_AirControl && !m_GrapplingRope.enabled)
        {
            // двигаем персонажа путем нахождения целевой скорости
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // затем сглаживаем и применяем к персонажу
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (isGrounded && Input.GetAxisRaw("Horizontal") != 0)
                footEmission.rateOverTime = 50f;
            else
                footEmission.rateOverTime = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
