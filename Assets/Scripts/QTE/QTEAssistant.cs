using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTEAssistant : MonoBehaviour
{
    public static QTEAssistant Main { get; private set; }
    public bool InPorgress {  get; private set; }
    public QTEInput Input { get; private set; }
    public QTEComponent CurrentQTE { get; private set; }


    [field: SerializeField] 
    public UIAssistant UI { get; private set; }


    private void Awake()
    {
        Main = this;
        Input = new QTEInput();
    }


    private void OnEnable()
    {
        Input.Enable();
    }
    private void OnDisable()
    {
        Input.Disable(); 
    }


    public void SetQTE(QTEComponent component)
    {
        if (InPorgress)
            return;

        CurrentQTE = component;

        UI.Init(component);

        CurrentQTE.LaunchQTE();

        InPorgress = true;
        CurrentQTE.onFinish.AddListener(() => InPorgress = false);
    }
}
