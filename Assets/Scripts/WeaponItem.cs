using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public GameObject prefab;      
    public float fireRate;
    public int magazineSize;
    public float reloadTime;
}
