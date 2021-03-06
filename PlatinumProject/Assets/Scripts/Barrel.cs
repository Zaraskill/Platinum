﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

// Code créé et géré par Siméon
public class Barrel : MonoBehaviour
{
    [Header("Players To Knock")]
    public List<PlayerEntity> playerIntoArea;
    public List<PickupableObject> objetIntoArea;

    [Header("Explosion")]
    public float knockPower = 0f;
    public bool isTouchingPlayer;
    public bool isExploding;
    public GameObject explosionEffect;

    [Header("Camera Shaker")]
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;

    [Header("Components")]
    private PlayerEntity playerCollisionned;
    private Animator animator;

    //Vibration
    [Header("Vibration Manette")]
    public int motorIndex;
    public float motorLevel;
    public float duration;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isTouchingPlayer)
        {
            Explosion();
        }
        else if(isExploding)
        {
            Destruction();
        }
    }

    #region Collisions/Trigger Fonctions

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            return;
        }
        else
        {
            if (collision.gameObject.CompareTag("Pickable"))
            {
                if (objetIntoArea.Contains(collision.gameObject.GetComponent<PickupableObject>()))
                {
                    objetIntoArea.Remove(collision.gameObject.GetComponent<PickupableObject>());
                }                
            }
            isTouchingPlayer = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!playerIntoArea.Contains(collision.gameObject.GetComponent<PlayerEntity>()) )
            {
                playerIntoArea.Add(collision.gameObject.GetComponent<PlayerEntity>());
            }            
        }
        else if (collision.gameObject.CompareTag("Pickable"))
        {
            if (!objetIntoArea.Contains(collision.gameObject.GetComponent<PickupableObject>()) )
            {
                objetIntoArea.Add(collision.gameObject.GetComponent<PickupableObject>());
                collision.gameObject.GetComponent<PickupableObject>().GoInsideRangeBarrel(this);
            }            
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerIntoArea.Contains(collision.gameObject.GetComponent<PlayerEntity>()) )
            {
                playerIntoArea.Remove(collision.gameObject.GetComponent<PlayerEntity>());
            }            
        }
        else if (collision.gameObject.CompareTag("Pickable"))
        {
            objetIntoArea.Remove(collision.gameObject.GetComponent<PickupableObject>());
            collision.gameObject.GetComponent<PickupableObject>().ExitInsideRangeBarrel();
        }
    }
    #endregion

    #region Destruction Fonctions

    private void Explosion()
    {
        animator.SetBool("isExploding", true);
    }

    private void Destruction()
    {
        if (playerIntoArea.Count >= 1)
        {
            for (int i = playerIntoArea.Count; 0 <= --i;)
            {
                if(playerIntoArea[i])
                {
                Vector3 orientDir = (playerIntoArea[i].transform.position - transform.position);
                Vector3 directionNormalized = orientDir.normalized;
                playerIntoArea[i].Knockback(new Vector2(directionNormalized.x, directionNormalized.z), knockPower);
                playerIntoArea[i].controller.mainPlayer.SetVibration(motorIndex, motorLevel, duration);
                }
            }            
        }

        if (objetIntoArea.Count >= 1)
        {
            for (int i = objetIntoArea.Count; 0 <= --i;)
            {
                if(objetIntoArea[i])
                {
                Vector3 orientDir = (objetIntoArea[i].transform.position - transform.position);
                Vector3 directionNormalized = orientDir.normalized;
                objetIntoArea[i].Throw(new Vector2(directionNormalized.x, directionNormalized.z));
                }
            }            
        }

        SoundManager.managerSound.MakeBarrelExplosionSound();
        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        GameObject _instance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(_instance, 2f);
        GameManager.managerGame.SpawnBarrel();
        Destroy(gameObject);
    }

    #endregion

    public void objectLeaveRange(PickupableObject item)
    {
        if (objetIntoArea.Contains(item))
        {
            objetIntoArea.Remove(item);
        }
    }

}
