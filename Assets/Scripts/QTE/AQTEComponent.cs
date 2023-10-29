using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public abstract class AQTEComponent : MonoBehaviour
{
    protected abstract void Logic();

    #region Properties
    [field: Header("Base")]

    [field: SerializeField]
    [field: Tooltip("��������")]
    public QTEVariant[] Variants { get; private set; } = new QTEVariant[2];

    [SerializeField]
    [Tooltip("ID �������� �� ��������� (������� �� ������ ���� ����� �� ����� �������� �� �����)")]
    public int DefaultVariantId { get; protected set; } = 0;

    [SerializeField]
    [Tooltip("������� ��������� ����� ������ ��������")]
    public UnityEvent onFinish;

    [field: SerializeField]
    [field: Tooltip("Timeline assistant")]
    protected TimelineAssistant timeline {  get; private set; }

    [field: SerializeField]
    [field: Tooltip("QTE View point")]
    protected GameObject viewPoint { get; private set;}

    protected QTEInputComponent input { get; private set; } = new QTEInputComponent();
    #endregion

    public void LaunchQTE()
    {
        Init();
        Logic();
    }
    protected virtual void Init()
    {
        viewPoint.SetActive(true);

        input.BindEvents(Variants);
        input.onClick.AddListener(Deinit);

        if (timeline != null)
            timeline.BindEventToTimline(Variants);

        onFinish.AddListener(() => viewPoint.SetActive(false));
    }
    protected virtual void Deinit()
    {
        input.UnbindEvents();

        onFinish?.Invoke();
        onFinish.RemoveAllListeners();
    }


    private void OnValidate()
    {
        if (Variants.Length <= 1 || Variants.Length > 4)
            Variants = new QTEVariant[2];

        if (DefaultVariantId > Variants.Length || DefaultVariantId < 0)
            DefaultVariantId = 0;
    }
}

[Serializable]
public class QTEVariant
{
    [field: SerializeField]
    [field: Tooltip("�������� �������� (������������ � UI)")]
    public string Description { get; private set; }

    [field: SerializeField]
    [field: Tooltip("�������� Input Action (�������� ����� ���������� ������� ��� ���� �� QTE InputAction (QTE))")]
    public string InputActionNameOrId {  get; private set; }

    [field: SerializeField]
    [field: Tooltip("����� �� ����� ��������� ��� ������ �������� ���� ����� ������ ��������")]
    public int TimelineValue {  get; private set; }

    [field: SerializeField]
    [field: Tooltip("������� ������� ������ ���������, ���� ������� ���� �������")]
    public UnityEvent<QTEVariant> onSelect {  get; private set; }
}
