using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class KeepFromRotating : MonoBehaviour
{
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0.0f);
    }
}
