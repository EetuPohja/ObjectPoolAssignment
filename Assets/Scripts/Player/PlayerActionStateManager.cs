﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace CursedWoods
{
    public class PlayerActionStateManager : MonoBehaviour
    {
        public Rigidbody PlayerRb
        {
            get;
            private set;
        }

        public Transform CamT
        {
            get;
            private set;
        }

        // List of all the player action states aka stuff that happens from player inputs.
        private List<PlayerActionStateBase> playerActionStates = new List<PlayerActionStateBase>();

        public PlayerActionStateBase CurrentState
        {
            get;
            private set;
        }

        public PlayerActionStateBase PreviousState
        {
            get;
            private set;
        }

        public CharController CharController { get; private set; }

        private void Awake()
        {
            PlayerRb = GetComponent<Rigidbody>();
            CamT = Camera.main.transform;
            CharController = GetComponent<CharController>();
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            CurrentState.HandleInput();
            CurrentState.DaUpdate();
        }

        private void FixedUpdate()
        {
            CurrentState.DaFixedUpdate();
        }

        private void Init()
        {
            // Populates playerStates with all player move states.
            for (int i = 0; i < Enum.GetNames(typeof(PlayerInputType)).Length; i++)
            {
                PlayerActionStateBase state;
                state = CreateStateByType((PlayerInputType)i);
                state.Init(this);
                playerActionStates.Add(state);
            }

            // Set type to idle at the start.
            CurrentState = GetStateByType(PlayerInputType.None);
        }

        public bool ChangeState(PlayerInputType nextStateType)
        {
            // Let's check first if we can transition from current state to the target state or not
            if (!CurrentState.IsValidTargetState(nextStateType))
            {
                return false;
            }

            // Fetch the next state object
            PlayerActionStateBase nextState = GetStateByType(nextStateType);
            if (nextState == null)
            {
                return false;
            }

            CurrentState.TransitionOut();
            PreviousState = CurrentState;
            CurrentState = nextState;
            CurrentState.TransitionIn();

            return true;
        }

        private PlayerActionStateBase GetStateByType(PlayerInputType nextMoveStageType)
        {
            PlayerActionStateBase nextState = null;
            foreach (PlayerActionStateBase state in playerActionStates)
            {
                if (state.Type == nextMoveStageType)
                {
                    nextState = state;
                    break;
                }
            }

            return nextState;
        }

        private PlayerActionStateBase CreateStateByType(PlayerInputType wantedType)
        {
            PlayerActionStateBase wantedStage = null;
            switch (wantedType)
            {
                case PlayerInputType.None:
                    wantedStage = gameObject.AddComponent<NoneState>();
                    break;
                case PlayerInputType.Move:
                    wantedStage = gameObject.AddComponent<MoveState>();
                    break;
                case PlayerInputType.Dash:
                    wantedStage = gameObject.AddComponent<DashState>();
                    break;
                case PlayerInputType.Attack:
                    wantedStage = gameObject.AddComponent<AttackState>();
                    break;
                case PlayerInputType.Spellcast:
                    wantedStage = gameObject.AddComponent<SpellcastState>();
                    break;
                case PlayerInputType.Interact:
                    wantedStage = gameObject.AddComponent<InteractState>();
                    break;
            }

            return wantedStage;
        }
    }
}