using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CinemachineCameraLook : MonoBehaviour
{
    //[SerializeField] private Player player; player objesi �zerinde oldu�u i�in gerek yok
    [SerializeField] private GameObject cinemachineCameraTarget; //yukar� a�a�� bakma a��s�
    private float cinemachineTargetPitch; //PlayerCameraRoot
    private float topClamp = 90f; //yukar� max a��
    private float bottomClamp = -90f; //a�a�� max a��

    private Vector3 aimPosition; //
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked; // kilidi kald�rmak i�in CursorLockMode.None;
    }

    private void LateUpdate() {
        RotateCamera();
    }

    private void RotateCamera() {
        float rotationSpeed = 5f;

        cinemachineTargetPitch += rotationSpeed * Input.GetAxisRaw("Mouse Y");

        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        // pitch Yunuslama demekmi�, yukar� a�a�� y�ne bakmay� ifade eder
        cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(-cinemachineTargetPitch, 0f, 0f);

        //sadece sa� ve sola d�nd�r�r, yukar� +1 ile �arparak y ekseni etraf�nda d�nmesini sa�l�yoruz 
        transform.Rotate(Vector3.up * rotationSpeed * Input.GetAxisRaw("Mouse X"));
        //mevcut a��n�n �st�ne ekler
    }

    private float ClampAngle(float angle, float minAngle, float maxAngle) {
        angle %= 360;

        return Mathf.Clamp(angle, minAngle, maxAngle);
    }

}
//private static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
//    // if'ler yerine bunu kullanabilirsiniz:
//    //lfAngle %= 360f;

//    //a��n�n devasa boyutlara ula�mas�n� engellemek i�in, yap�lmazsa float'�n s�n�rlar�n� a�abilir 
//    if (lfAngle < -360f) lfAngle += 360f;
//    if (lfAngle > 360f) lfAngle -= 360f;
//    return Mathf.Clamp(lfAngle, lfMin, lfMax);
//}