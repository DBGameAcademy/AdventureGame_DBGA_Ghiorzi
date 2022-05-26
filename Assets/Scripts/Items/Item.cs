using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string ID;
    public string Name;
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