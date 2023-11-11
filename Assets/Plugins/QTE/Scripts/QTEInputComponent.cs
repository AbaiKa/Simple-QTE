using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class QTEInputComponent
{
    public UnityEvent onClick = new UnityEvent();

    private QTEVariant[] _variants;

    public void BindEvents(QTEVariant[] variants)
    {
        if (variants == null)
            return;

        _variants = variants;

        for (int i = 0; i < _variants.Length; i++)
        {
            var ia = QTEAssistant.Main.Input.FindAction(_variants[i].InputActionNameOrId);

            if (ia == null)
            {
                Debug.LogError($"НЕправильное название ia в варианте: {i}");
                continue;
            }

            ia.performed += OnSelectVariant;
        }
    }

    public void UnbindEvents()
    {
        if (_variants == null)
            return;

        for (int i = 0; i < _variants.Length; i++)
        {
            InputAction ia = QTEAssistant.Main.Input.FindAction(_variants[i].InputActionNameOrId);


            if (ia == null)
            {
                Debug.LogError($"НЕправильное название Input Action в варианте: {i}");
                continue;
            }

            ia.performed -= OnSelectVariant;
        }
    }

    /// <summary>
    /// Событие вызывает IA
    /// </summary>
    /// <param name="context"></param>
    private void OnSelectVariant(InputAction.CallbackContext context)
    {
        int id = GetIdByActionName(context.action.name);
        _variants[id].onSelect?.Invoke(_variants[id]);

        onClick?.Invoke();
        onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Возвращает id варианта по имени IA
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    private int GetIdByActionName(string component)
    {
        for (int i = 0; i < _variants.Length; ++i)
        {
            if (_variants[i].InputActionNameOrId == component)
            {
                return i;
            }
        }

        return 0;
    }
}
