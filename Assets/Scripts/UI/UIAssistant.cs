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

    private AQTEComponent _qteComponent;

    private void Start()
    {
        _qtePanel.SetActive(false);
    }

    public void Init(AQTEComponent component)
    {
        _qteComponent = component;
        _qteComponent.onFinish.AddListener(OnFinishQTE);

        _qtePanel.SetActive(true);

        _variants.Init(component.Variants);

        component.TryGetComponent(out TimeQTE timeQTE);

        if (timeQTE != null)
        {
            _progressBar.gameObject.SetActive(true);
            _progressBar.Init(timeQTE);
        }
        else
        {
            _progressBar.gameObject.SetActive(false);
        }
    }

    private void OnFinishQTE()
    {
        _qtePanel.SetActive(false);
    }
}
