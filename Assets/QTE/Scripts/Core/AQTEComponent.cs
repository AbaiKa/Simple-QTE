using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public abstract class AQTEComponent : MonoBehaviour
{
    protected abstract void Logic();

    #region Properties
    [field: Header("Base")]

    [field: SerializeField]
    [field: Tooltip("ID �������� �� ��������� (������� �� ������ ���� ����� �� ����� �������� �� �����)")]
    public int DefaultVariantId { get; protected set; } = 0;

    [field: SerializeField]
    [field: Tooltip("��������")]
    public QTEVariant[] Variants { get; private set; } = new QTEVariant[2];

    [SerializeField]
    [Tooltip("������� ��������� ����� ������ ��������")]
    public UnityEvent onFinish;

    [field: Space()]
    [field: SerializeField]
    [field: Tooltip("Timeline assistant")]
    protected TimelineAssistant timeline { get; private set; }

    [field: SerializeField]
    [field: Tooltip("����� ������ ����� �� ���������")]
    protected float _startClipTime;

    [field: SerializeField]
    [field: Tooltip("����� ����� ����� �� ���������")]
    protected float _endClipTime;

    [field: SerializeField]
    [field: Tooltip("QTE View point")]
    protected GameObject viewPoint { get; private set;}

    [Space()]
    [SerializeField] protected float timelineLoopStartTime;
    [SerializeField] protected float timelineLoopEndTime;

    protected QTEInputComponent input { get; private set; } = new QTEInputComponent();
    #endregion

    private void Start()
    {
        DisableVariants();
    }

    public void LaunchQTE()
    {
        Init();
        Logic();
    }
    protected virtual void Init()
    {
        DisableVariants();
        viewPoint.SetActive(true);

        input.BindEvents(Variants);
        input.onClick.AddListener(Deinit);

        for (int i = 0; i < Variants.Length; i++)
        {
            Variants[i].onSelect.AddListener((self) =>
            {
                self.Cutscene.SetActive(true);
                timeline.ActivateOnFinishClipEvent(self.ClipDuration, _endClipTime);
                timeline.ContinueTimeline(_startClipTime);
            });
        }

        onFinish.AddListener(() => viewPoint.SetActive(false));
    }

    private void DisableVariants()
    {
        for (int i = 0; i < Variants.Length; i++)
        {
            Variants[i].Cutscene.SetActive(false);
        }
    }
    protected virtual void Deinit()
    {
        for (int i = 0; i < Variants.Length; i++)
        {
            Variants[i].onSelect.RemoveAllListeners();
        }

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
    [field: Tooltip("��������")]
    public GameObject Cutscene {  get; private set; }

    [field: SerializeField]
    [field: Tooltip("����� �����")]
    public float ClipDuration { get; private set; }

    [field: SerializeField]
    [field: Tooltip("������� ������� ������ ���������, ���� ������� ���� �������")]
    public UnityEvent<QTEVariant> onSelect {  get; private set; }
}
