using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{

    Vector3 center;
    private void Start()
    {
        Draw();
    }
    
    private void Draw()
    {
        center = gameObject.transform.position;
        float radius = 3f;
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 360;
        lineRenderer.startWidth = 0.04f;
        lineRenderer.endWidth = 0.04f;

        for (int i = 0; i < 360; i++)
        {
            float x = center.x + radius * Mathf.Cos(i * Mathf.PI / 180f);
            //float z = center.z + radius * Mathf.Sin(i * Mathf.PI / 180f);
            float y = center.y + radius * Mathf.Sin(i * Mathf.PI / 180f);
            lineRenderer.SetPosition(i, new Vector3(x, y, center.z));
        }
    }
}
