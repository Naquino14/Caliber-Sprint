﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //haha yes
    #region character controller and general stuff

    public CharacterController controller;
    public float characterCrouch;
    public float characterStand;
    public float speed = 20;

    #endregion

    #region sliding

    Vector3 slideDirection;
    public float minSpeed = 4;
    public float slideSpeed = 40f; 
    public float slideDecayRate = 20f;
    private bool isSliding;
    private float slideTimer;
    public float maxSildeTime = 2.0f;
    public float slideCooldown;

    #endregion

    #region crouching

    private bool isCrouching;

    #endregion

    #region jumping

    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private int amtJump;

    #endregion

    #region ground checks

    public Transform groundCheck;
    public float checkRad;
    public LayerMask floorMask;
    private bool isGrounded;

    #endregion

    void Start() // Not used
    {
        
    }

    
    void Update() // Update is called once per frame
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRad, floorMask); // Ground Check

        if (isGrounded && velocity.y < -2f) // Reset jumps and velocity when the player hits the ground
        {
            velocity.y = 0f;
            amtJump = 0;
        }

        #region general movement

        float x = Input.GetAxis("Horizontal"); // these two lines gather input axis info
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; // make move position relative to player...

        if (!isSliding) // Check if the player is sliding... general movement goes in here...
        {
            controller.Move(move * speed * Time.deltaTime);
            controller.height = characterStand;
        }

        #endregion

        #region jumping

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.Space) && (amtJump < 2))
            // check if space is pressed, the player has only jumped once, and the player is on the ground before performing a jump (or another)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            amtJump++;
        }

        velocity.y += gravity * Time.deltaTime; // calculates velocity
        controller.Move(velocity * Time.deltaTime); // Time.deltaTime a second time counts as a square (deltaV = 1/2 gravity * Time^2)
                                                    //The jump is actually performed on this line btw ^

        #endregion

        #region Sliding

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSliding && isGrounded) // check to see if the player is grounded, not sliding, and if shift pressed...
        {
            isSliding = true;
            slideTimer = 0.0f;
            controller.height = characterCrouch;

        }

        if (isSliding) // is the player sliding?
        {
            slideTimer += Time.deltaTime; // ticks off a timer (fuck IENumerators, Time.deltaTime is where its at bby!)
            slideSpeed = slideSpeed - slideDecayRate * Time.deltaTime; // Subtract the speed of the slide by a set rate (slideDecayRate)
            Mathf.Clamp(slideSpeed, 0f, Mathf.Infinity); // make sure the player doesnt have negative speed...
            controller.Move(move * slideSpeed * Time.deltaTime); // this is what actually moves the player during the slide
        } if (slideTimer > maxSildeTime) //checks to see if the timer is up
        {
            isSliding = false;
            //isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) // cancel the slide
        {
            // Nate please add cooldown
            isSliding = false;
            controller.height = characterStand;
            slideSpeed = 60f;
        }

        #endregion

    }
}