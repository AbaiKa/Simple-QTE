using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineAssistant : MonoBehaviour
{
    [field: SerializeField]
    [field: Tooltip("Target timeline")]
    public PlayableDirector Timeline { get; private set; }

    private bool _stopLoop;

    public void StopLoop()
    {
        _stopLoop = true;
    }

    public void BindEventToTimline(QTEVariant[] variants)
    {
        for(int i = 0; i < variants.Length; i++)
        {
            variants[i].onSelect.AddListener((self) => 
            {
                Timeline.time = GetTimelineValue(self.TimelineValue);
            });
        }
    }

    public void StartLoop(float startTime, float endTime)
    {
        _stopLoop = false;
        StartCoroutine(LoopRoutine(GetTimelineValue(startTime), GetTimelineValue(endTime)));
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
        return value / 60;
    }
}
