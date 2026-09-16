using NUnit.Framework.Interfaces;
using System.Collections;
using UnityEngine;

public class Analyzer_Button : MonoBehaviour, I_Interactable
{

    public Message_Type msgType;
    public Analyzer analyzer;

    private bool canInteract = true;

    public void Detect()
    {
        if (!canInteract) return;
                        
        UI_Manager.instance.hud.Detect(Input_Manager.instance.GetKeyBindingMSG(msgType));
    }

    public void Interact(I_Player _interactingPlayer)
    {
        if (!canInteract) return;

        canInteract = false;

        Player_Look player = _interactingPlayer.GetPlayer().GetComponent<Player_Look>();
        analyzer.ProcessEarningsRpc();
        StartCoroutine(RepeatInteraction(player));
    }

    public IEnumerator RepeatInteraction(Player_Look _player)
    {
        yield return new WaitForSeconds(.5f);

        canInteract = true;

        if(_player != null)
        {
            _player.ResetInteractable();
        }
    }
}
