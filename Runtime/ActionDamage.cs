using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActionDamage : ActionTemporary
    {
        public float Damage = 1.0f;
        private Healthable _healthable;

        public override void Enter()
        {
            base.Enter();

            _healthable = mainTransform.GetComponent<Healthable>();
            _healthable.TakeDamage(Damage);

            if (_healthable.IsDead)
            {
                Name = "Death";
                Type = ActionType.Required;
            }
        }

        public override void UpdateLoop()
        {
            if (_healthable.IsDead == false)
            {
                base.UpdateLoop();
            }
        }
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