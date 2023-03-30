using System.Collections.Generic;
using UnityEngine;
using AssemblyActorCore;

public class InputTargetController : MonoBehaviour
{
    public List<string> InteractionTagList;
    private Targetable _targetable;
    private Inputable _inputable;
    private AssemblyActorCore.Input _input => _inputable.Input;
    private Transform _mainTransform;

    private void Awake()
    {
        _targetable = gameObject.GetComponentInParent<Targetable>();
        _inputable = gameObject.GetComponentInParent<Inputable>();
        _mainTransform = transform;
    }

    private void Update()
    {
        getTarget();
        inputToTarget();
    }

    private void inputToTarget()
    {
        if (_targetable.IsPosition)
        {
            Vector2 targetPosition = new Vector2(_targetable.GetPosition.x, _targetable.GetPosition.z);
            Vector2 currentPosition = new Vector2(_mainTransform.position.x, _mainTransform.position.z);
            Vector2 direction = targetPosition - currentPosition;

            bool isReady = direction.magnitude > 0.1f;

            _input.Move = isReady ? direction.normalized : Vector2.zero;
        }
        else
        {
            _input.Move = Vector2.zero;
        }
    }

    private void getTarget()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            _input.ActionLeft = true;

            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                foreach (string tag in InteractionTagList)
                {
                    if (hit.collider.tag == tag)
                    {
                        _targetable.AddTarget(hit.collider.transform);

                        return;
                    }
                }

                _targetable.AddTarget(hit.point);
            }
            else
            {
                _targetable.Clear();
            }
        }

        if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            _input.ActionLeft = false;
        }
    }
}
