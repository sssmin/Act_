using UnityEngine;

public class UI_Option_DisplayContent : MonoBehaviour
{
    
    private void OnDisable()
    {
        GI.Inst.SaveDisplayData();
    }
}
