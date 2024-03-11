using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponsSwitching : MonoBehaviour
{
    private InputAction switchAction;

    [SerializeField] private int selectWeaponIndex = 0;
    private int previouslySelectedIndex = 0; // 初始化为默认的第一个武器索引
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
        // 记录当前选中武器的索引作为上一次选择的武器索引
        previouslySelectedIndex = selectWeaponIndex;
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
        if (previouslySelectedIndex != selectWeaponIndex)
        {
            SelectWeapon();
        }

    }
    private void SelectWeapon()
    {
            // 所有武器设置为不可见
            foreach (Transform weapon in transform)
            {
                weapon.gameObject.SetActive(false);
            }

            // 激活新选中的武器
            transform.GetChild(selectWeaponIndex).gameObject.SetActive(true);
        
    }
}
