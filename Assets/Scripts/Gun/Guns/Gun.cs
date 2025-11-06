using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] public Transform bulletSpawnPoint; //tam konumundan çaðýrmak için
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private ParticleSystem muzzleEffect;
    public bool IsReloading { get; private set; }
    public bool IsShooting { get; private set; }

    private Vector3 mouseWorldPosition;
    private float rateOfFire = 0.07f;
    private float shootCounter;

    private float defaultReloadTime = 2.4f; //animasyonun süresi, bunu doðrudan almak lazým
    private float reloadTimeCounter; // (clip'e event ekleyerek sayacý da kaldýrabiliyormuþuz ama ileride yapýcam

    [SerializeField] private Transform debugTransformObject;

    public void Reload() {
        if (!IsReloading) {
            IsReloading = true;
            reloadTimeCounter = defaultReloadTime;
        }
    }

    private void Update() {

        shootCounter -= Time.deltaTime;

        if (!IsReloading && (IsShooting && shootCounter <= 0f)) { //
            shootCounter = rateOfFire;
            Shoot();
            //Debug.Log("fiyuv");
        }
        else if (IsReloading) {
            reloadTimeCounter -= Time.deltaTime;
            if(reloadTimeCounter <= 0f) {
                IsReloading = false;
            }
        }
        
    }

    private void Shoot() { //raycasti update içinde yazsak daha mý iyi olur?
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f)) {
            debugTransformObject.position = raycastHit.point;
            CreateBulletHole(raycastHit);
            if(raycastHit.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.AddExplosionForce(500f, raycastHit.point, 5f);
            }
        }
        muzzleEffect.Play();
    }

    

    private void CreateBulletHole(RaycastHit raycastHit) { //bura cemileye emanet
        // 1. Z-Fighting (Titreþim) sorunlarýný önlemek için ofsetler
        float baseOffset = 0.001f;
        float randomOffset = Random.Range(0.000f, 0.002f);

        // Mermi deliðinin oluþacaðý pozisyon (duvardan çok az önde)
        var bulletHoleSpawnPoint = raycastHit.point + raycastHit.normal * (baseOffset + randomOffset);

        // 2. Rotasyon
        // Prefab'ýn "ileri" (Z) yönünün duvarýn "içine" bakmasýný saðlar.
        // Bu, mermi deliðini yüzeye düzgünce yapýþtýrýr.
        var bulletHoleRotation = Quaternion.LookRotation(raycastHit.normal);

        // 3. Mermi deliðini DÜNYADA (ebeveynsiz) oluþtur
        var bulletHole = Instantiate(bulletHolePrefab, bulletHoleSpawnPoint, bulletHoleRotation);

        // 4. Vurulan objenin transform'unu ve ölçeðini al
        Transform parentObject = raycastHit.transform;
        Vector3 parentScale = parentObject.lossyScale;

        // 5. Prefab'ýn kendi varsayýlan ölçeðini al
        Vector3 prefabScale = bulletHolePrefab.transform.localScale;

        // 6. Ebeveynin ölçeðinin simetrik (uniform) olup olmadýðýný kontrol et
        bool isUniformScale = Mathf.Approximately(parentScale.x, parentScale.y) &&
                              Mathf.Approximately(parentScale.y, parentScale.z);

        // 7. SENÝN ÝSTEDÝÐÝN MANTIK:
        // Eðer ölçek simetrikse (veya obje statikse), yapýþtýr.
        if (isUniformScale || parentObject.gameObject.isStatic) {
            // Mermi deliðini vurulan objenin çocuðu (child) yap
            bulletHole.transform.SetParent(parentObject);

            // Ölçeði tersine çevirerek düzelt.
            // (Ebeveynin ölçeði 5,5,5 ise, deliðin ölçeðini 1/5 = 0.2 yaparýz)
            float inverseScale = 1.0f / parentScale.x; // (x, y, z hepsi ayný olduðu için 'x'i kullanmak yeterli)
            bulletHole.transform.localScale = prefabScale * inverseScale;
        }
        // Eðer ölçek simetrik DEÐÝLSE (örn: 1,5,1),
        // "orantýsýz kaymayý" önlemek için hiçbir þey yapma.
        // Mermi deliði ebeveynsiz olarak (dünyada) kalýr.
        else {
            // Ebeveyn atama (SetParent yapma)
        }
        float life = 5f;
        Destroy(bulletHole, life); 
    }

    //private void CreateBulletHole(RaycastHit raycastHit) {
    //    float baseOffset = 0.001f;
    //    float randomOffset = Random.Range(0.000f, 0.002f);

    //    var bulletHoleSpawnPoint = raycastHit.point + raycastHit.normal * (baseOffset + randomOffset); //raycasthit.normal ýþýnýn çarptýðý yüzeyin dýþa bakan yönü
    //    var bulletHole = Instantiate(BulletHolePrefab, bulletHoleSpawnPoint, Quaternion.LookRotation(raycastHit.normal)); //visual x rotasyonu olmasý lazým

    //    //Destroy(currentBulletHole, life);
    //}
    public void SetState(bool isShooting) {
        IsShooting = isShooting;
    }
    //public void Shoot() { 
    //    Vector3 aimDir = (mouseWorldPosition - bulletSpawnPoint.position).normalized;
    //    var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
    //    bullet.InitiateBullet(bulletSpeed, aimDir, mouseWorldPosition);
    //}

    //public void SetState(bool isFiring, Vector3 mouseWorldPosition) {
    //    //this.isFiring = isFiring;
    //    this.mouseWorldPosition = mouseWorldPosition;
    //    IsFiring = isFiring;
    //}


    

}