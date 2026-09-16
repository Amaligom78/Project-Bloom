using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEngine;

public class Analyzer_Button : MonoBehaviour, I_Interactable
{

    public Message_Type msgType;
    public Analyzer analyzer;
    private Player_Look playerDetecting;

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

    public void Interact(I_Player _interactingPlayer)
    {
        playerDetecting = _interactingPlayer.GetPlayer().GetComponent<Player_Look>();
        analyzer.ProcessEarningsRpc();
        StartCoroutine(RepeatInteraction());
    }

    public void Remove()
    {
        
    }

    public void SetItemData(Item_Data _data)
    {
        
    }

    public IEnumerator RepeatInteraction()
    {
        yield return new WaitForSeconds(1f);

        playerDetecting.ResetInteractable();
        playerDetecting = null;
    }
}
