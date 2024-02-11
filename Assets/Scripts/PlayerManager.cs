using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerInputManager m_playerInputManager;
    private void Awake()
    {
        m_playerInputManager =  GetComponent<PlayerInputManager>();
    }
}
