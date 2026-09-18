using System.Threading;
using TMPro;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Clock_UI : NetworkBehaviour
{

    public TMP_Text clockTxt;
    private float timer = 0f;

    [SerializeField] private NetworkVariable<bool> isTicking = new NetworkVariable<bool>(false);
    [SerializeField] private NetworkVariable<int> remainingSeconds = new NetworkVariable<int>(60);


    public override void OnNetworkSpawn()
    {
        remainingSeconds.OnValueChanged += OnTimeChanged;
        UpdateClockUI(remainingSeconds.Value);
    }

    public override void OnNetworkDespawn()
    {
        remainingSeconds.OnValueChanged -= OnTimeChanged;
    }

    void Update()
    {
        if (!IsServer) return;
        if (!isTicking.Value) return;

        timer += Time.deltaTime;

        if(timer >= 1f)
        {
            timer -= 1f;

            if(remainingSeconds.Value > 0)
            {
                remainingSeconds.Value--;
            }

            if(remainingSeconds.Value <= 0)
            {
                remainingSeconds.Value = 0;
                isTicking.Value = false;

                Debug.Log("Times Up!");
            }
        }
    }

    public void SetClock(int seconds)
    {
        if (!IsServer) return;

        timer = 0f;

        remainingSeconds.Value = Mathf.Max(0, seconds);
        isTicking.Value = remainingSeconds.Value > 0;
    }

    private void OnTimeChanged(int previousValue, int newValue)
    {
        UpdateClockUI(newValue);
    }

    private void UpdateClockUI(int _totalSeconds)
    {
        int minutes = _totalSeconds / 60;
        int seconds = _totalSeconds % 60;

        clockTxt.text = $"{minutes:00}:{seconds:00}";
    }
}
