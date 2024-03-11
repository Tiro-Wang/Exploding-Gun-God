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

    [SerializeField] public Animator animator;
    //射击间隔
    private float nextTimeToShoot = 0;
    //配置弹药
    [SerializeField] public int currentAmmo;
    [SerializeField] public int maxAmmo = 10;
    [SerializeField] public int magazineAmmo = 30;//弹夹

    [SerializeField] public TextMeshProUGUI ammoInfoText;
    public bool isReload = false;

    [SerializeField] private WeaponsSwitching weaponsSwitching;
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
    public IEnumerator Reload()
    {
        animator.SetBool("isReload", true);
        isReload = true;
        yield return new WaitForSeconds(1f);
        animator.SetBool("isReload", false);
        // 检查弹夹剩余子弹数是否大于等于最大子弹数
        if (magazineAmmo >= maxAmmo)
        {
            // 将当前子弹数设为最大子弹数
            currentAmmo = maxAmmo;
            // 减去满载的子弹数
            magazineAmmo -= maxAmmo;
        }
        else
        {
            // 将当前子弹数设为弹夹剩余子弹数
            currentAmmo = magazineAmmo;
            // 清空弹夹剩余子弹数
            magazineAmmo = 0;
        }
        isReload = false;

    }
}
