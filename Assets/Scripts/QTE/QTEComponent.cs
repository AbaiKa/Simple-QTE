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
    [field: Tooltip("��������")]
    public QTEVariant[] Variants { get; private set; } = new QTEVariant[2];

    [field: Space()]
    [field: SerializeField]
    [field: Tooltip("����� �������� ������")]
    [field: Range(0f, 10f)]
    public float ResponseTime = 0;

    [SerializeField]
    [Tooltip("ID �������� �� ��������� (������� �� ������ ���� ����� �� ����� �������� �� �����)")]
    private int _defaultVariantId = 0;
    #endregion

    /// <summary>
    /// ���������� ����� ������ �������� ��� ����� ��������� �������
    /// </summary>
    public UnityEvent onFinish;

    /// <summary>
    /// ���������� ����� �������� �������� �������
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
    /// ������������� ����� �������� QTE
    /// </summary>
    private void Init()
    {
        _timeLeft = ResponseTime;

        for (int i = 0; i < Variants.Length; i++)
        {
            var ia = QTEAssistant.Main.Input.FindAction(Variants[i].InputActionNameOrId);

            if (ia == null)
            {
                Debug.LogError($"������������ �������� ia � ��������: {i}");
                continue;
            }

            ia.performed += OnSelectVariant;
        }
    }

    /// <summary>
    /// ������� �������� IA
    /// </summary>
    /// <param name="context"></param>
    private void OnSelectVariant(InputAction.CallbackContext context)
    {
        Variants[GetIdByActionName(context.action.name)].onSelect?.Invoke();
        ForceStop();
    }

    /// <summary>
    /// ���������� id �������� �� ����� IA
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
    /// ������������ �� ���� ������� � ������������� ������
    /// </summary>
    private void ForceStop()
    {
        for (int i = 0; i < Variants.Length; i++)
        {
            InputAction ia = QTEAssistant.Main.Input.FindAction(Variants[i].InputActionNameOrId);


            if (ia == null)
            {
                Debug.LogError($"������������ �������� Input Action � ��������: {i}");
                continue;
            }

            ia.performed -= OnSelectVariant;
        }

        onFinish?.Invoke();

        onFinish.RemoveAllListeners();
        StopAllCoroutines();
    }

    /// <summary>
    /// �������� ��� ���������� �������
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

        Debug.Log("����������� �����, �������� �� �������");
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
    [field: Tooltip("�������� �������� (������������ � UI)")]
    public string Description;

    [field: SerializeField]
    [field: Tooltip("�������� Input Action (�������� ����� ���������� ������� ��� ���� �� QTE InputAction (QTE))")]
    public string InputActionNameOrId;

    [field: SerializeField]
    [field: Tooltip("������� ������� ������ ���������, ���� ������� ���� �������")]
    public UnityEvent onSelect;
}