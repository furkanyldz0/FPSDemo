using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bulletSpeed;
    private Vector3 moveDir;
    [SerializeField] float life = 3;

    private void Awake() {
        Destroy(gameObject, life); //3sn sonunda yok olur
    }

    void Update()
    {
        transform.position += moveDir * bulletSpeed * Time.deltaTime;
    }

    public void InitiateBullet(float bulletSpeed, Vector3 moveDir) {
        this.bulletSpeed = bulletSpeed;
        this.moveDir = moveDir;
    }

    //private void OnCollisionEnter(Collision collision) { ihtiyaca göre
        
    //}
}
