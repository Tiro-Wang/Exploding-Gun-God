using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponsSwitching : MonoBehaviour
{
    private InputAction switchAction;

    [SerializeField] private int selectWeaponIndex = 0;
    //[SerializeField] Transform[] weapons;

    private void Start()
    {
        switchAction = new InputAction("Scroll", binding: "<Mouse>/scroll");
        switchAction.AddBinding("<Gamepad>/Dpad");
        switchAction.Enable();
        SelectWeapon();
    }
    private void Update()
    {
        float scrollValue = switchAction.ReadValue<Vector2>().y;
        float preventIndex = selectWeaponIndex;
        if (scrollValue > 0)//向上滑
        {
            selectWeaponIndex--;
            if (selectWeaponIndex == -1)
            {
                selectWeaponIndex = transform.childCount - 1;
            }
        }
        else if (scrollValue < 0)//向下滑
        {
            selectWeaponIndex++;
            if (selectWeaponIndex == transform.childCount)
            {
                selectWeaponIndex = 0;
            }
        }
        if (preventIndex!=selectWeaponIndex)
        {
            SelectWeapon();
        }
    }
    private void SelectWeapon()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }
        transform.GetChild(selectWeaponIndex).gameObject.SetActive(true);
    }
}
