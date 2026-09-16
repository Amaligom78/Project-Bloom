using UnityEngine;
using TMPro;
using System;
using Unity.Netcode;

public class HUD_UI : NetworkBehaviour
{
    public TMP_Text detectionTxt;
    public TMP_Text currentMoneyTxt;


    public void Detect(string _text)
    {
        detectionTxt.gameObject.SetActive(true);
        detectionTxt.text = _text;
    }

    public void DisableDetect()
    {
        detectionTxt.text = "";
        detectionTxt.gameObject.SetActive(false);
    }

    public void UpdateEarningsUI(string _earningsTxt)
    {
        currentMoneyTxt.text = "Earnings: $" + _earningsTxt;
    }
}
