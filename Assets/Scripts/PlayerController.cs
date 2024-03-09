using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private Vector3 move;
    private float moveSpeed = 15f;
    private float mouseSensitivity = 100f;
    [SerializeField] GameObject playerHead;
    private float xRotate;
    private Vector3 verticality;
    private float gravity = -9.87f;
    private float jumpForce = 8f;
    //�������߼���Ƿ��ڵ�����
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject groundCheck;
    private float groundCheckDistance = 0.3f;
    private bool isGrounded = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        //��mouse��������Ϸ����
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //��������ƶ�
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        move = transform.right * moveX + transform.forward * moveY;
        characterController.Move(move * moveSpeed * Time.deltaTime);
        //��������ת
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        //��x����Playerͷ�����ת��
        //���ַ�ʽÿ��ֻ�Ե�ǰ����ת�Ƕ���һ��΢С���������ģ�������һ������������ȵ���ת����������������������ת���̸���������ƽ�ȡ�
        xRotate -= mouseY;
        //������ת�Ƕ�,math=>int  mathf=>float
        xRotate = Mathf.Clamp(xRotate, -80, 60);
        //playerHead.transform.Rotate(Vector3.right * xRotate);��һֱ��ͣ����ת
        //���������������ľֲ���ת����ʹ������������ת������localRotationֻ��ı��Transform�����ڸ���������ϵ�µ���ת
        //������localRotation���ԣ�����playerHead�ľֲ���תһ��������Ϊ��xRotate����������ת,
        //����һ����ÿ��Updateʱ��������������ת�Ƕȣ��������ۼ���ת��
        playerHead.transform.localRotation = Quaternion.Euler(xRotate, 0, 0);

        // ����ɫ�Ƿ񴥵�
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckDistance, groundLayer);
        /*        if(isGrounded && verticality.y<0)
                {
        //�Ὣ��ֱ�ٶ�Ӳ����Ϊ-1������ζ��ֻҪ��ɫ�ڵ������Ҵ�ֱ�ٶ�С��0��
        //�ͽ��������ٶȹ̶�Ϊһ���ض�ֵ��������-1��������ܻ�ı��ɫ����ʱ��������Ϊ��
        //ʹ�������ٶȱ�ú㶨����������ʱ���𽥼ӿ졣����ûʲô��������
                    verticality.y = -1;
                }*/
        //���������Ծ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //��Ծ����jumpForce��ͨ����һ��˲��ʩ�ӵĳ�ʼ������
            //����ʹ��ɫ�����뿪���濪ʼ��������������������ÿ֡�����ۻ��ģ���Ծ��ֻ�ڰ���������һ֡��Ч��
            verticality.y = jumpForce;
        }
        else
        {
            //���ۻ�����Ӱ��ʱ��ȷ��ÿһ֡���������ٶ��Ǻ㶨�ģ�����֡��Ӱ�졣
            //+=ʵ�ֽ�ɫ�𽥼�������Ĺ��̡�
            verticality.y += gravity*Time.deltaTime;
        }
        //��ִ��ʵ���ƶ�ʱ�����ٶ�����ת��Ϊÿ֡��ʵ��λ�ƣ�ͬ����֤���ƶ��ٶ���֡���޹أ�ʵ��ƽ���������˶�ģ�⡣
        characterController.Move(verticality*Time.deltaTime);
    }
}
