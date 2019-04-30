using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController2D controller;
    public Animator animator;
    public float moveSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    bool ignoreFirst = false;

    void Update () {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsJumping", true);
            jump = true;
            ignoreFirst = true;
        }

        if (Input.GetButtonDown("Throw"))
        {
            GameManager.instance.ThrowBerry();
        }
            
    }

    void FixedUpdate () {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    public void OnLanding ()
    {
        if (ignoreFirst)
            ignoreFirst = false;
        else
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Thorns")
        {
            Die();
        }
    }

    void Die ()
    {
        animator.SetBool("IsAlive", false);
        GameManager.instance.Invoke("StartGame", 1f / 3f);
    }
}