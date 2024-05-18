using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;


public class CharacterInput : NetworkBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool Jump { get; private set; }
    public bool Interact { get; private set; }

    public bool ShowLeaderboard { get; private set; }

    public override void OnStartClient()
    {
        if (!hasAuthority) enabled = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnStopClient()
    {
        if (!hasAuthority) enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");
        Jump = Input.GetButtonDown("Jump");
        Interact = Input.GetButtonDown("Interact");
        ShowLeaderboard = Input.GetButton("Leaderboard");
    }
}
