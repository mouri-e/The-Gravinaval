using Unity.Cinemachine;
using UnityEngine;

public class CameraSnapToSide : MonoBehaviour
{
    public CinemachineVirtualCamera levelCamera;
    float currentCameraRotation;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            currentCameraRotation = levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().HorizontalAxis.Value;
            //front wall snap
            if (currentCameraRotation >= -45.5f && currentCameraRotation < 45.5f)
            {
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().HorizontalAxis.Value = Mathf.Lerp(currentCameraRotation, 0f, Time.deltaTime * 300);
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().VerticalAxis.Value = 0;
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().RadialAxis.Value = 1;
            }
            //left wall snap
            else if (currentCameraRotation >= 45.5f && currentCameraRotation < 135.5f)
            {
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().HorizontalAxis.Value = Mathf.Lerp(currentCameraRotation, 90f, Time.deltaTime * 300);
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().VerticalAxis.Value = 0;
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().RadialAxis.Value = 1;
            }
            //back wall snap
            else if (currentCameraRotation >= 135.5f && currentCameraRotation <= 180f ||
                    currentCameraRotation >= -180.5f && currentCameraRotation < -134.5f)
            {
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().HorizontalAxis.Value = Mathf.Lerp(currentCameraRotation, -180f, Time.deltaTime * 300);
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().VerticalAxis.Value = 0;
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().RadialAxis.Value = 1;
            }

            //right wall snap
            else if (currentCameraRotation >= -134.5f && currentCameraRotation < -45.5f)
            {
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().HorizontalAxis.Value = Mathf.Lerp(currentCameraRotation, -90f, Time.deltaTime * 300);
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().VerticalAxis.Value = 0;
                levelCamera.GetCinemachineComponent<CinemachineOrbitalFollow>().RadialAxis.Value = 1;
            }

        }
    }
}