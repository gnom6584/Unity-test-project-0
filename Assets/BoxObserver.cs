using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObserver
{
    public const float DEFAULT_ZOOM = 3.0f;
    public const float ROTATION_ZOOM = 100.0f;

    public BoxCollider Box { get; set; }

    public float Zoom { get; set; }

    public float RotationSpeed { get; set; }

    public BoxObserver()
    {
        Zoom = DEFAULT_ZOOM;
        RotationSpeed = ROTATION_ZOOM;
    }

    public void ResetRotation()
    {
        Box.transform.rotation = Quaternion.identity;
    }

    public void RotateX()
    {
        Box.transform.Rotate(RotationSpeed * Time.deltaTime, 0.0f, 0.0f);
    }

    public void RotateY()
    {
        Box.transform.Rotate(0.0f, RotationSpeed * Time.deltaTime, 0.0f);
    }

    public void RotateZ()
    {
        Box.transform.Rotate(0.0f, 0.0f, RotationSpeed * Time.deltaTime);
    }

    public void PutBoxFront()
    {
        if (Box != null)
        {
            Box.transform.position = Camera.main.transform.position + Camera.main.transform.forward * Zoom;
        }
    }
}
