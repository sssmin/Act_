using UnityEngine;

public class Poolable : MonoBehaviour
{
    public bool IsUsing { get; set; }
    [SerializeField] public int poolingNum;
}
