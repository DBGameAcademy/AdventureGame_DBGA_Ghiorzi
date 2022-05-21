using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Create Player Data")]
public class PlayerData : ScriptableObject
{
    public int MaxHealth;
    public int MaxDark;
    public int BasicLightDamage;
    public int BasicDarkDamage;
    public int DarkAddAmount;
    public int DarkRemoveAmount;
}
