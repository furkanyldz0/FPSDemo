using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CinemachineCameraLook : MonoBehaviour
{
    //[SerializeField] private Player player; player objesi üzerinde olduðu için gerek yok
    [SerializeField] private GameObject cinemachineCameraTarget; //yukarý aþaðý bakma açýsý
    private float cinemachineTargetPitch; //PlayerCameraRoot
    private float topClamp = 90f; //yukarý max açý
    private float bottomClamp = -90f; //aþaðý max açý

    private Vector3 aimPosition; //
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked; // kilidi kaldýrmak için CursorLockMode.None;
    }

    private void LateUpdate() {
        RotateCamera();
    }

    private void RotateCamera() {
        //float rotationSpeed = 5f;
        float mouseX = Player.Instance.gameInput.GetMouseDelta().x;
        float mouseY = Player.Instance.gameInput.GetMouseDelta().y;
        float mouseSensivity = 0.25f;

        //cinemachineTargetPitch += rotationSpeed * Input.GetAxisRaw("Mouse Y");
        cinemachineTargetPitch += mouseY * mouseSensivity;

        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        // pitch Yunuslama demekmiþ, yukarý aþaðý yöne bakmayý ifade eder
        cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(-cinemachineTargetPitch, 0f, 0f);

        //sadece sað ve sola döndürür, yukarý +1 ile çarparak y ekseni etrafýnda dönmesini saðlýyoruz 
        //transform.Rotate(Vector3.up * rotationSpeed * Input.GetAxisRaw("Mouse X"));
        transform.Rotate(Vector3.up * mouseX * mouseSensivity);
        //mevcut açýnýn üstüne ekler
    }

    private float ClampAngle(float angle, float minAngle, float maxAngle) {
        angle %= 360;

        return Mathf.Clamp(angle, minAngle, maxAngle);
    }

}
//private static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
//    // if'ler yerine bunu kullanabilirsiniz:
//    //lfAngle %= 360f;

//    //açýnýn devasa boyutlara ulaþmasýný engellemek için, yapýlmazsa float'ýn sýnýrlarýný aþabilir 
//    if (lfAngle < -360f) lfAngle += 360f;
//    if (lfAngle > 360f) lfAngle -= 360f;
//    return Mathf.Clamp(lfAngle, lfMin, lfMax);
//}