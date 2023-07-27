using UnityEngine;

public class OutOfBoundDetecter_Tutorial : MonoBehaviour
{
    private BoxCollider2D BoxCollider2D { get; set; }
    private GameObject playerStartPoint;
    
    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        //플레이어 원위치
        if (playerStartPoint == null)
            playerStartPoint = GameObject.Find("PlayerStartPoint");
        Player player = col.GetComponent<Player>();
        if (player)
        {
            player.transform.position = playerStartPoint.transform.position;
        }
    }
}
