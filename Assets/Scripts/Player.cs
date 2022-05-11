using UnityEngine;

public class Player : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    private int experience;
    private Quest[] quests;
    private Weapon heldWeapon;
    private Armour equipedArmour;
    private Item[] consumables;
    private float potionCooldown;

    private Vector2Int _currentPosition;

    public void SetPosition(Vector2Int position)
    {
        _currentPosition = position;
        transform.position = new Vector3(_currentPosition.x, 0.28f, _currentPosition.y);

    }

    void OnKill()
    {

    }

    void OnLevelUp()
    {

    }
}
