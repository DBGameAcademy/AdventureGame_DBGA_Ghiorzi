using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemsCreator
{
    public static string CollectionName = "ItemCollection";
    public static string Path = "Assets/ScriptableObjects/Items/";
    [MenuItem("Assets/Create/Items/Equipable/Create Weapon Item")]
    public static void MakeNewWeapon()
    {
        // Create the Assets
        WeaponItem weaponAssets = ScriptableObject.CreateInstance<WeaponItem>();
        AssetDatabase.CreateAsset(weaponAssets, Path+"NewWeapon.asset");
        AssetDatabase.SaveAssets();

        // Load the so asset
        var collection = Resources.Load<ItemCollection>(CollectionName);

        // Add data
        collection.Items.Add(weaponAssets);

        // Tell Unity to save the changes
        EditorUtility.SetDirty(collection);
    }

    [MenuItem("Assets/Create/Items/Equipable/Create Armour Item")]
    public static void MakeNewArmour()
    {
        // Create the Assets
        ArmourItem armourAssets = ScriptableObject.CreateInstance<ArmourItem>();
        AssetDatabase.CreateAsset(armourAssets, Path + "NewArmour.asset");
        AssetDatabase.SaveAssets();

        // Load the so asset
        var collection = Resources.Load<ItemCollection>(CollectionName);

        // Add data
        collection.Items.Add(armourAssets);

        // Tell Unity to save the changes
        EditorUtility.SetDirty(collection);
    }

    [MenuItem("Assets/Create/Items/Consumable/Create Health Potion")]
    public static void MakeNewHealthPotion()
    {
        // Create the Assets
        HealthPotionItem potionAssets = ScriptableObject.CreateInstance<HealthPotionItem>();
        AssetDatabase.CreateAsset(potionAssets, Path + "NewHealthPotion.asset");
        AssetDatabase.SaveAssets();

        // Load the so asset
        var collection = Resources.Load<ItemCollection>(CollectionName);

        // Add data
        collection.Items.Add(potionAssets);

        // Tell Unity to save the changes
        EditorUtility.SetDirty(collection);
    }
}
