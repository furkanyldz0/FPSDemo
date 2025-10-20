using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public Transform bulletSpawnPoint; //tam konumundan çaðýrmak için
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




//// --- YENÝ KOMUT METOTLARI ---
//public void StartShooting() {  //gameinput'dan event oluþturup player'dan bu fonksiyonlarý çaðýrabiliriz
//    isFiring = true;           //Bu yapýda Player'ýn Update'i, her kare gun.UpdateAimPosition(mouseWorldPosition)'ý çaðýrýr ama Start/StopShooting sadece tuþa basýlýp býrakýldýðýnda bir kez çaðrýlýr.
                                    //diyor gemini
//}

//public void StopShooting() {
//    isFiring = false;
//}

//// Niþan alma pozisyonunu güncellemek için ayrý bir metot
//public void UpdateAimPosition(Vector3 newPosition) {
//    aimTargetPosition = newPosition;
//}
