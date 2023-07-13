

public class WindHashashinAnimTrigger : MonsterAnimTrigger
{
    public void Teleport()
    {
        ((AIController_WindHashashin)Monster.AIController).Teleport();
    }
}
