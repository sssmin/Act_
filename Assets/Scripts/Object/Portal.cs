using UnityEngine;

public class Portal : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
        {
            GI.Inst.CinemachineTarget.DeactivateCamera();
            GI.Inst.UIManager.CreateDungeonInfoUI();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            GI.Inst.UIManager.DestroyDungeonSelectUI();
            GI.Inst.CinemachineTarget.ActivateCamera();
        }
    }
}
