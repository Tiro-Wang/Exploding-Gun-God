using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RidingSystem : MonoBehaviour
{
    private InputAction ridingAction;
    [SerializeField] private TextMeshProUGUI ridingMessage;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject robot;
    private void Start()
    {
        ridingAction=new InputAction("Ride",binding:"<Keyboard>/F");
        ridingAction.Enable();
    }
    //�ӽ�system��ʾ����
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("23");
        ridingMessage.enabled = true;
        ridingMessage.text = "Press F To Ride The Robot";
    }
    //������ʱ���в���
    private void OnTriggerStay(Collider other)
    {
        if(ridingAction.triggered)
        {
            Ride();
        }
    }
    //�뿪��ʾ
    private void OnTriggerExit(Collider other)
    {
        ridingMessage.enabled = false;
    }
    //
    private void Ride()
    {
        player.SetActive(false);
        robot.SetActive(true);
        transform.gameObject.SetActive(false);

    }
}
