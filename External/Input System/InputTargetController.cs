using System.Collections.Generic;
using UnityEngine;
using AssemblyActorCore;

public class InputTargetController : MonoBehaviour
{
    public List<string> InteractionTagList;
    
    private Targetable _targetable;
    private Inputable _inputable;
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
        //inputToTarget();
        inputLookDirection();
    }

    private void inputLookDirection()
    {
        _inputable.Look.Delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void getTarget()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            _inputable.ActionLeft = true;

            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                foreach (string tag in InteractionTagList)
                {
                    if (hit.collider.tag == tag)
                    {
                        //_targetable.AddTarget(hit.collider.transform);

                        return;
                    }
                }

                //_targetable.AddTarget(hit.point);
            }
            else
            {
                //_targetable.Clear();
            }
        }

        if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            _inputable.ActionLeft = false;
        }
    }
}
