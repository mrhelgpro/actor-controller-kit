using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(ItemProperty))]
    [CanEditMultipleObjects]
    public class ItemPropertyInspector : ActormachineBaseInspector
    {
        public override void OnInspectorGUI()
        {
            ItemProperty thisTarget = (ItemProperty)target;

            thisTarget.StorageType = (StorageType)EditorGUILayout.EnumPopup("Storage Type", thisTarget.StorageType);

            if (thisTarget.StorageType == StorageType.AddToInventory)
            {
                thisTarget.ActiveType = (ActiveType)EditorGUILayout.EnumPopup("Active Type", thisTarget.ActiveType);
                thisTarget.ReplacementType = (ReplacementType)EditorGUILayout.EnumPopup("Replacement Type", thisTarget.ReplacementType);
                thisTarget.InventorySlotNumber = EditorGUILayout.IntField("Inventory Slot Number", thisTarget.InventorySlotNumber);

                if (thisTarget.ActiveType == ActiveType.ActiveSlot)
                {
                    thisTarget.ActiveSlotNumber = EditorGUILayout.IntField("Active Slot Number", thisTarget.ActiveSlotNumber);
                }
            }
            else if (thisTarget.StorageType == StorageType.TakeAndActivate)
            {
                thisTarget.ActiveSlotNumber = EditorGUILayout.IntField("Active Slot Number", thisTarget.ActiveSlotNumber);
            }
        }
    }
}