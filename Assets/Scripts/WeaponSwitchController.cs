using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitchController : MonoBehaviour
{
    private float selectedWeapon = 0;
    private float lastWeapon;
    [SerializeField] private GunController[] weapons;
    private void Start()
    {
        UpdateWeaponList(); //weapons dizisine oyuncuda olan silahlarý geçer
        SelectWeapon();
    }

    

    private void Update() //reload ve ateþ ederken silah deðiþtirmemeli
    {
        lastWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) { //selectedWeapon++;selectedWeapon %= transform.childCount;
            if (selectedWeapon >= weapons.Length - 1) {
                selectedWeapon = 0;
            }
            else {
                selectedWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) {
            if (selectedWeapon <= 0) { //selectedweapon 0 olduðu zaman bu if'te hemen 0'ýn altýna inmiyor, ikinci if'te sýfýr oluyor o yüzden dahil etmen lazým
                selectedWeapon = weapons.Length - 1;
            }
            else {
                selectedWeapon--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length > 1) {
            selectedWeapon = 1;
        }
        if (lastWeapon != selectedWeapon) {
            SelectWeapon();
        }

    }
    private void SelectWeapon() {
        int i = 0;
        foreach (GunController weapon in weapons) {
            if (i == selectedWeapon) {
                ActivateWeapon(weapon);
            }
            else {
                DeactivateWeapon(weapon);
            }
            i++;
        }
    }

    public void UpdateWeaponList() {
        weapons = transform.GetComponentsInChildren<GunController>(includeInactive: true);
    }
    private void ActivateWeapon(GunController weapon) {
        weapon.gameObject.SetActive(true);
        Player.Instance.SetCurrentGun(weapon);
        weapon.GetComponent<GunAnimator>().enabled = true;
        Debug.Log(weapon.ToString());
    }
    private void DeactivateWeapon(GunController weapon) {
        weapon.gameObject.SetActive(false);
        weapon.GetComponent<GunAnimator>().enabled = false;
        
    }
}
