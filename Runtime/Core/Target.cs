using UnityEngine;
          
namespace Actormachine
{
    /// <summary> Structure - for data storage about the target. </summary>
    public struct Target
    {
        private Transform _targetTransform;
        private Transform _followerTransform;
        private Vector3 _pointPosition;

        public Target(Transform follower, Transform target, Vector3 point)
        {
            _followerTransform = follower;
            _targetTransform = target;
            _pointPosition = point;
        }

        public bool IsTargetExists => _targetTransform != null;
        public Transform GetTransform => _targetTransform;
        public Vector3 GetPosition => IsTargetExists ? _pointPosition : Vector3.zero;
        public float GetTargetDistance => IsTargetExists ? Vector3.Distance(GetPosition, _followerTransform.position) : 0;
        public float GetHorizontalDistance => IsTargetExists ? Vector2.Distance(new Vector2(GetPosition.x, GetPosition.z), new Vector2(_followerTransform.position.x, _followerTransform.position.z)) : 0;
        public Vector3 GetTargetDirection => IsTargetExists ? (GetPosition - _followerTransform.position).normalized : Vector3.zero;
        public Vector2 GetHorizontalDirection => new Vector2(GetTargetDirection.x, GetTargetDirection.z);

        public bool IsRequiredLayer(LayerMask layerMask)
        {
            if (IsTargetExists)
            {
                if ((layerMask.value & (1 << GetTransform.gameObject.layer)) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void Clear() => _targetTransform = null;
    }
}