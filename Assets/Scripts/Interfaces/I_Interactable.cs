using UnityEngine;

public interface I_Interactable
{
    public void Detect();
    public void Interact();

    public void Remove();

    public GameObject GetObject();
}
