using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public class Storagable : ModelBehaviour
    {
        public List<Transform> InventorySlots;
        public List<Transform> ActiveSlots;

        public Transform GetInventoryItem(int number)
        {
            if (InventorySlots.Count > number)
            {
                if (InventorySlots[number].transform.childCount > 0)
                {
                    return InventorySlots[number].transform.GetChild(0);
                }
            }

            return null;
        }

        public Transform GetInventorySlot(int number)
        {
            return InventorySlots.Count > number ? InventorySlots[number] : null;
        }

        public Transform GetActiveSlot(int number)
        {
            return ActiveSlots.Count > number ? ActiveSlots[number] : null;
        }
    }
}