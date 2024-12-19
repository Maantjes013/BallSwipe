using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public PhysicMaterial bounceMaterial;
    public float forwardSpeedMultiplier;
    public float upwardsSpeedMultiplier;
    public float forceMultiplier;

    public bool isFlying;

    [HideInInspector]
    public Rigidbody rb;

    private Vector2 swipeStartPos;
    private Vector2 swipeEndPos;
    private bool isHoldingMouse;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHoldingMouse = true;
            swipeStartPos = Input.mousePosition;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && isHoldingMouse)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        swipeEndPos = Input.mousePosition;
        Vector2 swipeDelta = swipeEndPos - swipeStartPos;

        // Make sure player can't shoot backwards
        if (swipeDelta.y > 0)
            swipeDelta.y = 0;

        // Determine force and apply to ball
        Vector3 force = new Vector3(-swipeDelta.x * forwardSpeedMultiplier, -swipeDelta.y * upwardsSpeedMultiplier, 
            -swipeDelta.y * forwardSpeedMultiplier) * forceMultiplier;
        rb.AddForce(force, ForceMode.Impulse);

        isHoldingMouse = false;
        isFlying = true;

        GetComponent<SphereCollider>().material = bounceMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
            GameManager.Instance.AddPoints();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Backwall"))
            GameManager.Instance.ExplodeBall();
    }
}
