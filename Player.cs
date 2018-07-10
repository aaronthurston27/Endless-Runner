using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region main variables

    private CharacterController controller;
    private Animator animator;

    public float runningSpeed;
    public float strafeSpeed;

    public float gravity;
    public float jumpHeight;
    private Vector3 movement;

    [NonSerialized]
    public float score = 0;

    private float jump;

    #endregion

    #region monobehavior methods

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (!controller)
            Debug.LogError("Player character controller not found");
        movement.z = runningSpeed;
    }

    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal") * strafeSpeed, 0, runningSpeed);

        if (controller.isGrounded)
        {
            jump = 0;
            if (Input.GetAxis("Jump") > 0)
                jump = jumpHeight;
        }

        float distance = this.transform.position.z;

        movement.y = jump;
        controller.Move(movement * Time.fixedDeltaTime);
        jump -= gravity * Time.deltaTime;

        distance = this.transform.position.z - distance;

        if (Overseer.Instance.gameStarted)
        {
            score += distance;
            Overseer.Instance.distanceTilCheckpoint -= distance;
        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Platform>())
        {
            if(hit.gameObject != PlatformSpawner.Instance.currentPlayerPlatform.gameObject)
            {
                PlatformSpawner.Instance.platforms.Remove(PlatformSpawner.Instance.currentPlayerPlatform);
                Destroy(PlatformSpawner.Instance.currentPlayerPlatform.gameObject);
                PlatformSpawner.Instance.currentPlayerPlatform = hit.gameObject.GetComponent<Platform>();
            }
        }
        else if(hit.gameObject.tag == "Boundaries")
        {
            Overseer.Instance.GameOver();
            Destroy(this.gameObject);
        }
        else if(hit.gameObject.tag == "Obstruction")
        {
            Vector3 dir = (hit.transform.position - this.transform.position);
            if(Vector3.Angle(dir.normalized,this.transform.forward) <= 90f && (dir.magnitude <= 10f) && Overseer.Instance.gameStarted)
            {
                Overseer.Instance.GameOver();
                Destroy(this.gameObject);
            }
        }
    }

    #endregion

    #region methods

    #endregion
}
