using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup CinemachineTargetGroup { get; set; }
    [SerializeField] Transform cursorTransform;

    private void Awake()
    {
        CinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    void Start()
    {
    }

    public void Init()
    {
        CinemachineTargetGroup.Target targetPlayer = new CinemachineTargetGroup.Target
            { weight = 1f, radius = 2.5f, target = GI.Inst.Player.transform.Find("CameraFocus") };
        
        CinemachineTargetGroup.Target targetCursor = new CinemachineTargetGroup.Target
            { weight = 0.5f, radius = 1f, target = cursorTransform };

        CinemachineTargetGroup.Target[] targets = new CinemachineTargetGroup.Target[] { targetPlayer, targetCursor };
        CinemachineTargetGroup.m_Targets = targets;

    }

    private void Update()
    {
        cursorTransform.position = Util.GetMouseWorldPos();
    }
}
