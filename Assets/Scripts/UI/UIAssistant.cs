using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.InputSystem.Android.LowLevel.AndroidGameControllerState;

public class UIAssistant : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VariantsAssistant _variants;
    [SerializeField] private ProgressBar _progressBar;

    [SerializeField] private GameObject _qtePanel;

    private QTEComponent _qteComponent;

    private void Start()
    {
        _qtePanel.SetActive(false);
    }

    public void Init(QTEComponent component)
    {
        if (_qteComponent != component)
        {
            _qteComponent = component;
            _qteComponent.onFinish += OnFinishQTE;
        }
        
        _qtePanel.SetActive(true);

        _variants.Init(component.Variants);
        _progressBar.Init(component);
    }

    private void OnFinishQTE()
    {
        _qtePanel.SetActive(false);
    }
}
