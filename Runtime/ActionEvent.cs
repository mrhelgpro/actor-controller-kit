using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionEvent : ActionBehaviour
    {
        public ActionType Type = ActionType.Interaction;    
        public float Duration = 1;
        private float _speed => 1 / Duration;
        private float _timer = 0;
        protected override void Initialization() => type = Type;

        public override void WaitLoop()
        {
            if (myGameObject.activeSelf) actionable.Activate(myGameObject);
        }

        public override void Enter()
        {
            movable.FreezAll();
            _timer = 0;
        }

        public override void UpdateLoop()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration)
            {
                actionable.Deactivate(myGameObject);
            }
        }

        public override void FixedLoop()
        {
            animatorable.Play(Name, _speed);
            movable.MoveToDirection(Vector3.zero, 0);
        }

        public override void Exit()
        {
            movable.FreezAll();
            gameObject.SetActive(false);
        }
    }
}