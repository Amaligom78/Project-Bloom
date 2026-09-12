using UnityEngine;

public class Item : MonoBehaviour, I_Interactable
{
    [SerializeField] private Message_Type msg;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Detect()
    {
        UI_Manager.instance.hud.Detect(Input_Manager.instance.GetKeyBindingMSG(msg));
    }

    public void Interact()
    {
        gameObject.SetActive(false);
    }
}
