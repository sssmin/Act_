using Cinemachine;
using UnityEngine;

public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup CinemachineTargetGroup { get; set; }
    private Transform cursorTransform;
    private Transform CameraFocus { get; set; }
    private Transform OnlyFocus { get; set; }

    private void Awake()
    {
        CinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        cursorTransform = GameObject.FindWithTag("Cursor").transform;
    }

    public void Init()
    {
        CameraFocus = GI.Inst.Player.transform.Find("CameraFocus");
        ActivateCamera();
    }

    private void Update()
    {
        cursorTransform.position = Util.GetMouseWorldPos();
    }

    public void ActivateCamera()
    {
        CinemachineTargetGroup.AddMember(CameraFocus, 1f, 2.5f);
        CinemachineTargetGroup.AddMember(cursorTransform, 0.5f, 1f);
    }

    public void DeactivateCamera()
    {
        CinemachineTargetGroup.RemoveMember(CameraFocus);
        CinemachineTargetGroup.RemoveMember(cursorTransform);
    }

    public void FocusOnlyThisObj(Transform objTransform)
    {
        DeactivateCamera();
        OnlyFocus = objTransform;
        CinemachineTargetGroup.AddMember(OnlyFocus, 1f, 2.5f);
    }

    public void RemoveOneFocus()
    {
        CinemachineTargetGroup.RemoveMember(OnlyFocus);
    }
}
