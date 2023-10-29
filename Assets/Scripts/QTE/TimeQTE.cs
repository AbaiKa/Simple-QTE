using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeQTE : AQTEComponent
{
    [field: Space()]
    [field: SerializeField]
    [field: Tooltip("Время ожидания ответа")]
    [field: Range(0f, 10f)]
    public float ResponseTime = 0;

    /// <summary>
    /// Вызывается когда меняется значение таймера
    /// </summary>
    public Action<float> onTimerValueChanged;

    private float _timeLeft = 0f;

    protected override void Init()
    {
        base.Init();
        _timeLeft = ResponseTime;
    }
    protected override void Logic()
    {
        StartCoroutine(QTERoutine());
    }
    protected override void Deinit()
    {
        base.Deinit();
        StopAllCoroutines();
    }
    private IEnumerator QTERoutine()
    {
        yield return new WaitForEndOfFrame();

        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            onTimerValueChanged?.Invoke(_timeLeft);
            yield return null;
        }

        Debug.Log("Закончилось время, выбираем по дефолту");
        Variants[DefaultVariantId].onSelect?.Invoke(Variants[DefaultVariantId]);

        Deinit();
    }
}
