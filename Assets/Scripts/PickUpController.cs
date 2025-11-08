using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour //bu script sadece weaponholder nesnesine atalý
{
    private GunController currentGun;
    private Player player;
    void Start()
    {
        player = Player.Instance;

        GunController[] guns = FindObjectsByType<GunController>(FindObjectsSortMode.None);
        foreach(GunController g in guns) {
            DisableGun(g);
        }

        currentGun = Player.Instance.GetCurrentGun();
        EnableGun(currentGun);
    }


    public void PickUpGun(GunController gun) {
        DropCurrentGun();
        currentGun = gun;
        currentGun.transform.SetParent(transform);
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        currentGun.transform.localScale = Vector3.one; //tekrar parente atýnca scale'i kendi kendine deðiþiyor
        EnableGun(currentGun);
    }

    public void DropCurrentGun() {
        if(currentGun != null) {
            DisableGun(currentGun);
            currentGun.transform.SetParent(null);

            var currentGunRb = currentGun.GetComponent<Rigidbody>();
            float dropForwardForce = 2f;
            float dropUpwardForce = 2f;

            currentGunRb.velocity = player.GetComponent<CharacterController>().velocity;

            currentGunRb.AddForce(player.transform.forward * dropForwardForce, ForceMode.Impulse);
            currentGunRb.AddForce(player.transform.up * dropUpwardForce, ForceMode.Impulse);

            float random = UnityEngine.Random.Range(-1f, 1f);
            currentGunRb.AddTorque(new Vector3(random, random, random) * 10);

            currentGunRb = null;
            currentGun = null; //havadayken de g'ye basabiliyor null olmazsa
        }
    }

    private void DisableGun(GunController gun) {
        gun.enabled = false;
        gun.GetComponent<GunAnimator>().enabled = false; //animator scriptini kapatýyoruz direkt
        gun.GetComponent<Animator>().enabled = false;
        gun.GetComponent<CapsuleCollider>().enabled = true;
        gun.GetComponent<Rigidbody>().isKinematic = false;
        gun.GetComponent<Rigidbody>().useGravity = true;
    }
    private void EnableGun(GunController gun) {
        gun.enabled = true;
        gun.GetComponent<GunAnimator>().enabled = true; //animator scriptini kapatýyoruz direkt
        gun.GetComponent<Animator>().enabled = true;
        gun.GetComponent<CapsuleCollider>().enabled = false;
        gun.GetComponent<Rigidbody>().isKinematic = true;
        gun.GetComponent<Rigidbody>().useGravity = false;
    }
}
