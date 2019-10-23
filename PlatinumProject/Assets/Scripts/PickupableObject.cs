﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code créé et géré par Siméon
public class PickupableObject : MonoBehaviour
{
    private Vector2 velocity = Vector2.zero;
    private bool isPickable = true;

    private Vector2 orient = Vector2.zero;
    private float powerKnock = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 movePosition = transform.position;
        movePosition.x += velocity.x * Time.fixedDeltaTime;
        movePosition.z += velocity.y * Time.fixedDeltaTime;
        transform.position = movePosition;
    }

    public void Throw(Vector2 orient, float power)
    {
        this.orient = orient;
        powerKnock = power;
        velocity = orient * power;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    public bool IsPickable()
    {
        return isPickable;
    }

    public void SetPickable(bool pick)
    {
        isPickable = pick;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isPickable)
        {
            collision.gameObject.GetComponent<PlayerEntity>().Knockback(orient, powerKnock);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}