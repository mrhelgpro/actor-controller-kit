using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionOnEnable : ActionBehaviour
    {
        public ActionType Type = ActionType.Interaction;    
        public float Duration = 1;
        private float _speed => 1 / Duration;
        private float _timer = 0;

        private void OnEnable() => actionable.Activate(myGameObject);

        protected override void Initialization() => type = Type;

        public override void WaitLoop() => myGameObject.SetActive(false);

        public override void Enter() 
        {
            movable.FreezAll();
            _timer = 0;
        }

        public override void UpdateLoop()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration) actionable.Deactivate(myGameObject);
        }

        public override void FixedLoop()
        {
            animatorable.Play(Name, _speed);
            movable.MoveToDirection(Vector3.zero, 0);
        }

        public override void Exit()
        {
            myGameObject.SetActive(false);
            movable.FreezAll();
        }
    }
}