using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class Game_Manager : NetworkBehaviour
{

    public static Game_Manager instance;

    private NetworkVariable<int> earnings = new NetworkVariable<int>();

    private void Awake()
    {
        instance = this;
    }

    public override void OnNetworkSpawn()
    {
        earnings.OnValueChanged += OnAddEarnings;
        UpdateEarningsUI(earnings.Value);
    }

    public override void OnNetworkDespawn()
    {
        earnings.OnValueChanged -= OnAddEarnings;
    }

    public void AddEarnings(int _amount)
    {
        if (!IsServer) return;

        earnings.Value += _amount;
    }

    public void OnAddEarnings(int previousValue, int newValue)
    {
        UpdateEarningsUI(newValue);
    }

    public void UpdateEarningsUI(int _amount)
    {
        UI_Manager.instance.hud.UpdateEarningsUI(_amount.ToString());
    }

}
