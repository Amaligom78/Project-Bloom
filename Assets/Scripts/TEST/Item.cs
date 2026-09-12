using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour, I_Interactable
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider itemCollider;

    [Header("Settings")]
    [SerializeField] private Message_Type msg;
    [SerializeField] private Vector3 heldPosition = new Vector3(0f, 1.2f, 1.5f);

    private NetworkVariable<bool> isHeld =
        new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        isHeld.OnValueChanged += OnHeldChanged;

        ApplyHeldState(isHeld.Value);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Detect()
    {
        if (isHeld.Value) return;

        UI_Manager.instance.hud.Detect(Input_Manager.instance.GetKeyBindingMSG(msg));
    }

    public void Interact()
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
        ulong clientId = rpcParams.Receive.SenderClientId;

        NetworkObject player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;

        if (player == null) return;

        isHeld.Value = true;

        rb.isKinematic = true;
        itemCollider.enabled = false;

        NetworkObject.TrySetParent(player, false);

        transform.localPosition = heldPosition;
        transform.localRotation = Quaternion.identity;
    }

    [Rpc(SendTo.Server)]
    private void RequestDropRpc()
    {
        isHeld.Value = false;

        NetworkObject.TryRemoveParent(true);

        rb.isKinematic = false;
        itemCollider.enabled = true;
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
        itemCollider.enabled = !held;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }
}
