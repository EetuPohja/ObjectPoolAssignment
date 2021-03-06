﻿using UnityEngine;

namespace CursedWoods
{
    public class NoneState : PlayerActionStateBase
    {
        private Vector3 velocity;

        public override PlayerInputType Type
        {
            get
            {
                return PlayerInputType.None;
            }
        }

        private void Awake()
        {
            AddTargetState(PlayerInputType.Move);
            AddTargetState(PlayerInputType.Dash);
            AddTargetState(PlayerInputType.Attack);
            AddTargetState(PlayerInputType.Spellcast);
        }

        public override void HandleInput()
        {
            Vector2 inputDir = new Vector2(Input.GetAxisRaw(CharController.HORIZONTAL), Input.GetAxisRaw(CharController.VERTICAL));

            if (Input.GetButtonDown(CharController.DASH))
            {
                actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Dash);
            }
            else if (Input.GetButtonDown(CharController.ATTACK))
            {
                actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Attack);
            }
            else if (Input.GetButtonDown(CharController.SPELLCAST))
            {
                actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Spellcast);
            }
            else if (Input.GetButtonDown(CharController.INTERACT))
            {
                actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Interact);
            }
            else if (inputDir.magnitude != 0f)
            {
                actionStateManager.PlayerRb.isKinematic = false;
                actionStateManager.ChangeState(PlayerInputType.Move);
            }
        }

        public override void DaUpdate()
        {
            Rigidbody rb = actionStateManager.PlayerRb;
            velocity = rb.velocity;
            Vector3 slowedVel = velocity * 42f;
            if (actionStateManager.CharController.IsGrounded)
            {
                if (velocity.y < 0f)
                {
                    rb.isKinematic = true;

                }
                else
                {
                    velocity = new Vector3(slowedVel.x, velocity.y, slowedVel.z);
                }
            }
            else
            {
                actionStateManager.PlayerRb.isKinematic = false;
                if (velocity.y > 0f)
                {
                    velocity = new Vector3(slowedVel.x, 0f, slowedVel.z);
                }
                else
                {
                    velocity = new Vector3(slowedVel.x, velocity.y, slowedVel.z);
                }
            }
        }

        public override void DaFixedUpdate()
        {
            actionStateManager.PlayerRb.velocity = new Vector3(velocity.x * Time.fixedDeltaTime, velocity.y, velocity.z * Time.fixedDeltaTime);
        }
    }
}