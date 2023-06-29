using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup CinemachineTargetGroup { get; set; }
    [SerializeField] Transform cursorTransform;
    private Transform CameraFoucs { get; set; }

    private void Awake()
    {
        CinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    void Start()
    {
    }

    public void Init()
    {
        CameraFoucs = GI.Inst.Player.transform.Find("CameraFocus");
        
        ActivateCamera();
    }

    private void Update()
    {
        cursorTransform.position = Util.GetMouseWorldPos();
    }

    public void ActivateCamera()
    {
        CinemachineTargetGroup.AddMember(CameraFoucs, 1f, 2.5f);
        CinemachineTargetGroup.AddMember(cursorTransform, 0.5f, 1f);
    }

    public void DeactivateCamera()
    {
        CinemachineTargetGroup.RemoveMember(CameraFoucs);
        CinemachineTargetGroup.RemoveMember(cursorTransform);
    }
}
