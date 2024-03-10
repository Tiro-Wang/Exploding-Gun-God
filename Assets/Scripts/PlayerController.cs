using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private CharacterController characterController;
    private Vector3 move;
    private float moveSpeed = 10f;
    private float mouseSensitivity = 50f;
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

    [SerializeField] private Animator animator;

    //ʹ�����������ϵͳ
    private InputAction moveAction;
    private InputAction jumpAction;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        //��mouse��������Ϸ����
        //Cursor.lockState = CursorLockMode.Locked;

        jumpAction = new InputAction("Jump", binding: "<Gamepad>/a");
        jumpAction.AddBinding("<keyboard>/space");
        jumpAction.Enable();
        moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("Dpad")//���һ����Ƽ�
            .With("Up", "<keyboard>/w")
            .With("Up", "<keyboard>/upArrow")
            .With("Down", "<keyboard>/s")
            .With("Down", "<keyboard>/downArrow")
            .With("Left", "<keyboard>/a")
            .With("Left", "<keyboard>/leftArrow")
            .With("Right", "<keyboard>/d")
            .With("Right", "<keyboard>/rightArrow");
        moveAction.Enable();
    }
    private void Update()
    {
        /*        //��������ƶ�
                float moveX = Input.GetAxis("Horizontal");
                float moveY = Input.GetAxis("Vertical");*/
        //��
        float moveX = moveAction.ReadValue<Vector2>().x;
        float moveY = moveAction.ReadValue<Vector2>().y;
        move = transform.right * moveX + transform.forward * moveY;
        characterController.Move(move * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed",Mathf.Abs(moveY)+Mathf.Abs(moveX));//ȡ����ֵ
        /*        //��������ת
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                transform.Rotate(Vector3.up * mouseX);*/
        //��
        float mouseX = 0;
        float mouseY = 0;
        if (Mouse.current != null)//�жϵ�ǰʹ�õ��Ե�������
        {
            mouseX = Mouse.current.delta.ReadValue().x;
            mouseY = Mouse.current.delta.ReadValue().y;
        }
        if (Gamepad.current != null)
        {
            mouseX = Gamepad.current.rightStick.ReadValue().x;
            mouseY = Gamepad.current.rightStick.ReadValue().y;
        }
        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime);
        //��x����Playerͷ�����ת��
        //���ַ�ʽÿ��ֻ�Ե�ǰ����ת�Ƕ���һ��΢С���������ģ�������һ������������ȵ���ת����������������������ת���̸���������ƽ�ȡ�
        xRotate -= mouseY * Time.deltaTime;
        //������ת�Ƕ�,math=>int  mathf=>float
        xRotate = Mathf.Clamp(xRotate, -80, 60);
        //playerHead.transform.Rotate(Vector3.right * xRotate);��һֱ��ͣ����ת
        //���������������ľֲ���ת����ʹ������������ת������localRotationֻ��ı��Transform�����ڸ���������ϵ�µ���ת
        //������localRotation���ԣ�����playerHead�ľֲ���תһ��������Ϊ��xRotate����������ת,
        //����һ����ÿ��Updateʱ��������������ת�Ƕȣ��������ۼ���ת��
        playerHead.transform.localRotation = Quaternion.Euler(xRotate, 0, 0);

        // ����ɫ�Ƿ񴥵�
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckDistance, groundLayer);
        /*        if (isGrounded && verticality.y < 0)
                {
                    //�Ὣ��ֱ�ٶ�Ӳ����Ϊ-1������ζ��ֻҪ��ɫ�ڵ������Ҵ�ֱ�ٶ�С��0��
                    //�ͽ��������ٶȹ̶�Ϊһ���ض�ֵ��������-1��������ܻ�ı��ɫ����ʱ��������Ϊ��
                    //ʹ�������ٶȱ�ú㶨����������ʱ���𽥼ӿ졣����ûʲô��������
                    verticality.y = -1f;
                }*/
        //���������Ծ
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //��
        //if (jumpAction.ReadValue<float>()>0 && isGrounded)//floatֵ���в���ȫ���ʱ�������Ȼ��Ǵ��벻��ȷ,
        if (Mathf.Approximately(jumpAction.ReadValue<float>(), 1f) && isGrounded)//ʹ����ѧ�����ķ������ж�����ֵ���ӽ�
        {
            //��Ծ����jumpForce��ͨ����һ��˲��ʩ�ӵĳ�ʼ������
            //����ʹ��ɫ�����뿪���濪ʼ��������������������ÿ֡�����ۻ��ģ���Ծ��ֻ�ڰ���������һ֡��Ч��
            verticality.y = jumpForce;
        }
        else
        {
            //���ۻ�����Ӱ��ʱ��ȷ��ÿһ֡���������ٶ��Ǻ㶨�ģ�����֡��Ӱ�졣
            //+=ʵ�ֽ�ɫ�𽥼�������Ĺ��̡�
            verticality.y += gravity * Time.deltaTime;
        }
        //��ִ��ʵ���ƶ�ʱ�����ٶ�����ת��Ϊÿ֡��ʵ��λ�ƣ�ͬ����֤���ƶ��ٶ���֡���޹أ�ʵ��ƽ���������˶�ģ�⡣
        characterController.Move(verticality * Time.deltaTime);
    }
}
