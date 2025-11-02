using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimator : MonoBehaviour //script gun'lardan baðýmsýz aslýnda
{
    [SerializeField] private Animator animator;
    //[SerializeField] private Player player;
    [SerializeField] private float animationSpeed = 1.25f;
    [SerializeField] private Gun gun;

    private void Start() {
        animator.SetFloat("SpeedMultiplier", animationSpeed);
    }

    private void Update() {
        if (gun.IsShooting) {
            animator.SetBool("IsShooting", true);
        }
        else{
            animator.SetBool("IsShooting", false);
        }
        if (gun.IsReloading) {
            animator.SetBool("IsReloading", true);
        }
        else {
            animator.SetBool("IsReloading", false);
        }
    }
}
