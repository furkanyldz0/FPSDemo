using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedGunVisual : MonoBehaviour
{
    //bu script selected nesnesine atalý
    [SerializeField] private Gun SelectedGun;
    [SerializeField] private GameObject VisualGameObject;
    [SerializeField] private GameObject SelectedGameObject;

    void Start()
    {
        Player.Instance.OnSelectedGunChanged += Instance_OnSelectedGunChanged;
    }

    //bu eventi SetSelectedGun çaðýrýyor
    private void Instance_OnSelectedGunChanged(object sender, Player.OnSelectedGunChangedEventArgs e) {
        if(e.selectedGun == SelectedGun) {
            Show();          
        }
        else {
            Hide();
        }
    }

    private void Show() {
        SelectedGameObject.SetActive(true);
        VisualGameObject.SetActive(false);
    }
    private void Hide() {
        SelectedGameObject.SetActive(false);
        VisualGameObject.SetActive(true);
    }
}
