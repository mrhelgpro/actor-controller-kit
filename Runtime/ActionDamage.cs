using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActionDamage : ActionInteraction
    {
        private Healthable _healthable;

        protected new void Awake()
        {
            base.Awake();

            Type = ActionType.Forced;
            Name = "Damage";
            Duration = 0.25f;

            _healthable = mainTransform.gameObject.AddThisComponent<Healthable>();
        }

        private void OnEnable() => _healthable.EventDamage += DamageHandler;

        protected virtual void DamageHandler()
        {
            if (_healthable.IsDead == false)
            {
                TryToActivate();
            }
            else
            {
                Name = "Death";
                Type = ActionType.Required;

                TryToActivate();
            }
        }

        public override void UpdateLoop()
        {
            if (_healthable.IsDead == false)
            {
                base.UpdateLoop();
            }
        }

        private void OnDisable() => _healthable.EventDamage -= DamageHandler;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActionDamage))]
    public class ActionDamageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Default <Action> - to deal damage");
        }
    }
#endif
}