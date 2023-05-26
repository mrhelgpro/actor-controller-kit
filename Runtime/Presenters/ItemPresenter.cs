using UnityEngine.AI;
using UnityEngine;

namespace Actormachine
{
    public class ItemPresenter : StateBehaviour, IEnableState, IDisableState
    {
        private Collider _collider;
        private Collider2D _collider2D;
        private Rigidbody _rigidbody;
        private Rigidbody2D _rigidbody2D;
        private NavMeshAgent _navMeshAgent;

        public void OnEnableState()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            _collider2D = GetComponent<Collider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _navMeshAgent = GetComponent<NavMeshAgent>();

            if (_collider) _collider.enabled = false;
            if (_rigidbody) _rigidbody.isKinematic = true;

            if (_collider2D) _collider2D.enabled = false;
            if (_rigidbody2D) _rigidbody2D.isKinematic = true;

            if (_navMeshAgent) _navMeshAgent.enabled = false;
        }

        public void OnDisableState()
        {
            if (_collider) _collider.enabled = true;
            if (_rigidbody) _rigidbody.isKinematic = false;

            if (_collider2D) _collider2D.enabled = true;
            if (_rigidbody2D) _rigidbody2D.isKinematic = false;

            if (_navMeshAgent) _navMeshAgent.enabled = true;
        }
    }
}