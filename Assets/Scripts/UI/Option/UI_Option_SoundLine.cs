using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option_SoundLine : MonoBehaviour
{
    [SerializeField] private ESoundType soundType;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;

    public void InitOnce()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
        slider.onValueChanged.AddListener(OnValueChanged);

        GI.Inst.SoundManager.GetAudioVolume(soundType, out float value);
        value = 100 * (value + 70) / 70; //dB -> 0~100
        slider.value = value * 0.01f;
        valueText.text = Mathf.FloorToInt(value).ToString();
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        slider.value = value;
        value *= 100;
        valueText.text = Mathf.FloorToInt(value).ToString();
        value = (value * 70 / 100) - 70; //0~100 => dB
        GI.Inst.SoundManager.SetAudioVolume(soundType, value);
    }
}
