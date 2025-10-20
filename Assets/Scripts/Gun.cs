using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public Transform bulletSpawnPoint; //tam konumundan �a��rmak i�in
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float bulletSpeed = 70f;

    private bool isFiring;
    private Vector3 mouseWorldPosition;
    private float rateOfFire = 0.05f;
    private float shootCounter;

    private void Update() {
        shootCounter -= Time.deltaTime;

        if (isFiring && shootCounter <= 0f) {
            Shoot();
            shootCounter = rateOfFire;
        }
    }

    public void Shoot() { 
        Vector3 aimDir = (mouseWorldPosition - bulletSpawnPoint.position).normalized;
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
        bullet.InitiateBullet(bulletSpeed, aimDir, mouseWorldPosition);
    }

    public void SetState(bool isFiring, Vector3 mouseWorldPosition) {
        this.isFiring = isFiring;
        this.mouseWorldPosition = mouseWorldPosition;
    }

    
}




//// --- YEN� KOMUT METOTLARI ---
//public void StartShooting() {  //gameinput'dan event olu�turup player'dan bu fonksiyonlar� �a��rabiliriz
//    isFiring = true;           //Bu yap�da Player'�n Update'i, her kare gun.UpdateAimPosition(mouseWorldPosition)'� �a��r�r ama Start/StopShooting sadece tu�a bas�l�p b�rak�ld���nda bir kez �a�r�l�r.
                                    //diyor gemini
//}

//public void StopShooting() {
//    isFiring = false;
//}

//// Ni�an alma pozisyonunu g�ncellemek i�in ayr� bir metot
//public void UpdateAimPosition(Vector3 newPosition) {
//    aimTargetPosition = newPosition;
//}
