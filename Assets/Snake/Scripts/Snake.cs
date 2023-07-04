using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public Vector2 Position;
    public Vector2 LastPosition;
    public void SetPosition()
    {
        Position = GetComponentInParent<SnakeTile>().Position;
    }
}
