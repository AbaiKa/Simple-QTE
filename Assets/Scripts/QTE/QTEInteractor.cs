using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(QTEComponent), typeof(Rigidbody), typeof(SphereCollider))]
public class QTEInteractor : MonoBehaviour
{
    [SerializeField]
    [Range(0, 10)]
    private float _interactionRadius;

    [SerializeField]
    private GameObject _viewPoint;

    private QTEComponent _component;
    private SphereCollider _sphereCollider;
    private Rigidbody _body;

    private void Awake()
    {
        _component = GetComponent<QTEComponent>();
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

            _viewPoint.gameObject.SetActive(true);
            _viewPoint.transform.position = other.transform.position;
            _viewPoint.transform.rotation = other.transform.rotation;

            _component.onFinish.AddListener(() => _viewPoint.SetActive(false));
        }
    }

    private void OnValidate()
    {
        _component = GetComponent<QTEComponent>();
        _sphereCollider = GetComponent<SphereCollider>();
        _body = GetComponent<Rigidbody>();

        _body.isKinematic = true;
        _body.useGravity = false;

        _sphereCollider.isTrigger = true;
        _sphereCollider.radius = _interactionRadius;
    }
}
