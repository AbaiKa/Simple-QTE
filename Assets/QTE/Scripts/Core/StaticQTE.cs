using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticQTE : AQTEComponent
{
    [SerializeField] private int _startTimeline;
    [SerializeField] private int _endTimeline;

    protected override void Logic()
    {
        timeline.StartLoop(_startTimeline, _endTimeline);
    }
}
