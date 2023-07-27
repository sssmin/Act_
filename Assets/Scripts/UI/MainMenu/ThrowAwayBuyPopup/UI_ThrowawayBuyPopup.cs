using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ThrowawayBuyPopup : UI_Popup
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] Button background;
    [SerializeField] RectTransform contentParent;
    [SerializeField] Button closeButton;
    [SerializeField] RectTransform contentParentBackground;
    
    public void Init(EThrowawayBuyPopupType type, SO_Item item)
    {
        background.onClick.AddListener(() =>
        {
            GI.Inst.UIManager.ClosePopup();
        });
        
        closeButton.onClick.AddListener(() =>
        {
            GI.Inst.UIManager.ClosePopup();
        });
        
        switch (type)
        {
            case EThrowawayBuyPopupType.ThrowAwayConfirm:
                {
                    titleText.text = "정말 버릴건가요?";
                    GameObject go =
                        GI.Inst.ResourceManager.Instantiate("UI_TB_ConfirmButton", contentParent.transform);
                    UI_TB_ConfirmButton tbConfirmButton = go.GetComponent<UI_TB_ConfirmButton>();
                    contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x,
                        contentParent.sizeDelta.y + tbConfirmButton.GetComponent<RectTransform>().rect.height);
                    contentParentBackground.sizeDelta = new Vector2(contentParentBackground.sizeDelta.x,
                        contentParentBackground.sizeDelta.y + tbConfirmButton.GetComponent<RectTransform>().rect.height);
                    tbConfirmButton.Init(item, (i) => { GI.Inst.ListenerManager.SubItem(i, true); }, "버리기");
                }
                break;
            case EThrowawayBuyPopupType.ThrowAway:
                {
                    titleText.text = "버리기";
                    //Icon/Desc
                    GameObject go =
                        GI.Inst.ResourceManager.Instantiate("UI_TB_IconDesc", contentParent.transform);
                    UI_TB_IconDesc tbIconDesc = go.GetComponent<UI_TB_IconDesc>();
                    contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x,
                        contentParent.sizeDelta.y + tbIconDesc.GetComponent<RectTransform>().rect.height);
                    contentParentBackground.sizeDelta = new Vector2(contentParentBackground.sizeDelta.x,
                        contentParentBackground.sizeDelta.y + tbIconDesc.GetComponent<RectTransform>().rect.height);
                    tbIconDesc.Init(item.itemIcon, item.itemDesc);
                    //Amount
                    go =
                        GI.Inst.ResourceManager.Instantiate("UI_TB_Amount", contentParent.transform);
                    UI_TB_Amount tbAmount = go.GetComponent<UI_TB_Amount>();
                    contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x,
                        contentParent.sizeDelta.y + tbAmount.GetComponent<RectTransform>().rect.height);
                    contentParentBackground.sizeDelta = new Vector2(contentParentBackground.sizeDelta.x,
                        contentParentBackground.sizeDelta.y + tbAmount.GetComponent<RectTransform>().rect.height);
                    tbAmount.Init(item, type);
                    //Confirm
                    go =
                        GI.Inst.ResourceManager.Instantiate("UI_TB_ConfirmButton", contentParent.transform);
                    UI_TB_ConfirmButton tbConfirmButton = go.GetComponent<UI_TB_ConfirmButton>();
                    contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x,
                        contentParent.sizeDelta.y + tbConfirmButton.GetComponent<RectTransform>().rect.height);
                    contentParentBackground.sizeDelta = new Vector2(contentParentBackground.sizeDelta.x,
                        contentParentBackground.sizeDelta.y + tbConfirmButton.GetComponent<RectTransform>().rect.height);
                    tbConfirmButton.Init(item, (i) =>
                    {
                        GI.Inst.ListenerManager.SubItem(i, true, tbAmount.Amount);
                    }, "버리기");
                }
                break;
            case EThrowawayBuyPopupType.Buy:
                {
                    //헤더 이름 바꾸기
                    titleText.text = "구매";
                    
                    //Icon/Desc
                    GameObject go =
                        GI.Inst.ResourceManager.Instantiate("UI_TB_IconDesc", contentParent.transform);
                    UI_TB_IconDesc tbIconDesc = go.GetComponent<UI_TB_IconDesc>();
                    contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x,
                        contentParent.sizeDelta.y + tbIconDesc.GetComponent<RectTransform>().rect.height);
                    contentParentBackground.sizeDelta = new Vector2(contentParentBackground.sizeDelta.x,
                        contentParentBackground.sizeDelta.y + tbIconDesc.GetComponent<RectTransform>().rect.height);
                    tbIconDesc.Init(item.itemIcon, item.itemDesc);
                    //Amount
                    go =
                        GI.Inst.ResourceManager.Instantiate("UI_TB_Amount", contentParent.transform);
                    UI_TB_Amount tbAmount = go.GetComponent<UI_TB_Amount>();
                    contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x,
                        contentParent.sizeDelta.y + tbAmount.GetComponent<RectTransform>().rect.height);
                    contentParentBackground.sizeDelta = new Vector2(contentParentBackground.sizeDelta.x,
                        contentParentBackground.sizeDelta.y + tbAmount.GetComponent<RectTransform>().rect.height);
                    
                    //Price
                    go =
                        GI.Inst.ResourceManager.Instantiate("UI_TB_Price", contentParent.transform);
                    UI_TB_Price tbPrice = go.GetComponent<UI_TB_Price>();
                    contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x,
                        contentParent.sizeDelta.y + tbPrice.GetComponent<RectTransform>().rect.height);
                    contentParentBackground.sizeDelta = new Vector2(contentParentBackground.sizeDelta.x,
                        contentParentBackground.sizeDelta.y + tbPrice.GetComponent<RectTransform>().rect.height);
                  
                    tbAmount.BuyInit(item, type, tbPrice.Init);
                    //Confirm
                    go =
                        GI.Inst.ResourceManager.Instantiate("UI_TB_ConfirmButton", contentParent.transform);
                    UI_TB_ConfirmButton tbConfirmButton = go.GetComponent<UI_TB_ConfirmButton>();
                    contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x,
                        contentParent.sizeDelta.y + tbConfirmButton.GetComponent<RectTransform>().rect.height);
                    contentParentBackground.sizeDelta = new Vector2(contentParentBackground.sizeDelta.x,
                        contentParentBackground.sizeDelta.y + tbConfirmButton.GetComponent<RectTransform>().rect.height);
                    tbConfirmButton.Init(item, (i) =>
                    {
                        GI.Inst.ListenerManager.BuyItem(i, tbAmount.Amount);
                    }, "구매");
                }
                break;
        }
    }

    private void OnDestroy()
    {
        background.onClick.RemoveListener(() =>
        {
            GI.Inst.UIManager.ClosePopup();
        });
        
        closeButton.onClick.RemoveListener(() =>
        {
            GI.Inst.UIManager.ClosePopup();
        });
    }

    public override void Close()
    {
        GI.Inst.ResourceManager.Destroy(gameObject);
    }
}
