using UnityEngine;

public class ParticlePrefab : MonoBehaviour
{
    private ParticleSystem ParticleSystem { get; set; }

    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!ParticleSystem.IsAlive())
        {
            GI.Inst.ResourceManager.Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        ParticleSystem.Play();
    }
}
