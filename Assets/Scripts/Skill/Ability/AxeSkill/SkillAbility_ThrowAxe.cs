using System.Collections;
using UnityEngine;

public class SkillAbility_ThrowAxe : MonoBehaviour
{
    private PlayerController OwnerController { get; set; }
    private StatManager OwnerStatManager { get; set; }    
    private Rigidbody2D Rb { get; set; }
    private Animator Animator { get; set; }
    private DamageInfo DamageInfo { get; set; }
    public float speed;
    private Coroutine CoDestroy;
    public float rotationSpeed;
    private float time;
    private float returnElapseTime;
    private bool bIsReturn;
    private bool bIsSpining;
    private float damageTimer;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;

        transform.eulerAngles -= new Vector3(0f, 0f, rotationAmount);
    }

    public void Init(Vector2 dir, PlayerController inOwner, StatManager statManager, DamageInfo damageInfo)
    {
        OwnerController = inOwner;
        OwnerStatManager = statManager;
        Rb.velocity = new Vector2(dir.x * speed, 0f);
        DamageInfo = damageInfo;
        bIsReturn = false;
        StartCoroutine(CoSpinInPlace());
        time = 0f;
        returnElapseTime = 0f;
        bIsSpining = false;
        damageTimer = 0.3f;
    }


    private void Update()
    {
        if (bIsReturn)
        {
            returnElapseTime += Time.deltaTime;
            if (Vector2.Distance(transform.position, OwnerController.ControlledPlayer.arrowSpawnPoint.transform.position) >= 1f)
            {
                if (returnElapseTime > 1f)
                {
                    transform.position = Vector2.Lerp(transform.position, OwnerController.ControlledPlayer.arrowSpawnPoint.transform.position, time += Time.deltaTime / 5);
                }
                else
                {
                    transform.position = Vector2.Lerp(transform.position, OwnerController.ControlledPlayer.arrowSpawnPoint.transform.position, Time.deltaTime);
                }
            }
            else
            {
                bIsReturn = false;
                GI.Inst.ResourceManager.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<StatManager>()?.TakeDamage(DamageInfo, OwnerStatManager, Define.EDamageType.Skill);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (bIsSpining)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer < 0f)
            {
                DamageInfo damageInfo = DamageInfo;
                damageInfo.damage = Mathf.RoundToInt(damageInfo.damage * 0.2f);
                other.GetComponent<StatManager>()?.TakeDamage(damageInfo, OwnerStatManager, Define.EDamageType.Skill);
                damageTimer = 0.3f;
            }
        }
    }
    
    IEnumerator CoSpinInPlace()
    {
        yield return new WaitForSeconds(.5f); //0.5초 동안 날아감
        bIsSpining = true;
        Rb.velocity = new Vector2(0f, 0f); //1.5초동안 머무름 => 0.3초 마다 대미지를 주면 5타 
        yield return new WaitForSeconds(2f); //2초 후에 복귀
        bIsReturn = true;
        bIsSpining = false;
    }
    
    

    
    
}
