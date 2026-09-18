using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class Game_Manager : NetworkBehaviour
{

    public static Game_Manager instance;

    private NetworkVariable<int> earnings = new NetworkVariable<int>();
    private NetworkVariable<int> playersStarted = new NetworkVariable<int>();

    private void Awake()
    {
        instance = this;
    }

    public override void OnNetworkSpawn()
    {
        earnings.OnValueChanged += OnAddEarnings;
        playersStarted.OnValueChanged += OnChangePlayerStart;
        UpdateEarningsUI(earnings.Value);
        UpdatePlayerCount(playersStarted.Value);
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

    public void AddPlayerStartCount()
    {
        playersStarted.Value++;
    }

    public void RemovePlayerStartCount()
    {
        playersStarted.Value--;
    }

    public void OnChangePlayerStart(int previousValue, int newValue)
    {
        UpdatePlayerCount(newValue);
    }

    public void UpdatePlayerCount(int _playersStarted)
    {
        if(_playersStarted >= NetworkManager.Singleton.ConnectedClients.Count)
        {
            Debug.Log("Shift has started!");
        }
    }
}
