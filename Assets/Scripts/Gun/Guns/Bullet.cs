using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float life = 1.5f;
    private Vector3 mouseWorldPosition;

    private void Awake() {
        Destroy(gameObject, life); //3sn sonunda yok olur
        rb = GetComponent<Rigidbody>();
    }

    public void InitiateBullet(float bulletSpeed, Vector3 moveDir, Vector3 mouseWorldPosition) {
        this.mouseWorldPosition = mouseWorldPosition;
        rb.velocity = moveDir * bulletSpeed;
    }




}
