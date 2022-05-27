using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string ID;
    public string Name;
    [TextArea]
    public string Description;
    public Sprite Image;

    public enum eItemType
    {
        None,
        Armour,
        Weapon,
        Consumable
    }
    public eItemType Type;
}