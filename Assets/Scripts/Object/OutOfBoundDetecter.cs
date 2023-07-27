using UnityEngine;

public class OutOfBoundDetecter : MonoBehaviour
{
    private BoxCollider2D BoxCollider2D { get; set; }

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerStatManager playerStatManager = col.GetComponent<PlayerStatManager>();
        if (playerStatManager)
        {
            playerStatManager.OutOfBoundDead();
        }
    }
}

