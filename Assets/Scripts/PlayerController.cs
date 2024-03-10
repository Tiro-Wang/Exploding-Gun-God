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
    //利用射线检测是否在地面上
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject groundCheck;
    private float groundCheckDistance = 0.3f;
    private bool isGrounded = false;

    [SerializeField] private Animator animator;

    //使用新输入管理系统
    private InputAction moveAction;
    private InputAction jumpAction;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        //将mouse锁定在游戏界面
        //Cursor.lockState = CursorLockMode.Locked;

        jumpAction = new InputAction("Jump", binding: "<Gamepad>/a");
        jumpAction.AddBinding("<keyboard>/space");
        jumpAction.Enable();
        moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("Dpad")//添加一组控制键
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
        /*        //控制玩家移动
                float moveX = Input.GetAxis("Horizontal");
                float moveY = Input.GetAxis("Vertical");*/
        //新
        float moveX = moveAction.ReadValue<Vector2>().x;
        float moveY = moveAction.ReadValue<Vector2>().y;
        move = transform.right * moveX + transform.forward * moveY;
        characterController.Move(move * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed",Mathf.Abs(moveY)+Mathf.Abs(moveX));//取绝对值
        /*        //鼠标控制旋转
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                transform.Rotate(Vector3.up * mouseX);*/
        //新
        float mouseX = 0;
        float mouseY = 0;
        if (Mouse.current != null)//判断当前使用电脑的鼠标操作
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
        //以x轴令Player头部相机转动
        //这种方式每次只对当前的旋转角度做一个微小的增量更改，而不是一次性做出大幅度的旋转，这样的做法有利于让旋转过程更加流畅和平稳。
        xRotate -= mouseY * Time.deltaTime;
        //限制旋转角度,math=>int  mathf=>float
        xRotate = Mathf.Clamp(xRotate, -80, 60);
        //playerHead.transform.Rotate(Vector3.right * xRotate);会一直不停地旋转
        //是相对于自身父对象的局部旋转。即使父对象有所旋转，设置localRotation只会改变该Transform对象在父对象坐标系下的旋转
        //设置了localRotation属性，即将playerHead的局部旋转一次性设置为由xRotate决定的新旋转,
        //这样一来，每次Update时都是重新设置旋转角度，而不是累加旋转。
        playerHead.transform.localRotation = Quaternion.Euler(xRotate, 0, 0);

        // 检查角色是否触地
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckDistance, groundLayer);
        /*        if (isGrounded && verticality.y < 0)
                {
                    //会将垂直速度硬编码为-1。这意味着只要角色在地面上且垂直速度小于0，
                    //就将其下落速度固定为一个特定值（这里是-1）。这可能会改变角色下落时的物理行为，
                    //使其下落速度变得恒定，而不是随时间逐渐加快。不过没什么用在这里
                    verticality.y = -1f;
                }*/
        //控制玩家跳跃
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //新
        //if (jumpAction.ReadValue<float>()>0 && isGrounded)//float值会有不完全相等时候，因此相等会是代码不精确,
        if (Mathf.Approximately(jumpAction.ReadValue<float>(), 1f) && isGrounded)//使用数学函数的方法，判断两个值更接近
        {
            //跳跃力（jumpForce）通常是一个瞬间施加的初始力量，
            //用于使角色立刻离开地面开始上升。不像重力那样是每帧持续累积的，跳跃力只在按键按下那一帧生效。
            verticality.y = jumpForce;
        }
        else
        {
            //在累积重力影响时，确保每一帧的重力加速度是恒定的，不受帧率影响。
            //+=实现角色逐渐加速下落的过程。
            verticality.y += gravity * Time.deltaTime;
        }
        //在执行实际移动时，将速度向量转换为每帧的实际位移，同样保证了移动速度与帧率无关，实现平滑的物理运动模拟。
        characterController.Move(verticality * Time.deltaTime);
    }
}
