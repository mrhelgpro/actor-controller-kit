using UnityEngine;
          
namespace Actormachine
{
    /// <summary> Structure - for data storage about the target. </summary>
    public struct Target
    {
        private Transform _actorTransform;
        private Transform _targetTransform;
        private Vector3 _pointPosition;

        public Target(Transform actor, Transform target)
        {
            _actorTransform = actor;
            _targetTransform = target;
            _pointPosition = _targetTransform.position;
        }

        public Target(Transform actor, Transform target, Vector3 point)
        {
            _actorTransform = actor;
            _targetTransform = target;
            _pointPosition = point;
        }

        public void Clear()
        {
            _actorTransform = null;
            _targetTransform = null;
        }

        public bool IsExists => _targetTransform != null && _actorTransform != null;
        public string GetName => IsExists ? _targetTransform.name : "None";
        public Transform GetTransform => IsExists ? _targetTransform : null;
        public Vector3 GetPosition => IsExists ? _pointPosition : Vector3.zero;
        public float GetDistance => IsExists ? Vector3.Distance(GetPosition, _actorTransform.position) : 0;
        public Vector3 GetDirection => IsExists ? (GetPosition - _actorTransform.position).normalized : Vector3.zero;
        public float GetDistanceHorizontal => IsExists ? Vector2.Distance(new Vector2(GetPosition.x, GetPosition.z), new Vector2(_actorTransform.position.x, _actorTransform.position.z)) : 0;
        public Vector2 GetDirectionHorizontal => new Vector2(GetDirection.x, GetDirection.z);

        public bool IsLayer(LayerMask layerMask)
        {
            if (IsExists)
            {
                if ((layerMask.value & (1 << GetTransform.gameObject.layer)) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsTag(string tag) => IsExists ? tag == _targetTransform.tag : false;
    }
}