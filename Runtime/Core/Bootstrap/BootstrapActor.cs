using System.Collections.Generic;

namespace Actormachine
{
    public sealed class BootstrapActor : Bootstrap
    {
        private static List<Actor> _actorList = new List<Actor>();

        public static List<Actor> GetActors => _actorList;

        public override void Initiation()
        {
            // Update all Actors    
            findAllActors();

            // Check Single Instance
            BootstrapExtantion.IsSingleInstanceOnScene<BootstrapActor>();
        }

        private void OnDisable()
        {
            _actorList.Clear();
        }

        public static void AddActor(Actor actor)
        {
            if (_actorList.Exists(a => a == actor))
            {
                return;
            }

            _actorList.Add(actor);
        }

        private void Update()
        {
            if (BootstrapGame.Mode == GameMode.Play)
            {
                foreach (Actor actor in _actorList) actor.UpdateLoop();
            }
        }

        private void FixedUpdate()
        {
            if (BootstrapGame.Mode == GameMode.Play)
            {
                foreach (Actor actor in _actorList) actor.FixedUpdateLoop();
            }
        }

        private void findAllActors()
        {
            _actorList.Clear();

            Actor[] actors = FindObjectsOfType<Actor>();

            foreach (Actor actor in actors)
            {
                _actorList.Add(actor);
            }
        }
    }
}