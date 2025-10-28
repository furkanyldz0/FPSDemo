using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour //bu script sadece weaponholder nesnesine atalý
{
    private Gun currentGun;
    void Start()
    {
        Gun[] guns = FindObjectsByType<Gun>(FindObjectsSortMode.None);
        foreach(Gun g in guns) {
            DisableGun(g);
        }

        currentGun = Player.Instance.GetCurrentGun();
        EnableGun(currentGun);
    }


    public void PickUpGun(Gun gun) {
        DropCurrentGun();

        currentGun = gun;
        currentGun.transform.SetParent(transform);
        currentGun.transform.localPosition = Vector3.zero;
        currentGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
        currentGun.transform.localScale = Vector3.one;
        EnableGun(currentGun);
    }

    private void DropCurrentGun() {
        DisableGun(currentGun);
        currentGun.transform.SetParent(null);
        currentGun.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentGun.GetComponent<Rigidbody>().AddExplosionForce(5f ,transform.forward,1f);
        
    }

    private void DisableGun(Gun gun) {
        gun.enabled = false;
        gun.GetComponent<GunAnimator>().enabled = false; //animator scriptini kapatýyoruz direkt
        gun.GetComponent<Animator>().enabled = false;
        gun.GetComponent<CapsuleCollider>().enabled = true;
        gun.GetComponent<Rigidbody>().isKinematic = false;
        gun.GetComponent<Rigidbody>().useGravity = true;
    }
    private void EnableGun(Gun gun) {
        gun.enabled = true;
        gun.GetComponent<GunAnimator>().enabled = true; //animator scriptini kapatýyoruz direkt
        //gun.GetComponent<Animator>().enabled = true;
        gun.GetComponent<CapsuleCollider>().enabled = false;
        gun.GetComponent<Rigidbody>().isKinematic = true;
        gun.GetComponent<Rigidbody>().useGravity = false;
    }
}
