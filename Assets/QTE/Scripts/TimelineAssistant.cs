using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

public class TimelineAssistant : MonoBehaviour
{
    [field: SerializeField]
    [field: Tooltip("Target timeline")]
    public PlayableDirector Timeline { get; private set; }

    [field: SerializeField]
    [field: Tooltip("Вызывается после окончания клипа")]
    public UnityEvent onFinishClip;

    [SerializeField] private float _timelineLoopStartTime;
    [SerializeField] private float _timelineLoopEndTime;

    private TimelineAsset _timelineAsset;
    private bool _stopLoop;

    private float _endTime;

    private void Start()
    {
        _timelineAsset = (TimelineAsset)Timeline.playableAsset;
    }

    public void ContinueTimeline(float continueTime)
    {
        Timeline.time = GetTimelineValue(continueTime);
    }
    public void StartLoop()
    {
        _stopLoop = false;
        StartCoroutine(LoopRoutine(GetTimelineValue(_timelineLoopStartTime), GetTimelineValue(_timelineLoopEndTime)));
    }
    public void StartLoop(float startTime, float endTime)
    {
        _stopLoop = false;
        StartCoroutine(LoopRoutine(GetTimelineValue(startTime), GetTimelineValue(endTime)));
    }
    public void StopLoop()
    {
        _stopLoop = true;
    }

    public void ActivateOnFinishClipEvent(float clipDuration, float endTime)
    {
        _endTime = endTime;
        Invoke(nameof(OnFinishClip), GetTimelineValue(clipDuration));
    }

    private void OnFinishClip()
    {
        ContinueTimeline(_endTime);
        onFinishClip?.Invoke();
    }

    private IEnumerator LoopRoutine(float startTime, float endTime)
    {
        while(true)
        {
            if(_stopLoop)
            {
                yield break;
            }

            if(Timeline.time >= endTime)
            {
                Timeline.time = startTime;
            }
            Timeline.time += Time.deltaTime;
            Timeline.Evaluate();

            yield return null;
        }
    }

    private float GetTimelineValue(float value)
    {
        Debug.Log($"Value: {value} Result: {value / 60}");
        return value / 60;
    }
}
