using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _txtTimer;

    private QTEComponent _qteComponent;

    private float _onePercent;
    public void Init(QTEComponent component)
    {
        _qteComponent = component;
        _onePercent = component.ResponseTime / 100;
        _qteComponent.onTimerValueChanged += UpdateProgressBar;
    }
    private void OnDestroy()
    {
        _qteComponent.onTimerValueChanged -= UpdateProgressBar;
    }

    public void UpdateProgressBar(float value)
    {
        float result = (value / _onePercent) / 100;

        _fillImage.fillAmount = result;

        TimeSpan time = TimeSpan.FromSeconds(value);
        _txtTimer.text = $"{time.Seconds}:{time.Milliseconds}";
    }
}
