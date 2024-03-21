using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeButtom : MonoBehaviour
{
    [SerializeField] NodeButtom parentNode;
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (parentNode != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, parentNode.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

