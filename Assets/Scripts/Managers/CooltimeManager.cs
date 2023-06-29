using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CooltimeManager : MonoBehaviour
{
    #region Active

    public float FirstSkillTimer { get; set; }
    public float SecondSkillTimer { get; set; }
    public float ThirdSkillTimer { get; set; }
    public float FifthSkillTimer { get; set; }

    #endregion

    #region Item

    //아래 딕셔너리는 FillAmount 계산에 필요, value = 쿨타임
    private Dictionary<string, float> ItemCooltimeDict { get; set; } = new Dictionary<string, float>();
    private Dictionary<string, float> ItemIconFillAmount { get; set; } = new Dictionary<string, float>();

    #endregion

    #region Passive

    private Dictionary<Define.ESkillId, float> PassiveCooltimeDict { get; set; } = new Dictionary<Define.ESkillId, float>();
    private Dictionary<Define.ESkillId, float> PassiveIconFillAmount { get; set; } = new Dictionary<Define.ESkillId, float>();

    #endregion
   
    
    

    private void Update()
    {
        CheckItemCooltime();
        CheckPassiveCooltime();
    }

    #region Item

    private void CheckItemCooltime()
    {
        if (ItemCooltimeDict.Count > 0)
        {
            foreach (KeyValuePair<string, float> pair in ItemIconFillAmount.ToList())
            {
                if (ItemCooltimeDict.ContainsKey(pair.Key))
                {
                    float cooltime = ItemCooltimeDict[pair.Key];

                    ItemIconFillAmount[pair.Key] -= (1 / cooltime * Time.deltaTime);
                    //Debug.Log(iconFillAmount[pair.Key]);
                }
            }
        }
    }
    
    public void SetItemCooltime(string itemId, float cooltime)
    {
        StartCoroutine(CoSetItemCooltime(itemId, cooltime));
    }
    
    public bool IsReadyItem(string itemId)
    {
        if (ItemCooltimeDict.ContainsKey(itemId))
            return false;
        return true;
    }
    
    IEnumerator CoSetItemCooltime(string itemId, float cooltime)
    {
        ItemCooltimeDict.Add(itemId, cooltime);
        ItemIconFillAmount.Add(itemId, 1);
        yield return new WaitForSeconds(cooltime);
        ItemCooltimeDict.Remove(itemId);
        ItemIconFillAmount.Remove(itemId);
    }

    public float GetItemIconFillAmount(string itemId)
    {
        if (ItemIconFillAmount.ContainsKey(itemId))
            return ItemIconFillAmount[itemId];
        return 0f;
    }

    #endregion


    #region Passive

    private void CheckPassiveCooltime()
    {
        if (PassiveCooltimeDict.Count > 0)
        {
            foreach (KeyValuePair<Define.ESkillId, float> pair in PassiveIconFillAmount.ToList())
            {
                if (PassiveCooltimeDict.ContainsKey(pair.Key))
                {
                    float cooltime = PassiveCooltimeDict[pair.Key];

                    PassiveIconFillAmount[pair.Key] -= (1 / cooltime * Time.deltaTime);
                }
            }
        }
    }
    
    public void SetPassiveCooltime(Define.ESkillId passiveId, float cooltime)
    {
        StartCoroutine(CoSetPassiveCooltime(passiveId, cooltime));
    }

    public bool IsReadyPassive(Define.ESkillId passiveId)
    {
        if (PassiveCooltimeDict.ContainsKey(passiveId))
            return false;
        return true;
    }
    
    IEnumerator CoSetPassiveCooltime(Define.ESkillId passiveId, float cooltime)
    {
        PassiveCooltimeDict.Add(passiveId, cooltime);
        PassiveIconFillAmount.Add(passiveId, 1);
        yield return new WaitForSeconds(cooltime);
        PassiveCooltimeDict.Remove(passiveId);
        PassiveIconFillAmount.Remove(passiveId);
    }

    public float GetPassiveFillAmount(Define.ESkillId passiveId)
    {
        if (PassiveIconFillAmount.ContainsKey(passiveId))
            return PassiveIconFillAmount[passiveId];
        return 0f;
    }

    #endregion


    public void ResetCooltimeRandomActive()
    {
        List<int> runningCooltimeSkillOrder = new List<int>();
        if (FirstSkillTimer > Time.time)
            runningCooltimeSkillOrder.Add(1);
        if (SecondSkillTimer > Time.time)
            runningCooltimeSkillOrder.Add(2);
        if (ThirdSkillTimer > Time.time)
            runningCooltimeSkillOrder.Add(3);

        if (runningCooltimeSkillOrder.Count > 1)
        {
            int rand = Random.Range(0, runningCooltimeSkillOrder.Count);
            
            if (runningCooltimeSkillOrder[rand] == 1)
            { 
                FirstSkillTimer = 0;
                GI.Inst.UIManager.ResetCooltimeUI(EActiveSkillOrder.First);
            }
            else if (runningCooltimeSkillOrder[rand] == 2)
            {
                SecondSkillTimer = 0;
                GI.Inst.UIManager.ResetCooltimeUI(EActiveSkillOrder.Second);
            }
            else
            {
                ThirdSkillTimer = 0;
                GI.Inst.UIManager.ResetCooltimeUI(EActiveSkillOrder.Third);
            }
        }
        
        
    }
    
}
