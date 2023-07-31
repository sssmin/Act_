using UnityEngine;

public class ThornTrapCollider : MonoBehaviour
{
    private BoxCollider2D BoxCollider2D { get; set; }

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
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
