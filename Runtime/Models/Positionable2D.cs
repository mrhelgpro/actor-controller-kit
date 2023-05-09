using UnityEngine;

namespace AssemblyActorCore
{
    public sealed class Positionable2D : Positionable
    {
        private Collision2D _groundCollision;

        protected override void GroundCheck()
        {
            bool isGroundedCollision = _groundCollision == null ? false : true;
            bool isGroundedPhysics = Physics2D.OverlapCircle(RootTransform.position, 0.2f, groundLayer);

            IsGrounded = IsGrounded == true ? isGroundedPhysics : isGroundedCollision && isGroundedPhysics;
        }

        protected override void SurfaceCheck()
        {
            SurfaceType = IsGrounded == true && _groundCollision != null ? _groundCollision.gameObject.tag : "None";
            SurfaceNormal = IsGrounded == true && _groundCollision != null ? _groundCollision.contacts[0].normal : Vector3.zero;
        }

        protected override void ObstacleCheck()
        {
            float length = 0.35f;
            Vector3 origin = new Vector3(RootTransform.position.x, RootTransform.position.y + 0.25f, RootTransform.position.z);
            Vector3 direction = RootTransform.TransformDirection(Vector3.forward);
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, length, layerMask);

            IsObstacle = hit.collider == null ? false : hit.collider.isTrigger ? false : true;
        }

        private void OnCollisionStay2D(Collision2D collision) => _groundCollision = collision;
        private void OnCollisionExit2D(Collision2D collision) => _groundCollision = null;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [UnityEditor.CustomEditor(typeof(Positionable2D))]
    public class Positionable2DEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            DrawModelBox("Ñhecks 2D position data");
        }
    }
#endif
}