using UnityEngine;

namespace Actormachine
{
    public enum GameMode { Loading, Clip, Play, Menu }
    public static class Gameplay
    {
        public static GameMode Mode = GameMode.Play;
    }

    public abstract class GameplayComponentBase : MonoBehaviour
    {
        protected virtual void GameplayFixedUpdate() { }
        protected virtual void GameplayUpdate() { }
        protected virtual void GameplayLateUpdate() { }
    }

    public abstract class ClipComponentBase : GameplayComponentBase
    {
        private void FixedUpdate()
        {
            if (Gameplay.Mode == GameMode.Clip) GameplayFixedUpdate();
        }

        private void Update()
        {
            if (Gameplay.Mode == GameMode.Clip) GameplayUpdate();
        }

        private void LateUpdate()
        {
            if (Gameplay.Mode == GameMode.Clip) GameplayLateUpdate();
        }
    }

    public abstract class PlayComponentBase : GameplayComponentBase
    {
        private void FixedUpdate()
        {
            if (Gameplay.Mode == GameMode.Play) GameplayFixedUpdate();
        }

        private void Update()
        {
            if (Gameplay.Mode == GameMode.Play) GameplayUpdate();
        }

        private void LateUpdate()
        {
            if (Gameplay.Mode == GameMode.Play) GameplayLateUpdate();
        }
    }

    public abstract class MenuComponentBase : GameplayComponentBase
    {
        private void FixedUpdate()
        {
            if (Gameplay.Mode == GameMode.Menu) GameplayFixedUpdate();
        }

        private void Update()
        {
            if (Gameplay.Mode == GameMode.Menu) GameplayUpdate();
        }

        private void LateUpdate()
        {
            if (Gameplay.Mode == GameMode.Menu) GameplayLateUpdate();
        }
    }
}