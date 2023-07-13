using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MainCamera : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera { get; set; }
    private Camera Camera { get; set; }
    
    private void Awake()
    {
        CinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        Camera = GetComponentInChildren<Camera>();
    }

    public void Init(Transform followTransform, Camera mainUICamera)
    {
        CinemachineVirtualCamera.Follow = followTransform;
        var cameraData = Camera.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(mainUICamera);
    }
}
