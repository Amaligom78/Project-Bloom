using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 0)]
public class Item_Data : ScriptableObject
{
    public Item_Type itemType;
    public int cost;
    //public int condition = 100;
    public float weight;
    public Message_Type msgType;


    public string GetItemName(Item_Type _itemType)
    {
        switch (_itemType)
        {
            case Item_Type.CHAIR:
                return "Chair";
            case Item_Type.TABLE:
                return "Table";
            case Item_Type.LAMP:
                return "Lamp";
            case Item_Type.BOOK:
                return "Book";
            default:
                return "ERROR!";
        }
    }
}

public enum Item_Type
{ 
    CHAIR,
    TABLE,
    LAMP,
    BOOK
}