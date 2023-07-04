using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void SetTargets()
    {
        List<Vector2> directions = new List<Vector2>() { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1),
                                                         new(0, 1), new(0, -1), new(-1, 0),  new(1, 0) };

        MoveTargets = new List<Vector2>();
        AttackTargets = new List<Vector2>();

        foreach (var dir in directions)
        {
            for (int i = 1; i <= 7; i++)
            {

                if (!IsTargetOnGrid(dir * i))
                    break;

                if (IsTargetFree(dir * i))
                {
                    MoveTargets.Add(dir * i);
                }

                else if (IsTargetEnemy(dir * i))
                {
                    AttackTargets.Add(dir * i);
                    break;
                }
                else
                {
                    break;
                }

            }
        }
    }
}
