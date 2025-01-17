﻿using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class InputDrivenMovementWalkNode : BaseEntityNode<MovementState>
    {
        private HumanController _owner = default;
        private InputWrapper _inputWrapper = default;
        private CameraOperator _cameraOperator = default;

        public InputDrivenMovementWalkNode(MovementState state) : base(state)
        {
        }

        public void Setup(HumanController owner, InputWrapper inputWrapper, CameraOperator cameraOperator)
        {
            _owner = owner;
            _inputWrapper = inputWrapper;
            _cameraOperator = cameraOperator;
        }

        public override void Enter(MovementState from)
        {
            base.Enter(from);

            if (from == MovementState.Fall ||
                from == MovementState.Jump)
            {
                _owner.DustParticlesComponent.Emit();

                var trauma =
                    Mathf.Abs(
                        _owner.MovementComponent.VelocityYBeforeCollision /
                        _owner.MovementComponent.Data.MaxVelocityY);
                _cameraOperator.Shaker.AddMovementTrauma(trauma);
            }
        }

        protected override void UpdateNextState()
        {
            if (_inputWrapper.Horizontal.IsEqual(0))
            {
                NextState =
                    _inputWrapper.IsCrouchPressed
                        ? MovementState.Crouch
                        : MovementState.Idle;
            }

            if (_owner.MovementComponent.Velocity.y < 0 &&
                !_owner.RaycastComponent.HasGround)
            {
                NextState = MovementState.Fall;
            }

            if (_inputWrapper.IsJumpPressed &&
                !_owner.MovementComponent.AreConsecutiveJumpsDepleted)
            {
                NextState = MovementState.Jump;
            }
        }

        protected override void UpdateNodeState()
        {
            if (_owner.GunComponent.IsInUse)
                return;

            _owner.MovementComponent.SetMovementDirection(_inputWrapper.Horizontal);

            if (_owner.MeleeComponent.IsInUse)
                return;

            _owner.AnimationComponent.SetState(AnimationStates.Run);
        }
    }
}
