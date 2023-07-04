using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override void SetTargets()
    {
        List<Vector2> targets = new List<Vector2>() { new(-1, 2), new(1, 2), new(-1, -2), new(1, -2),
                                                      new(2, 1),  new(2, -1),new(-2, 1),  new(-2, -1) };
        MoveTargets = new List<Vector2>();
        AttackTargets = new List<Vector2>();

        foreach (var target in targets)
        {
            if (!IsTargetOnGrid(target))
                continue;
            if (IsTargetFree(target))
            {
                MoveTargets.Add(target);
            }
            else if (IsTargetEnemy(target))
            {
                AttackTargets.Add(target);
            }
        }
    }
}
