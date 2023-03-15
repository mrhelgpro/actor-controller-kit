using UnityEngine;
//using UnityEngine.InputSystem;
using AssemblyActorCore;

public class InputKeyboard : MonoBehaviour
{
    private Inputable _inputable;
    //private InputAction _inputAction;
 
    private void Awake()
    {
        _inputable = gameObject.GetComponentInParent<Inputable>();
    }

    void Update()
    {
        //_inputable.A = Input.GetKey(KeyCode.LeftShift) ? KeyState.Press : KeyState.None;

        //_inputable.X = Input.GetKeyDown(KeyCode.Space) ? KeyState.Down : (Input.GetKey(KeyCode.Space) ? KeyState.Press : KeyState.None);

        //_inputable.KeyA.SetState(Input.GetKey(KeyCode.Space));

        Stick();
    }

    private void Stick()
    {
        //float vertical = Input.GetAxis("Vertical");
        //float horizontal = Input.GetAxis("Horizontal");
        float y = _inputable.KeyX.IsPress ? 1 : 0;

        //_inputable.Direction = new Vector3(horizontal, y, vertical);
        //GetInput.Rotation = new Vector3(horizontal, vertical, 0);
    }
}