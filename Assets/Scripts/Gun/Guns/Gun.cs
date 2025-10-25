using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public Transform bulletSpawnPoint; //tam konumundan çaðýrmak için
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float bulletSpeed = 70f;
    [SerializeField] private GameInput gameInput;
    public bool IsFiring { get; private set; }
    public bool IsReloading { get; private set; }

    private Vector3 mouseWorldPosition;
    private float rateOfFire = 0.05f;
    private float shootCounter;

    private float defaultReloadTime = 2.2f; //animasyonun süresi, bunu doðrudan almak lazým
    private float reloadTimeCounter; // (clip'e event ekleyerek sayacý da kaldýrabiliyormuþuz ama ileride yapýcam

    private void Start() {
        gameInput.OnReloadAction += GameInput_OnReloadAction;
    }

    private void GameInput_OnReloadAction(object sender, System.EventArgs e) {
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
