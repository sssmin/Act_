
using UnityEngine;

public class BossWallTrigger : MonoBehaviour
{
    private BoxCollider2D BoxCollider2D { get; set; }

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Player>())
        {
            GI.Inst.DungeonManager.BossMapWall.SetActive(true);
        }
    }
}
