using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class StaticQTE : AQTEComponent
{
    protected override void Init()
    {
        base.Init();

        onFinish.AddListener(timeline.StopLoop);
    }

    protected override void Logic()
    {
        timeline.StartLoop(timelineLoopStartTime, timelineLoopEndTime);
    }
}
