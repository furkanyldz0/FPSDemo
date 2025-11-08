using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float animationSpeed;

    private void Start() {
        animator.SetFloat("SpeedMultiplier", animationSpeed);
    }

    public void PlayRecoilAnimation() {
        animator.SetTrigger("FireTrigger");
    }

    public void PlayReloadAnimation() {
        animator.SetTrigger("ReloadTrigger");
    }
}
