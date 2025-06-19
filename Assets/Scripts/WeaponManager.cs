using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform weaponParent;  // куда прикреплять оружие (например, hand_right)
    [SerializeField] private InventoryControl toolbarInventory; // ссылка на тулбар
    [SerializeField] private int equipSlotIndex = 0;  // индекс слота для экипировки

    private GameObject currentWeapon;

    private void Start()
    {
        EquipFromSlot();
        toolbarInventory.OnInventoryChanged += EquipFromSlot;  // подписка на изменение
    }

    private void EquipFromSlot()
    {
        Item item = toolbarInventory.GetItemAt(equipSlotIndex);

        if (item != null && item is WeaponItem weaponItem)
        {
            EquipWeapon(weaponItem);
        }
        else
        {
            UnequipWeapon();
        }
    }

    private void EquipWeapon(WeaponItem weaponItem)
    {
        UnequipWeapon();

        currentWeapon = Instantiate(weaponItem.prefab, weaponParent);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }

    private void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
    }
}
