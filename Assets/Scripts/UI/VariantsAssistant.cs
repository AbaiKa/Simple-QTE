using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantsAssistant : MonoBehaviour
{
    [SerializeField] private VariantItem _variantPrefab;
    [SerializeField] private Transform _variantsContainer;

    private List<VariantItem> _variants = new List<VariantItem>();

    public void Init(QTEVariant[] variants)
    {
        ClearVariants();
        CreateVariants(variants);
    }

    private void CreateVariants(QTEVariant[] variants)
    {
        for (int i = 0; i < variants.Length; i++)
        {
            var v = Instantiate(_variantPrefab, _variantsContainer);
            var ia = QTEAssistant.Main.Input.FindAction(variants[i].InputActionNameOrId);
            v.Init(variants[i].Description, GetButtonNameFromBindingsPath(ia.bindings[0].path));
            _variants.Add(v);
        }
    }

    private void ClearVariants()
    {
        for(int i = 0; i < _variants.Count; i++)
        {
            Destroy(_variants[i].gameObject);
        }

        _variants.Clear();
    }

    /// <summary>
    /// Делает из "<Keyboard>/v" "v"
    /// </summary>
    /// <param name="component"></param>
    private string GetButtonNameFromBindingsPath(string component)
    {
        string result = component.Remove(0, component.IndexOf('/') + 1);

        return result;
    }
}
