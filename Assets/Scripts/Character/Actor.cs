using UnityEngine;
using UnityEngine.InputSystem;

public class Actor : ScriptableObject
{
    public Transform StartingPoint;
    public LayerMask LayerMask;
    public GameObject Prefab;
    public PlayerInput PlayerInput;
    public PlayerController PlayerController;
    public InputDevice InputDevice;
    public Transform MyTransform;

    public void Spawn()
    {
        PlayerInput = PlayerInput.Instantiate(Prefab, pairWithDevice: InputDevice);
        MyTransform = PlayerInput.transform;
        MyTransform.position = StartingPoint.position;
        PlayerController = PlayerInput.transform.GetComponent<PlayerController>();
    }
}
