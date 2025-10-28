using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public Transform bulletSpawnPoint; //tam konumundan �a��rmak i�in
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float bulletSpeed = 70f;
    //[SerializeField] private GameInput gameInput; sihahlara gameinput nesnesini ge�memelisin, gameinput scripti birden fazla kez �al��abilir
    public bool IsFiring { get; private set; }
    public bool IsReloading { get; private set; }

    private Vector3 mouseWorldPosition;
    private float rateOfFire = 0.05f;
    private float shootCounter;

    private float defaultReloadTime = 2.4f; //animasyonun s�resi, bunu do�rudan almak laz�m
    private float reloadTimeCounter; // (clip'e event ekleyerek sayac� da kald�rabiliyormu�uz ama ileride yap�cam

    public void Reload() {
        if (!IsReloading) {
            IsReloading = true;
            reloadTimeCounter = defaultReloadTime;
        }
    }

    private void Update() {
        shootCounter -= Time.deltaTime;

        if (!IsReloading && (IsFiring && shootCounter <= 0f)) { //
            Shoot();
            shootCounter = rateOfFire;
        }
        else if (IsReloading) {
            reloadTimeCounter -= Time.deltaTime;
            if(reloadTimeCounter <= 0f) {
                IsReloading = false;
            }
        }
    }
    public void Shoot() { 
        Vector3 aimDir = (mouseWorldPosition - bulletSpawnPoint.position).normalized;
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
        bullet.InitiateBullet(bulletSpeed, aimDir, mouseWorldPosition);
    }

    public void SetState(bool isFiring, Vector3 mouseWorldPosition) {
        //this.isFiring = isFiring;
        this.mouseWorldPosition = mouseWorldPosition;
        IsFiring = isFiring;
    }
    
    public void Interact() {
        Debug.Log("Interact!");
    }

}