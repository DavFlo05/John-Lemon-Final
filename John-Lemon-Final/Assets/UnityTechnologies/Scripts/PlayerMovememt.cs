using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;               // Controls how fast John turns
    public float speedBoostMultiplier = 1.5f;   // Mod-Multiplier for running speed

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    float movementMultiplier = 1f;              // Mod-Used to boost movement while running

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        m_AudioSource.pitch = Mathf.Lerp(1f, 1.3f, (movementMultiplier - 1f)); // Mod-Makes footsteps sound faster when running

        bool isWalking = horizontal != 0f || vertical != 0f;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
        // Mod-If Left Shift is held, boost movement multiplier
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementMultiplier = speedBoostMultiplier;
        }
        else
        {
            movementMultiplier = 1f;
        }

        // Turn to face movement direction
        if (isWalking)
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
        }
    }

    void OnAnimatorMove()
    {
        // Mod-Apply boosted movement distance based on animation and multiplier
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * movementMultiplier);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}