using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VariantItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtDescritpion;
    [SerializeField] private TextMeshProUGUI _txtButtonInfo;

    public void Init(string description, string buttonInfo)
    {
        _txtDescritpion.text = description;
        _txtButtonInfo.text = buttonInfo;
    }
}
