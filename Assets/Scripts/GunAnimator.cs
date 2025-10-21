using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameInput gameInput;

    private void Update() {
        if (gameInput.IsFiring()) {
            animator.SetBool("IsShooting", true);
        }
        else{
            animator.SetBool("IsShooting", false);
        }
    }
}
