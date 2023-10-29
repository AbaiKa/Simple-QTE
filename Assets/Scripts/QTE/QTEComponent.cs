using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class QTEComponent : MonoBehaviour
{
    #region Properties
    [field: SerializeField]
    [field: Tooltip("Варианты")]
    public QTEVariant[] Variants { get; private set; } = new QTEVariant[2];

    [field: Space()]
    [field: SerializeField]
    [field: Tooltip("Время ожидания ответа")]
    [field: Range(0f, 10f)]
    public float ResponseTime = 0;

    [SerializeField]
    [Tooltip("ID варианта по умолчанию (вариант на случай если игрок не успел ответить во время)")]
    private int _defaultVariantId = 0;
    #endregion

    /// <summary>
    /// Вызывается после выбора варианта или после истечения времени
    /// </summary>
    public UnityEvent onFinish;

    /// <summary>
    /// Вызывается когда меняется значение таймера
    /// </summary>
    public Action<float> onTimerValueChanged;

    private float _timeLeft = 0f;

    #region Methods

    #region Public
    public void LaunchQTE()
    {
        Init();
        StartCoroutine(QTERoutine());
    }
    #endregion

    #region Private

    /// <summary>
    /// Инициализация перед запуском QTE
    /// </summary>
    private void Init()
    {
        _timeLeft = ResponseTime;

        for (int i = 0; i < Variants.Length; i++)
        {
            var ia = QTEAssistant.Main.Input.FindAction(Variants[i].InputActionNameOrId);

            if (ia == null)
            {
                Debug.LogError($"НЕправильное название ia в варианте: {i}");
                continue;
            }

            ia.performed += OnSelectVariant;
        }
    }

    /// <summary>
    /// Событие вызывает IA
    /// </summary>
    /// <param name="context"></param>
    private void OnSelectVariant(InputAction.CallbackContext context)
    {
        Variants[GetIdByActionName(context.action.name)].onSelect?.Invoke();
        ForceStop();
    }

    /// <summary>
    /// Возвращает id варианта по имени IA
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    private int GetIdByActionName(string component)
    {
        for(int i = 0; i < Variants.Length; ++i)
        {
            if (Variants[i].InputActionNameOrId == component)
            {
                return i;
            }
        }

        return 0;
    }

    /// <summary>
    /// Отписываемся от всех инпутов и останавливаем таймер
    /// </summary>
    private void ForceStop()
    {
        for (int i = 0; i < Variants.Length; i++)
        {
            InputAction ia = QTEAssistant.Main.Input.FindAction(Variants[i].InputActionNameOrId);


            if (ia == null)
            {
                Debug.LogError($"НЕправильное название Input Action в варианте: {i}");
                continue;
            }

            ia.performed -= OnSelectVariant;
        }

        onFinish?.Invoke();

        onFinish.RemoveAllListeners();
        StopAllCoroutines();
    }

    /// <summary>
    /// Корутина для обновления таймера
    /// </summary>
    /// <returns></returns>
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
        Variants[_defaultVariantId].onSelect?.Invoke();

        ForceStop();
    }
    #endregion

#endregion

    #region Helper
    private void OnValidate()
    {
        if(Variants.Length <= 1 || Variants.Length > 4)
            Variants = new QTEVariant[2];

        if (_defaultVariantId > Variants.Length || _defaultVariantId < 0)
            _defaultVariantId = 0;
    }
    #endregion
}

[Serializable]
public class QTEVariant
{
    [field: SerializeField]
    [field: Tooltip("Описание варианта (отображается в UI)")]
    public string Description;

    [field: SerializeField]
    [field: Tooltip("Название Input Action (Название можно посмотреть кликнув два раза на QTE InputAction (QTE))")]
    public string InputActionNameOrId;

    [field: SerializeField]
    [field: Tooltip("Событие которое должно сработать, если выберем этот вариант")]
    public UnityEvent onSelect;
}