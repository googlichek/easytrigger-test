﻿using Game.Scripts.Utils;

namespace Game.Scripts.Core
{
    public class InputDrivenMovementJumpNode : BaseEntityNode<MovementState>
    {
        private HumanController _owner = default;
        private InputWrapper _inputWrapper = default;
        private CameraOperator _cameraOperator = default;

        public InputDrivenMovementJumpNode(MovementState state) : base(state)
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

            _owner.MovementComponent.Jump();
        }

        protected override void UpdateNextState()
        {
            if (_owner.MovementComponent.Velocity.y < 0 &&
                !_owner.RaycastComponent.HasGround)
            {
                NextState = MovementState.Fall;
            }

            if (_owner.MovementComponent.Velocity.y <= 0 &&
                _owner.RaycastComponent.HasGround)
            {
                NextState =
                    !_inputWrapper.Horizontal.IsEqual(0)
                        ? MovementState.Walk
                        : MovementState.Idle;
            }
        }

        protected override void UpdateNodeState()
        {
            if (_inputWrapper.IsJumpPressed &&
                !_owner.MovementComponent.AreConsecutiveJumpsDepleted)
            {
                _owner.MovementComponent.Jump();
            }

            if (_owner.MovementComponent.Velocity.y > _owner.MovementComponent.MinJumpVelocity &&
                _inputWrapper.IsJumpReleased)
            {
                _owner.MovementComponent.MinimizeJump();
            }

            _owner.MovementComponent.SetMovementDirection(_inputWrapper.Horizontal);
            _owner.AnimationComponent.SetState(AnimationStates.Jump);
        }
    }
}
