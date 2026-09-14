using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Analyzer : NetworkBehaviour
{

    private I_Interactable currentItem;

    //Network Variables
    [SerializeField] private NetworkVariable<Item_Type> requestedItemType = new NetworkVariable<Item_Type>();
    [SerializeField] private NetworkVariable<bool> rightItemPlaced =  new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {

    }

    public void GenerateRequestedItem()
    {
        if(!IsServer) return;

        requestedItemType.Value = Item_Type.CHAIR;
    }

    public void CheckItemData()
    {
        if(currentItem == null)
        {
            rightItemPlaced.Value = false;
            return;
        }

        rightItemPlaced.Value = currentItem.GetItemData().itemType == requestedItemType.Value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (currentItem != null) return;

        I_Interactable interactable = other.GetComponentInParent<I_Interactable>();

        if(interactable == null) return;

        Item_Data itemData = interactable.GetItemData();

        if(itemData == null) return;

        currentItem = interactable;
        CheckItemData();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        if (currentItem == null) return;

        I_Interactable interactable = other.GetComponentInParent<I_Interactable>();

        if (interactable == null) return;
        if (interactable != currentItem) return;

        currentItem = null;
        rightItemPlaced.Value = false;
    }
}
