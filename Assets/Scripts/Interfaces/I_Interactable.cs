using UnityEngine;

public interface I_Interactable
{
    public void Detect();

    public void Interact(I_Player _interactingPlayer);

    public void Remove();

    public Item_Data GetItemData();

    public void SetItemData(Item_Data _data);

    public GameObject GetObject();
}
