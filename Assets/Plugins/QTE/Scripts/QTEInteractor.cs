using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class QTEInteractor : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private float _interactionRadius;

    [SerializeField]
    private AQTEComponent _component;

    private SphereCollider _sphereCollider;
    private Rigidbody _body;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _body = GetComponent<Rigidbody>();

        _body.isKinematic = true;
        _body.useGravity = false;

        _sphereCollider.isTrigger = true;
        _sphereCollider.radius = _interactionRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QTEAssistant.Main.SetQTE(_component);
        }
    }

    private void OnValidate()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _body = GetComponent<Rigidbody>();

        _body.isKinematic = true;
        _body.useGravity = false;

        _sphereCollider.isTrigger = true;
        _sphereCollider.radius = _interactionRadius;
    }
}
