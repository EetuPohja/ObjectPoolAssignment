﻿using System.Collections;
using UnityEngine;

namespace CursedWoods
{
    public class DashState : PlayerActionStateBase
    {
        private float dashVel = 1000f;
        private bool isDashing = false;
        private float dashHoldTime = 0.25f;
        private PlayerInputType nextState = PlayerInputType.None;
        private Vector2 inputDir;
        private bool isDashInCoolDown = false;
        private float dashCoolDownTime = 1f;

        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.Dash;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.None);
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Attack);
            AddTargetState(PlayerInputType.Spellcast);
        }

        public override void DaUpdate()
        {
            if (!isDashing && !isDashInCoolDown)
            {
                isDashing = true;
                StartCoroutine(DashHoldTimer());
            }
        }

        public override void DaFixedUpdate()
        {
            if (isDashing && !isDashInCoolDown)
            {
                actionStateManager.PlayerRb.velocity = transform.forward * dashVel * Time.fixedDeltaTime;
            }
        }

        public override void HandleInput()
        {
            inputDir = new Vector2(Input.GetAxisRaw(CharController.HORIZONTAL), Input.GetAxisRaw(CharController.VERTICAL));
            // AWFUL SINCE IF WE ARE IN COOLDOWN THEN WE SHOULD NOT EVEN COME TO THIS STATE
            // JUST PUT BOOLEANS TO CHARCONTROLLER TO AVOID THIS KINDA STUFF
            if (isDashInCoolDown)
            {
                actionStateManager.ChangeState(nextState);
            }
            else if (Input.GetButtonDown(CharController.ATTACK))
            {
                nextState = PlayerInputType.Attack;
            }
            else if (Input.GetButtonDown(CharController.SPELLCAST))
            {
                nextState = PlayerInputType.Spellcast;
            }
            else if (Input.GetButtonDown(CharController.INTERACT))
            {
                nextState = PlayerInputType.Interact;
            }
            else if (inputDir.magnitude != 0f)
            {
                nextState = PlayerInputType.Move;
            }
        }

        public override void TransitionIn()
        {
            nextState = PlayerInputType.None;
        }

        private IEnumerator DashHoldTimer()
        {
            yield return new WaitForSeconds(dashHoldTime);
            isDashInCoolDown = true;
            isDashing = false;
            // Bad practice to call coroutine inside other coroutine
            StartCoroutine(DashCoolDownTimer());
            actionStateManager.ChangeState(nextState);
        }

        private IEnumerator DashCoolDownTimer()
        {
            yield return new WaitForSeconds(dashCoolDownTime);
            isDashInCoolDown = false;
        }
    }
}