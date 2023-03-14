using UnityEngine;
using AssemblyActorCore;

public class InputKeyboard : MonoBehaviour
{
    private Inputable _inputable;

    private void Awake()
    {
        _inputable = gameObject.GetComponentInParent<Inputable>();
    }

    void Update()
    {
        _inputable.A = Input.GetKey(KeyCode.LeftShift) ? Inputable.Key.Press : Inputable.Key.None;

        _inputable.X = Input.GetKeyDown(KeyCode.Space) ? Inputable.Key.Down : (Input.GetKey(KeyCode.Space) ? Inputable.Key.Press : Inputable.Key.None);

        Stick();
    }

    private void Stick()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float y = _inputable.X == Inputable.Key.Press ? 1 : 0;

        _inputable.Direction = new Vector3(horizontal, y, vertical);
        //GetInput.Rotation = new Vector3(horizontal, vertical, 0);
    }
}