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
    [field: Tooltip("ID варианта по умолчанию (вариант на случай если игрок не успел ответить во время)")]
    public int DefaultVariantId { get; protected set; } = 0;

    [field: SerializeField]
    [field: Tooltip("Варианты")]
    public QTEVariant[] Variants { get; private set; } = new QTEVariant[2];

    [SerializeField]
    [Tooltip("Событие сработает после выбора варианта")]
    public UnityEvent onFinish;

    [field: Space()]
    [field: SerializeField]
    [field: Tooltip("Timeline assistant")]
    protected TimelineAssistant timeline { get; private set; }

    [field: SerializeField]
    [field: Tooltip("Время начало клипа на таймлайне")]
    protected float _startClipTime;

    [field: SerializeField]
    [field: Tooltip("Время конца клипа на таймлайне")]
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
    [field: Tooltip("Описание варианта (отображается в UI)")]
    public string Description { get; private set; }

    [field: SerializeField]
    [field: Tooltip("Название Input Action (Название можно посмотреть кликнув два раза на QTE InputAction (QTE))")]
    public string InputActionNameOrId {  get; private set; }

    [field: SerializeField]
    [field: Tooltip("Катсцена")]
    public GameObject Cutscene {  get; private set; }

    [field: SerializeField]
    [field: Tooltip("Длина клипа")]
    public float ClipDuration { get; private set; }

    [field: SerializeField]
    [field: Tooltip("Событие которое должно сработать, если выберем этот вариант")]
    public UnityEvent<QTEVariant> onSelect {  get; private set; }
}
