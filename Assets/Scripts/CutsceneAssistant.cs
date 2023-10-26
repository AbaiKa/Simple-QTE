using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneAssistant : MonoBehaviour
{
    public static CutsceneAssistant Instance { get; private set; }
    public bool cutscneIsPlaying;

    [SerializeField] private GameObject _preQTECutscene;
    [SerializeField] private GameObject _cutsceneQTE_1;
    [SerializeField] private GameObject _cutsceneQTE_2;

    [SerializeField] private Collider _qteInteractCollider;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableInteractionCollider()
    {
        _qteInteractCollider.enabled = true;
        cutscneIsPlaying = true;
    }
    public void PreCutscene()
    {
        _preQTECutscene.SetActive(true);
        _cutsceneQTE_1.SetActive(false);
        _cutsceneQTE_2.SetActive(false);
        cutscneIsPlaying = true;
    }

    public void Cutscene1()
    {
        _preQTECutscene.SetActive(false);
        _cutsceneQTE_1.SetActive(true);
        _cutsceneQTE_2.SetActive(false);
        cutscneIsPlaying = true;
    }
    public void Cutscene2()
    {
        _preQTECutscene.SetActive(false);
        _cutsceneQTE_1.SetActive(false);
        _cutsceneQTE_2.SetActive(true);
        cutscneIsPlaying = true;
    }

    public void EndCutscene()
    {
        cutscneIsPlaying = false;
    }
}
