using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneAssistant : MonoBehaviour
{
    public static CutsceneAssistant Instance { get; private set; }
    public bool InProgress { get; private set; }

    [SerializeField] private GameObject _preQTECutscene;
    [SerializeField] private GameObject _cutsceneQTE_1;
    [SerializeField] private GameObject _cutsceneQTE_2;

    [SerializeField] private GameObject _fullCutscene;

    private void Awake()
    {
        Instance = this;
    }
    public void PreCutscene()
    {
        _fullCutscene.SetActive(false);
        _preQTECutscene.SetActive(true);
        _cutsceneQTE_1.SetActive(false);
        _cutsceneQTE_2.SetActive(false);
        InProgress = true;
    }

    public void Cutscene1()
    {
        _fullCutscene.SetActive(false);
        _preQTECutscene.SetActive(false);
        _cutsceneQTE_1.SetActive(true);
        _cutsceneQTE_2.SetActive(false);
        InProgress = true;
    }
    public void Cutscene2()
    {
        _fullCutscene.SetActive(false);
        _preQTECutscene.SetActive(false);
        _cutsceneQTE_1.SetActive(false);
        _cutsceneQTE_2.SetActive(true);
        InProgress = true;
    }
    public void FullCutscene()
    {
        _fullCutscene.SetActive(true);
        _preQTECutscene.SetActive(false);
        _cutsceneQTE_1.SetActive(false);
        _cutsceneQTE_2.SetActive(false);
        InProgress = true;
    }
    public void EndCutscene()
    {
        InProgress = false;
        _preQTECutscene.SetActive(false);
        _cutsceneQTE_1.SetActive(false);
        _cutsceneQTE_2.SetActive(false);
        _fullCutscene.SetActive(false);
    }
}
