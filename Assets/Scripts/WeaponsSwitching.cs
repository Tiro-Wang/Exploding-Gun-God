using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponsSwitching : MonoBehaviour
{
    private InputAction switchAction;

    [SerializeField] private int selectWeaponIndex = 0;
    private int previouslySelectedIndex = 0; // ��ʼ��ΪĬ�ϵĵ�һ����������
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
        // ��¼��ǰѡ��������������Ϊ��һ��ѡ�����������
        previouslySelectedIndex = selectWeaponIndex;
        if (scrollValue > 0)//���ϻ�
        {
            selectWeaponIndex--;
            if (selectWeaponIndex == -1)
            {
                selectWeaponIndex = transform.childCount - 1;
            }
        }
        else if (scrollValue < 0)//���»�
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
            // ������������Ϊ���ɼ�
            foreach (Transform weapon in transform)
            {
                weapon.gameObject.SetActive(false);
            }

            // ������ѡ�е�����
            transform.GetChild(selectWeaponIndex).gameObject.SetActive(true);
        
    }
}
