using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneAssistant : MonoBehaviour
{
    public static CutsceneAssistant Instance { get; private set; }
    public bool InProgress { get; private set; }

    [SerializeField] private PlayableDirector _dialogueCutscene;
    [SerializeField] private PlayableDirector _simpleDialogueCutscene;
    private void Awake()
    {
        Instance = this;
    }
    public void DialogueCutscene()
    {
        _dialogueCutscene.time = 0;
        _dialogueCutscene.Play();
        InProgress = true;
    }
    public void SimpleDialogue()
    {
        _simpleDialogueCutscene.time = 0;
        _simpleDialogueCutscene.Play();
        InProgress = true;
    }
    public void EndCutscene()
    {
        InProgress = false;
        _dialogueCutscene.time = 0;
        _simpleDialogueCutscene.time = 0;
        _dialogueCutscene.Stop();
        _simpleDialogueCutscene.Stop();
    }
}
