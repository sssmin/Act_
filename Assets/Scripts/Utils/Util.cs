using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
		if (component == null)
            component = go.AddComponent<T>();
        return component;
	}

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        
        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
		}
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.x = Mathf.Clamp(mouseScreenPos.x, 0f, Screen.width);
        mouseScreenPos.y = Mathf.Clamp(mouseScreenPos.y, 0f, Screen.height);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        worldPos.z = 0f;

        return worldPos;
    }

    public static List<Transform> GetMonstersInScreen()
    {
        List<Transform> monsters = new List<Transform>();
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 cameraBotLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraPos.z));
        Vector3 cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraPos.z));

        Collider2D[] cols = Physics2D.OverlapAreaAll(cameraBotLeft, cameraTopRight, LayerMask.GetMask("Monster"));

        foreach (Collider2D col in cols)
        {
            monsters.Add(col.transform);
        }
        
        return monsters;
    }

    public static Vector3 GetCenterWorldPos()
    {
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0);
        Vector3 worldCenter = Camera.main.ViewportToWorldPoint(viewportCenter);
        worldCenter.z = 0f;
        return worldCenter;
    }

    public static void ConvertStatString(Stats stats, Define.EStatType statType, out string name, out string value)
    {
        name = "";
        value = "";
        switch (statType)
        {
            case Define.EStatType.Attack:
                name = "공격력";
                value = stats.attack.Value.ToString("#,0");
                break;
            case Define.EStatType.Defence:
                name = "방어력";
                value = stats.defence.Value.ToString("#,0");
                break;
            case Define.EStatType.ElemAttack:
                name = "속성 공격력";
                value = stats.elemAttack.Value.ToString("#,0");
                break;
            case Define.EStatType.MaxHp:
                name = "최대 체력";
                value = stats.maxHp.Value.ToString("#,0");
                break;
            case Define.EStatType.CriticalChancePer:
                name = "치명타 확률";
                value = stats.criticalChancePer.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.CriticalResistPer:
                name = "치명타 저항 확률";
                value = stats.criticalResistPer.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.CriticalDamageIncPer:
                name = "치명타 대미지 증가율";
                value = stats.criticalDamageIncPer.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.NormalAttackDamageIncPer:
                name = "기본 공격 대미지 증가율";
                value = stats.normalAttackDamageIncPer.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.SkillAttackDamageIncPer:
                name = "스킬 공격 대미지 증가율";
                value = stats.skillAttackDamageIncPer.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.EvasionChancePer:
                name = "회피 확률";
                value = stats.evasionChancePer.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.SkillCooltimeDecRate:
                name = "스킬 쿨타임 감소율";
                value = stats.skillCooltimeDecRate.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.MoveSpeed:
                name = "이동 속도";
                value = stats.moveSpeed.Value.ToString("#,0");
                break;
        }
    }
    
    public static void ConvertStatString(Stat stat, out string name, out string value)
    {
        name = "";
        value = "";
        switch (stat.statType)
        {
            case Define.EStatType.Attack:
                name = "공격력";
                value = stat.Value.ToString("#,0");
                break;
            case Define.EStatType.Defence:
                name = "방어력";
                value = stat.Value.ToString("#,0");
                break;
            case Define.EStatType.ElemAttack:
                name = "속성 공격력";
                value = stat.Value.ToString("#,0");
                break;
            case Define.EStatType.MaxHp:
                name = "최대 체력";
                value = stat.Value.ToString("#,0");
                break;
            case Define.EStatType.CriticalChancePer:
                name = "치명타 확률";
                value = stat.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.CriticalResistPer:
                name = "치명타 저항 확률";
                value = stat.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.CriticalDamageIncPer:
                name = "치명타 대미지 증가율";
                value = stat.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.NormalAttackDamageIncPer:
                name = "기본 공격 대미지 증가율";
                value = stat.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.SkillAttackDamageIncPer:
                name = "스킬 공격 대미지 증가율";
                value = stat.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.EvasionChancePer:
                name = "회피 확률";
                value = stat.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.SkillCooltimeDecRate:
                name = "스킬 쿨타임 감소율";
                value = stat.Value.ToString("#,0") + "(%)";
                break;
            case Define.EStatType.MoveSpeed:
                name = "이동 속도";
                value = stat.Value.ToString("#,0");
                break;
        }
    }

    public static string ConvertRarityString(Item.ERarity rarity)
    {
        string value = "";
        switch (rarity)
        {
            case Item.ERarity.Common:
                value = "최하급";
                break;
            case Item.ERarity.Uncommon:
                value = "하급";
                break;
            case Item.ERarity.Rare:
                value = "중급";
                break;
            case Item.ERarity.Epic:
                value = "상급";
                break;
            case Item.ERarity.Legendary:
                value = "최상급";
                break;
        }
        return value;
    }
    
    public static Color GetRarityColor(Item.ERarity rarity)
    {
        Color value = new Color();
        switch (rarity)
        {
            case Item.ERarity.Common:
                value = Color.grey;
                break;
            case Item.ERarity.Uncommon:
                value = new Color(0f, 241f/255f, 18f/255f, 255f/255f);
                break;
            case Item.ERarity.Rare:
                value = new Color(59f/255f, 95f/255f, 255f/255f, 255f/255f);
                break;
            case Item.ERarity.Epic:
                value = new Color(180f/255f, 59/255f, 255f/255f, 255f/255f);
                break;
            case Item.ERarity.Legendary:
                value = new Color(255f/255f, 191f/255f, 0f/255f, 255f/255f);
                break;
        }
        return value;
    }

    public static string ConvertItemCategoryString(Item.EItemCategory category)
    {
        string value = "";
        switch (category)
        {
            case Item.EItemCategory.Weapon:
                value = "무기";
                break;
            case Item.EItemCategory.Armor:
                value = "방어구";
                break;
            case Item.EItemCategory.Acc:
                value = "악세사리";
                break;
            case Item.EItemCategory.Consumable:
                value = "소모품";
                break;
            case Item.EItemCategory.Etc:
                value = "기타";
                break;
        }

        return value;
    }
    
    
    
    
    
}
