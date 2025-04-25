using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;

    protected override void Awake()
    {
        //codegoc:
        // base.Awake();

        // playerControls = new PlayerControls();

        //codesua:
        playerControls = new PlayerControls();
        base.Awake();
    }

    private void Start()
    {
        // codegoc: playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

        //codesua:
        playerControls.Inventory.Keyboard.performed += ctx =>
        {
            if (gameObject.activeInHierarchy) // Chi chay khi GameObject active
            {
                ToggleActiveSlot((int)ctx.ReadValue<float>());
            }
        };
        //codesua:
        EquipStartingWeapon(); // Gan vu khi khoi dau trong Start
    }

    private void OnEnable()
    {
        playerControls.Enable();
        EquipStartingWeapon(); // Gan lai vu khi khi bat lai
    }

    //codesua: code goc khong co OnDisable()
    private void OnDisable()
    {
        if (playerControls != null) // Kiem tra null truoc khi goi Disable
        {
            playerControls.Disable();
        }
    }

    public void EquipStartingWeapon()
    {
        //codegoc: ToggleActiveHighlight(0);

        //codesua:
        if (gameObject.activeInHierarchy)
        {
            ToggleActiveHighlight(0);
        }
    }

    private void ToggleActiveSlot(int numValue)
    {
        //codegoc: ToggleActiveHighlight(numValue - 1);

        //code sua:
        int index = numValue - 1;
        if (index >= 0 && index < transform.childCount) // Kiem tra gioi han
        {
            ToggleActiveHighlight(index);
        }
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        //codegoc:
        //activeSlotIndexNum = indexNum;

        //foreach (Transform inventorySlot in this.transform)
        //{
        //    inventorySlot.GetChild(0).gameObject.SetActive(false);
        //}

        // this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        // ChangeActiveWeapon();

        //codesua:
        if (!gameObject.activeInHierarchy || transform.childCount <= indexNum || indexNum < 0)
        {
            return; // Thoat neu GameObject khong active hoac index khong hop le
        }

        activeSlotIndexNum = indexNum;

        // Tat tat ca highlight
        foreach (Transform inventorySlot in this.transform)
        {
            Transform highlight = inventorySlot.GetChild(0);
            if (highlight != null)
            {
                highlight.gameObject.SetActive(false);
            }
        }

        // Bat highlight cho slot duoc chon
        Transform activeSlot = this.transform.GetChild(indexNum);
        if (activeSlot != null && activeSlot.childCount > 0)
        {
            activeSlot.GetChild(0).gameObject.SetActive(true);
        }

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        //codegoc:
        //if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        // {
        //    Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        // }

        //Transform childTransform = transform.GetChild(activeSlotIndexNum);
        // InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        // WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        // GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        //  if (weaponInfo == null)
        //  {
        //     ActiveWeapon.Instance.WeaponNull();
        //     return;
        // }

        //  GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);

        //codegoc_honnua//ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        //codegoc_honnua//newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        // ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());

        //codesua:
        if (!gameObject.activeInHierarchy || ActiveWeapon.Instance == null)
        {
            return; // Thoat neu GameObject khong active hoac ActiveWeapon null
        }

        // Huy vu khi hien tai neu co
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        // Lay thong tin slot
        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        if (childTransform == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        if (inventorySlot == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        if (weaponInfo == null || weaponInfo.weaponPrefab == null) // Kiem tra slot trong
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        // Spawn vu khi moi neu slot co vat pham
        GameObject newWeapon = Instantiate(weaponInfo.weaponPrefab, ActiveWeapon.Instance.transform);
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
