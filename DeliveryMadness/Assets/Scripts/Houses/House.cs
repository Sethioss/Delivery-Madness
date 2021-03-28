using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Transform mailBoxPos;
    public int breakables;

    public enum color { red, yellow, blue };
    public color houseColor = color.red;

    private void Update()
    {

    }
}
