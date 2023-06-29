using UnityEngine;
using UnityEngine.Serialization;


    public class PrefabId : MonoBehaviour
    {
        [SerializeField] string value;
        public string Value => value;
    }

    // public enum EPrefabId
    // {
    //     Arrow,
    //     Dagger,
    //     PlayerClone,
    //     Player,
    //     NightBorne,
    //     DaggerUlt,
    //     DaggerBall,
    //     FireStrike,
    //     Earthquake,
    //     Axe,
    //     AxeUlt,
    //     ArrowRain,
    //     PiercingArrow,
    //     ArrowBuff,
    //     DistortionArrow,
    //     
    //     UI_MainMenu,
    //     UI_Main,
    //     UI_Inven_ItemSlot,
    //     UI_Inven_ConsumableItemSlot,
    //     UI_Inven_StatSlot,
    //     UI_Inven_StatsContent,
    //     UI_Skill_SkillContent,
    //     UI_Skill_ActiveSkillSlot,
    //     UI_Inven_CategoryButton,
    //     UI_Main_MonsterInfo,
    //     UI_Main_SkillHotkeySlot,
    //     
    //     UI_ItemTooltip,
    //     UI_ItemTooltip_ItemStats,
    //     UI_ItemTooltip_EffectDescObject,
    //     UI_ItemTooltip_DescText,
    //     
    //     UI_Inven_Popup,
    //     UI_Inven_PopupButton,
    //     
    //     UI_TB_Popup, //버리기/사기 팝업
    //     UI_TB_IconDesc,
    //     UI_TB_Amount,
    //     UI_TB_Price,
    //     UI_TB_ConfirmButton,
    //     
    //     UI_Main_ItemHotkeySlot,
    //     UI_Main_EffectSlot,
    //     
    //     UI_Skill_EquipPassiveSkillSlot,
    //     UI_Skill_PassiveSkillSlot,
    // }



