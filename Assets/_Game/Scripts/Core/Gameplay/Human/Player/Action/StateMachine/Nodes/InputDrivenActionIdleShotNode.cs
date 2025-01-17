﻿namespace Game.Scripts.Core
{
    public class InputDrivenActionIdleShotNode : BaseEntityNode<ActionState>
    {
        private HumanController _owner = default;
        private InputWrapper _inputWrapper = default;

        public InputDrivenActionIdleShotNode(ActionState state) : base(state)
        {
        }

        public void Setup(HumanController owner, InputWrapper inputWrapper)
        {
            _owner = owner;
            _inputWrapper = inputWrapper;
        }

        public override void Enter(ActionState from)
        {
            base.Enter(from);

            _owner.GunComponent.Use();
        }

        protected override void UpdateNextState()
        {
            if (_owner.GunComponent.IsInUse)
                return;

            NextState = ActionState.Idle;
        }

        protected override void UpdateNodeState()
        {
            if (!_owner.GunComponent.IsInUse)
                return;

            _owner.AnimationComponent.SetState(AnimationStates.IdleShot);
        }
    }
}
