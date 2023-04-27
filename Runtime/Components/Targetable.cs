using System.Collections.Generic;
using System;
using UnityEngine;
          
namespace AssemblyActorCore
{
    [Serializable]
    public class Targetable
    {
        public LayerMask InteractionLayer;

        public bool IsInteraction(Target target)
        {
            if (target != null)
            {
                if (target.GetTransform != null)
                {
                    if ((InteractionLayer.value & (1 << target.GetTransform.gameObject.layer)) > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void DirectionToTarget(Transform transform, ref Target target, ref Vector2 moveDirection)
        {
            if (IsInteraction(target))
            {
                Vector2 targetPosition = new Vector2(target.GetPosition.x, target.GetPosition.z);
                Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);
                Vector2 direction = targetPosition - currentPosition;

                bool isReady = direction.magnitude > 0.1f;

                if (isReady)
                {
                    moveDirection = direction.normalized;
                }
                else
                {
                    target = null;
                }
            }
        }
    }

    [Serializable]
    public class Target
    {
        public Transform _transform = null;
        private Vector3 _position;

        public Transform GetTransform => _transform;
        public Vector3 GetPosition => _transform == null ? Vector3.zero : _position;
        public string GetTag => _transform == null ? "Null" : _transform.tag;
        //public string GetLayer => 


        public Target(Transform transform, Vector3 position)
        {
            _transform = transform;
            _position = position;
        }
    }
}