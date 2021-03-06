﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

#pragma warning disable CS0108
public class GrenadeController : MonoBehaviour
{
    public Camera camera;
    public Animator animator;
    public GameObject thrownGrenade;
    public GameObject kinematicHandle;
    public GameObject thrownGrenadeHandle;
    private GameObject thrownGrenadeGO;
    public string grenadeType;
    public bool isImpact;
    public float fuzeTime;
    public float flashTime;
    public float highBlindTime;
    public float loBlindTime;
    public float grenadeEquipTime;
    public float throwForce;
    public float throwAnimationTime;
    public LayerMask playerLayer;
    public float loCheckRad;
    public float highCheckRad;
    public GameObject heldGrenadeMesh;
    public GameObject flashOverlay;

    private bool hasGrenade;
    private bool isExploding;
    private bool isGrenadeHeld;

    public float destroyTime;

    private bool check; //delete this later

    void Start()
    {
        if (grenadeType == null)
        {
            Debug.Log("No grenade type selected, defaulting to frag...");
            grenadeType = "frag";
        }

    } 

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!isExploding && Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(HoldGrenade(true));
        } */

        /*if (Input.GetKeyDown(KeyCode.G) && check)
        {
            isGrenadeHeld = false;
        }*/
        /*
        if(isGrenadeHeld && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Flash());
        }*/
    }

    public IEnumerator HoldGrenade(bool equip)
    {
        if (equip)
        {
            Debug.Log("fired equip");
            animator.SetBool("isGrenadeHeld", true);
            yield return new WaitForSeconds(grenadeEquipTime);
            animator.SetBool("isGrenadeHeld", false);
            isGrenadeHeld = true;
            check = true;
        } if (!equip)
        {
            animator.SetBool("isGrenadeHeld", false);
            yield return new WaitForSeconds(grenadeEquipTime);
            isGrenadeHeld = false;
        }
        
    }

    public IEnumerator Flash()
    {
        //piece of shit code but this is the only way i got this to work
        isExploding = true;
        animator.SetBool("letGo", true);
        
        Quaternion rot = Quaternion.Euler(camera.transform.forward);
        Instantiate(thrownGrenade, transform.position, rot);
        Instantiate(thrownGrenadeHandle, transform.position, rot * Quaternion.Euler(-30, 0, 0));
        thrownGrenadeGO = GameObject.FindGameObjectWithTag("thrown_flash");
        Rigidbody thrownGrenadeRB = thrownGrenadeGO.GetComponent<Rigidbody>();
        Light thrownGrenadeL = thrownGrenadeGO.GetComponentInChildren<Light>();
        ParticleSystem thrownGrenadePS = thrownGrenadeGO.GetComponentInChildren<ParticleSystem>();
        thrownGrenadeRB.AddForce(camera.transform.forward * throwForce);

        yield return new WaitForSeconds(fuzeTime);

        bool lo = Physics.CheckSphere(thrownGrenadeGO.transform.position, loCheckRad, playerLayer);
        bool hi = Physics.CheckSphere(thrownGrenadeGO.transform.position, highCheckRad, playerLayer);
        thrownGrenadePS.Play();

        yield return new WaitForSeconds(flashTime);

        thrownGrenadeGO.tag = "spent_grenade";
        thrownGrenadePS.Stop();
        if (lo)
        {
            flashOverlay.SetActive(true);
            yield return new WaitForSeconds(loBlindTime);
            flashOverlay.SetActive(false);
        }
        if (hi)
        {
            flashOverlay.SetActive(true);
            yield return new WaitForSeconds(highBlindTime);
            flashOverlay.SetActive(false);
        }

        if (destroyTime != 0)
        {

        }

        isExploding = false;
        animator.SetBool("letGo", false);
    }

    IEnumerator Frag()
    {
        yield return new WaitForSeconds(fuzeTime);
    }
}
