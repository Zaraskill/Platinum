﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


// Code crée et géré par Corentin
public class PlayerController : MonoBehaviour
{

    public string playerKey;

    public PlayerEntity entity;

    public Player mainPlayer;

    // Start is called before the first frame update
    void Start()
    {
        mainPlayer = ReInput.players.GetPlayer(playerKey);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.managerGame.IsWaitingForInput())
        {
            return;
        }
        if (!entity.CanMove())
        {
            return;
        }
        float dirX = 0f;
        float dirY = 0f;

        dirX = mainPlayer.GetAxis("HorizontalMove");
        dirY = mainPlayer.GetAxis("VerticalMove");

        Vector2 moveDir = new Vector2(dirX, dirY);
        moveDir.Normalize();

        entity.Move(moveDir);

        entity.GetComponent<Animator>().SetFloat("Move", moveDir.magnitude);

        if (mainPlayer.GetButtonDown("PickUp") && entity.IsInsideCanon())
        {
            entity.QuitCanon();
        }
        else if (mainPlayer.GetButtonDown("PickUp") && entity.CanPick() && !entity.IsHoldingItem())
        {
            Debug.Log("Pickup");
            entity.PickItem();
        }
        else if (mainPlayer.GetButtonUp("PickUp") && entity.CanThrow())
        {
            Debug.Log("Throw");
            entity.Throw();
        }

        
    }
}
