

using UnityEngine;


public class MonsterStatManager : StatManager
{
    private UI_MonsterInfo monsterInfoUI;
    
    public override void Start()
    {
        base.Start();

        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_Main_MonsterInfo", transform);
        monsterInfoUI = go.GetComponent<UI_MonsterInfo>();
        monsterInfoUI.InitPos();
    }

    public override void AddCurrentHp(float value)
    {
        base.AddCurrentHp(value);
        
        float ratio = Mathf.Round((stats.currentHp.Value / stats.maxHp.Value * 100f) * 10) * 0.1f; 
        monsterInfoUI.SetBar(ratio);
    }

    public void FlipMonsterInfoUI(BaseController.EDir dir)
    {
        monsterInfoUI.Flip(dir);
    }
}
