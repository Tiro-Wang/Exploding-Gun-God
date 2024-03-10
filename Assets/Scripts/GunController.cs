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
    [SerializeField] private float impactForce = 150f;//冲击力
    private InputAction shootAction;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject impactEffected;

    [SerializeField] private Animator animator;
    //射击间隔
    private float nextTimeToShoot = 0;
    //配置弹药
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private int magazineAmmo = 30;//弹夹

    [SerializeField] private TextMeshProUGUI ammoInfoText;
    private bool isReload = false;
    private void Start()
    {
        //配置射击动作
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

        //完成射击操作:对物体造成射击影响，如物体呗击退，敌人被射倒，粒子特效
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
        //使用射线检测是否射击到游戏对象
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range))
        {
            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);//在 hit 的法线方向上施加瞬时力
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
