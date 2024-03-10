using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [SerializeField] private float fireRate = 10f;
    [SerializeField] private float range = 20f;
    [SerializeField] private float impactForce = 150f;//�����
    private InputAction shootAction;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject impactEffected;

    [SerializeField] private Animator animator;
    //������
    private float nextTimeToShoot = 0;
    private void Start()
    {
        //�����������
        shootAction = new InputAction("Shoot", binding: "<Gamepad>/x");
        shootAction.AddBinding("<Mouse>/leftButton");
        shootAction.Enable();
    }
    private void Update()
    {
        //����������:������������Ӱ�죬�������»��ˣ����˱��䵹��������Ч
        bool isShooting = Mathf.Approximately(shootAction.ReadValue<float>(), 1f);
        animator.SetBool("isShooting", isShooting);
        if (isShooting && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;
            Fire();
        }

    }
    void Fire()
    {
        //ʹ�����߼���Ƿ��������Ϸ����
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range))
        {
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);//�� hit �ķ��߷�����ʩ��˲ʱ��
            }
            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject impactEffect = Instantiate(impactEffected, hit.point, impactRotation);
            impactEffect.transform.parent = hit.transform;
        }
        muzzleFlash.Play();

    }
}
