using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("CameraPoints")]
    public Transform leftDestination;
    public Transform rightDestination;
    public Transform basePoint;
    public float speed;

    private bool moveCameraTrigger = false;
    private bool isMoving = false;
    private Transform destinationToFollow;

    private Vector3 finalPos;
    private Quaternion finalRot;
    private float tempDelay;

    private Vector3 originPoint;
    private Quaternion originRot;

    private void Start()
    {
        originPoint = basePoint.position;
        originRot = basePoint.rotation;
    }

    private void Update()
    {
        if (moveCameraTrigger)
        {
            isMoving = true;
            moveCameraTrigger = false;
        }

        if(isMoving)
        {
            MoveCamToPoint(destinationToFollow);
        }
    }

    public void TriggerCamera(Transform destination)
    {
        if(!moveCameraTrigger && !isMoving)
        {
            if (destination != destinationToFollow)
            {
                moveCameraTrigger = true;

                destinationToFollow = destination;
            }
        }
    }

    private void MoveCamToPoint(Transform destination)
    {
        if (tempDelay <= speed)
        {
            float ratio = tempDelay / speed;

            finalPos = Vector3.Lerp(originPoint, destination.position, ratio);
            finalRot = Quaternion.Lerp(originRot, destination.rotation, ratio);

            transform.position = finalPos;
            transform.rotation = finalRot;

            tempDelay += Time.deltaTime;
        }

        else
        {
            tempDelay = 0;
            originPoint = transform.position;
            originRot = transform.rotation;
            isMoving = false;
        }
    }


}
