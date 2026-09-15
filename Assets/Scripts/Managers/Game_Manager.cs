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

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void OnAddEarnings(int previousValue, int newValue)
    {
        //if (!IsServer) return;

        earnings.Value += newValue;
        UI_Manager.instance.hud.UpdateEarningsUI(earnings.Value.ToString());
    }
}
