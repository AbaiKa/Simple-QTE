using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineAssistant : MonoBehaviour
{
    [field: SerializeField]
    [field: Tooltip("Target timeline")]
    public PlayableDirector Timeline { get; private set; }

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

    private float GetTimelineValue(float value)
    {
        return value / 60;
    }
}
