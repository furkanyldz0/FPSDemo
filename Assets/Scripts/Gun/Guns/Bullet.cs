using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //private float bulletSpeed;
    //private Vector3 moveDir;
    private Rigidbody rb;
    [SerializeField] float life = 1.5f;
    private Vector3 mouseWorldPosition;

    private void Awake() {
        Destroy(gameObject, life); //3sn sonunda yok olur
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //transform.position += moveDir * bulletSpeed * Time.deltaTime;
    }

    public void InitiateBullet(float bulletSpeed, Vector3 moveDir, Vector3 mouseWorldPosition) {
        this.mouseWorldPosition = mouseWorldPosition;
        rb.velocity = moveDir * bulletSpeed;
    }


    //private void OnTriggerEnter(Collider other) { mermi hýzlý hareket ettiði için collider düzgün çalýþmýyor
    //    if(other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
    //        //rigidbody.AddExplosionForce(1000f, mouseWorldPosition, 5f); //düzgün çalýþmýyor
    //    }
    //    Destroy(gameObject);
    //}



    //public void InitiateBullet(float bulletSpeed, Vector3 moveDir) {
    //    this.bulletSpeed = bulletSpeed;
    //    this.moveDir = moveDir;
    //}


}
