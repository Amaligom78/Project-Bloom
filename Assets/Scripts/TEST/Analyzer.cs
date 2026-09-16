using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Analyzer : NetworkBehaviour
{

    private I_Item currentItem;
    private float nextProcessTime;

    //Network Variables
    [SerializeField] private NetworkVariable<Item_Type> requestedItemType = new NetworkVariable<Item_Type>();
    [SerializeField] private NetworkVariable<bool> rightItemPlaced =  new NetworkVariable<bool>(false);
    [SerializeField] private NetworkVariable<int> itemCost = new NetworkVariable<int>();

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

        I_Item interactable = other.GetComponentInParent<I_Item>();

        if(interactable == null) return;

        Item_Data itemData = interactable.GetItemData();

        if(itemData == null) return;

        currentItem = interactable;
        itemCost.Value = itemData.cost;
        CheckItemData();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        if (currentItem == null) return;

        I_Item interactable = other.GetComponentInParent<I_Item>();

        if (interactable == null) return;
        if (interactable != currentItem) return;

        currentItem = null;
        rightItemPlaced.Value = false;
    }

    [Rpc(SendTo.Server)]
    public void ProcessEarningsRpc(RpcParams rpcParams = default)
    {
        if (Time.time < nextProcessTime) return;
        if (currentItem == null) return;
        if(!rightItemPlaced.Value) return;

        nextProcessTime = Time.time + 1f;
        Game_Manager.instance.AddEarnings(itemCost.Value);
    }
}
