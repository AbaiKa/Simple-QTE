using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class InteractionComponent : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private float _interactionRadius;

    [SerializeField]
    private UnityEvent _onInteract;

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
            _onInteract.Invoke();
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
