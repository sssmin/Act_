using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;
    private float xPos;
    void Start()
    {
        xPos = transform.position.x;
    }

    void Update()
    {
        float distToMove = GI.Inst.Player.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPos + distToMove, transform.position.y);
    }
}
