using System.Collections;
using UnityEngine;

public class NecromancerLightning : MonoBehaviour
{
    private AIController AIController { get; set; }
    private StatManager PlayerStatManager { get; set; }

    public void Init(AIController aiController)
    {
        AIController = aiController;
        transform.position = AIController.Target.transform.position;
        StartCoroutine(CoDestroyMyself(3f));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
        {
            PlayerStatManager = col.GetComponent<StatManager>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            PlayerStatManager = null;
        }
    }

    public void ApplyDamageNotify()
    {
        if (PlayerStatManager)
        {
            AIController.ControlledMonster.StatManager.CauseNormalAttack(PlayerStatManager, 2f);
        }
    }

    IEnumerator CoDestroyMyself(float second)
    {
        yield return new WaitForSeconds(second);
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
