using NUnit.Framework.Interfaces;
using UnityEngine;

public class Analyzer_Button : MonoBehaviour, I_Interactable
{

    public Message_Type msgType;
    public Analyzer analyzer;

    public void Detect()
    {
        UI_Manager.instance.hud.Detect(Input_Manager.instance.GetKeyBindingMSG(msgType));
    }

    public Item_Data GetItemData()
    {
        return null;
    }

    public GameObject GetObject()
    {
        return null;
    }

    public void Interact()
    {
        analyzer.ProcessEarningsRpc();
    }

    public void Remove()
    {
        
    }

    public void SetItemData(Item_Data _data)
    {
        
    }
}
