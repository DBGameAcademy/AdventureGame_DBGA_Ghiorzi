using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Create Player Data")]
public class PlayerData : ScriptableObject
{
    public int MaxHealth;
    public int MaxDark;
    public int[] BasicLightDamages = new int[3];
    public int[] BasicDarkDamages = new int[3];
    public int DarkAddAmount;
    public int DarkRemoveAmount;
}
