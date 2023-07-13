

public class NightBorne : Monster
{
    protected override void Start()
    {
        base.Start();
        
        AIController.NormalAttackRange = 2.5f;
    }
}
