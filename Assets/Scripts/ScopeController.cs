using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScopeController : MonoBehaviour
{
    private InputAction scopeAction;
    private bool isScoped = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject scopeOverlay;
    [SerializeField] private Camera cam;
    private GunController gunController;
    // Start is called before the first frame update
    void Start()
    {
        scopeAction = new InputAction("Scope", binding: "<Mouse>/rightButton");
        scopeAction.AddBinding("<Gamepad>/y");
        scopeAction.Enable();
        gunController = GetComponent<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gunController.currentAmmo == 0)
        {
            OnUnScope();
        }
        if (scopeAction.triggered)
        {
            isScoped = !isScoped;
            if (isScoped)
            {
                StartCoroutine(OnScope());
            }
            else
            {
                OnUnScope();
            }

        }
    }

    IEnumerator OnScope()
    {
        animator.SetBool("isScoped", true);

        yield return new WaitForSeconds(0.2f);

        scopeOverlay.SetActive(true);
        cam.cullingMask = cam.cullingMask & ~(1 << 6);
        cam.fieldOfView = 45;
    }
    void OnUnScope()
    {
        animator.SetBool("isScoped", false);
        scopeOverlay.SetActive(false);
        cam.cullingMask = cam.cullingMask | (1 << 6);
        cam.fieldOfView = 60;

    }
}
