using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 10f;
    float minX;

    private void Start()
    {
        minX = transform.position.x;
    }

    void FixedUpdate()
    {
        float desiredX = Mathf.Lerp(transform.position.x, target.position.x, smoothSpeed * Time.fixedDeltaTime);
        Vector3 nextPosition = new Vector3(desiredX < minX ? minX : desiredX, transform.position.y, -1);
        transform.position = nextPosition;
    }
}
