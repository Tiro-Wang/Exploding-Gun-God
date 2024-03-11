using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField] private float fireRate = 10f;
    [SerializeField] private float range = 20f;
    [SerializeField] private float impactForce = 150f;//�����
    private InputAction shootAction;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject impactEffected;

    [SerializeField] public Animator animator;
    //������
    private float nextTimeToShoot = 0;
    //���õ�ҩ
    [SerializeField] public int currentAmmo;
    [SerializeField] public int maxAmmo = 10;
    [SerializeField] public int magazineAmmo = 30;//����

    [SerializeField] public TextMeshProUGUI ammoInfoText;
    public bool isReload = false;

    [SerializeField] private WeaponsSwitching weaponsSwitching;
    private void Start()
    {
        //�����������
        shootAction = new InputAction("Shoot", binding: "<Gamepad>/x");
        shootAction.AddBinding("<Mouse>/leftButton");
        shootAction.Enable();
    }
    private void OnEnable()
    {
        isReload = false;
        animator.SetBool("isReload", false);
    }
    private void Update()
    {

        ammoInfoText.text = currentAmmo + "/" + magazineAmmo;
        if (currentAmmo == 0 && magazineAmmo == 0)
        {
            animator.SetBool("isShooting", false);
            return;
        }
        if (isReload==true)
        {
            animator.SetBool("isShooting", false);

            return;
        }
        //����������:������������Ӱ�죬�������»��ˣ����˱��䵹��������Ч
        bool isShooting = Mathf.Approximately(shootAction.ReadValue<float>(), 1f);
        animator.SetBool("isShooting", isShooting);
        if (isShooting && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;
            Fire();
        }
        //
        if (currentAmmo == 0 && magazineAmmo > 0 && !isReload)
        {
            StartCoroutine(Reload());
        }
    }
    void Fire()
    {
        muzzleFlash.Play();
        currentAmmo--;
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
    }
    public IEnumerator Reload()
    {
        animator.SetBool("isReload", true);
        isReload = true;
        yield return new WaitForSeconds(1f);
        animator.SetBool("isReload", false);
        // ��鵯��ʣ���ӵ����Ƿ���ڵ�������ӵ���
        if (magazineAmmo >= maxAmmo)
        {
            // ����ǰ�ӵ�����Ϊ����ӵ���
            currentAmmo = maxAmmo;
            // ��ȥ���ص��ӵ���
            magazineAmmo -= maxAmmo;
        }
        else
        {
            // ����ǰ�ӵ�����Ϊ����ʣ���ӵ���
            currentAmmo = magazineAmmo;
            // ��յ���ʣ���ӵ���
            magazineAmmo = 0;
        }
        isReload = false;

    }
}
