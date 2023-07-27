using System.Collections;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    private BoxCollider2D BoxCollider2D { get; set; }
    private Animator Animator { get; set; }
    [SerializeField] private float delayTime;

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Animator.SetBool(AnimHash.isIdle, true);
        StartCoroutine(CoRunTrap());
    }

    IEnumerator CoRunTrap()
    {
        yield return new WaitForSeconds(delayTime);
        Animator.SetBool(AnimHash.isRun, true);
    }

    public void RunCompleteNotify()
    {
        Animator.SetBool(AnimHash.isRun, false);
        StartCoroutine(CoRunTrap());
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        StatManager statManager = col.GetComponent<StatManager>();
        if (statManager)
        {
            statManager.TakeTrapDamage();
        }
    }
}
