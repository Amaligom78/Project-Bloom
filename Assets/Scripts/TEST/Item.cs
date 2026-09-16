using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : NetworkBehaviour, I_Interactable, I_Holdable
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider itemCollider;
    [SerializeField] private Item_Data itemData;

    [Header("Settings")]
    private Transform currentHoldPoint;


    //Server Variables
    private NetworkVariable<bool> isHeld = new NetworkVariable<bool>(false);
    private ulong holdingClientID = ulong.MaxValue;

    public override void OnNetworkSpawn()
    {
        isHeld.OnValueChanged += OnHeldChanged;
        ApplyHeldState(isHeld.Value);

        gameObject.name = itemData.GetItemName(itemData.itemType);
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (!isHeld.Value)
            return;

        if (currentHoldPoint == null)
            return;

        transform.position = currentHoldPoint.position;
    }

    public void Detect()
    {
        if (isHeld.Value) return;

        UI_Manager.instance.hud.Detect(Input_Manager.instance.GetKeyBindingMSG(itemData.msgType));
    }

    public void Interact(I_Player _interactingPlayer)
    {
        if (!isHeld.Value)
        {
            RequestPickupRpc();
        }
        else
        {
            RequestDropRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestPickupRpc(RpcParams rpcParams = default)
    {
        if (isHeld.Value) return;

        ulong clientId = rpcParams.Receive.SenderClientId;
        NetworkObject player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;

        if (player == null) return;

        Player_Look playerLook = player.GetComponent<Player_Look>();

        currentHoldPoint = playerLook.holdYPoint;

        holdingClientID = clientId;
        isHeld.Value = true;
        NetworkObject.TrySetParent(player, false);

        transform.position = currentHoldPoint.position;
        //transform.localRotation = Quaternion.identity;
    }

    [Rpc(SendTo.Server)]
    private void RequestDropRpc(RpcParams rpcParams = default)
    {
        ulong clientID = rpcParams.Receive.SenderClientId;

        if(!isHeld.Value) return;
        if (holdingClientID != clientID) return;

        NetworkObject.TryRemoveParent(true);
        currentHoldPoint = null;
        isHeld.Value = false;
    }

    public override void OnNetworkDespawn()
    {
        isHeld.OnValueChanged -= OnHeldChanged;
    }

    private void OnHeldChanged(bool previousValue, bool newValue)
    {
        ApplyHeldState(newValue);
    }

    private void ApplyHeldState(bool held)
    {
        rb.isKinematic = held;
        //itemCollider.enabled = !held;
    }

    public void Remove()
    {
        RequestDropRpc();
    }

    public Item_Data GetItemData()
    {
        if(itemData != null)
        {
            return itemData;
        }

        Debug.LogError(gameObject.name + " Requires Data!");
        return null;
    }

    public void SetItemData(Item_Data _data)
    {
        itemData = _data;
    }
}
