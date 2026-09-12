using UnityEngine;
using TMPro;
using System;

public class HUD_UI : MonoBehaviour
{
    public TMP_Text detectionTxt;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

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
}
