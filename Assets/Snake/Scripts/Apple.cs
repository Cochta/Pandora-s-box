using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public Vector2 Position;
    public void SetPosition()
    {
        Position = GetComponentInParent<SnakeTile>().Position;
    }
}
