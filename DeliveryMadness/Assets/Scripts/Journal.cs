using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    [HideInInspector]
    public Vector3 directionToTake;
    public float rotationSpeed;
    public float duration = 3f;
    public GameObject vfx;
    private Vector3 rotationAngle;
    private bool hit = false;

    private void Start()
    {
        transform.parent = GameManager.instance.level.transform;
        Vector3 finalDirection = new Vector3(directionToTake.x, directionToTake.y + .2f, directionToTake.z);
        rotationAngle = new Vector3(0, 1, 0.5f);
        GetComponent<Rigidbody>().velocity = finalDirection.normalized * 20;
    }

    private void Update()
    {

        duration -= Time.deltaTime;
        transform.Rotate(rotationAngle * rotationSpeed);

        if (duration <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        vfx.SetActive(true);

        if (!collision.gameObject.CompareTag("Window"))
        {
            if(!hit)
            {
            GameManager.instance.audioManager.PlaySound("thud");
            }
        }

        hit = true;
    }
}
