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

    [SerializeField] private Animator animator;
    //������
    private float nextTimeToShoot = 0;
    //���õ�ҩ
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private int magazineAmmo = 30;//����

    [SerializeField] private TextMeshProUGUI ammoInfoText;
    private bool isReload = false;
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
        if (currentAmmo == 0)
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
    IEnumerator Reload()
    {
        animator.SetBool("isReload", true);
        isReload = true;
        yield return new WaitForSeconds(1f);
        animator.SetBool("isReload", false);

        if (magazineAmmo > 0)
        {
            magazineAmmo -= maxAmmo;
            currentAmmo = maxAmmo;
        }
        isReload = false;
    }
}
