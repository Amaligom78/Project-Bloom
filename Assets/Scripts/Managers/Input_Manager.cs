using System;
using Unity.Netcode;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    //Binded Keys
    [Header("Key Bindings")]
    public KeyCode interactKey;
    public KeyCode dropKey;

    public static Input_Manager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        
    }


    public string GetKeyBindingMSG(Message_Type _msgType)
    {
        switch(_msgType)
        {
            case Message_Type.PICKUP:
                return "[" + interactKey.ToString() + "] Pick Up";
            case Message_Type.USE:
                return "[" + interactKey.ToString() + "] Use";
            default:
                return "ERROR!";
        }
    }
}
