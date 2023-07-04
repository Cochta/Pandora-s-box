using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Pawn : Piece
{
    private bool IsFirstMove = true;
    public bool HasJumped = false;

    public override void SetTargets()
    {
        int side = IsWhite ? 1 : -1;
        var tempMoveTargets = new List<Vector2>() { new(0, side) };
        if (IsFirstMove && IsTargetFree(new(0, side * 2)) && IsTargetFree(new(0, side)))
        {
            tempMoveTargets.Add(new(0, side * 2));
        }

        MoveTargets = new List<Vector2>();
        foreach (var target in tempMoveTargets)
        {
            if (IsTargetOnGrid(target))
            {
                if (IsTargetFree(target))
                {
                    MoveTargets.Add(target);
                }
            }
        }

        var tempAttackTargets = new List<Vector2>() { new(1, side), new(-1, side) };
        AttackTargets = new List<Vector2>();
        foreach (var target in tempAttackTargets)
        {
            if (IsTargetOnGrid(target))
            {
                if (!IsTargetFree(target))
                {
                    if (IsTargetEnemy(target))
                    {
                        AttackTargets.Add(target);
                    }
                }
            }
        }

        List<Vector2> enPassantTargets = new List<Vector2>() { new(1, 0), new(-1, 0) };
        foreach (var target in enPassantTargets)
        {
            if (IsTargetOnGrid(target))
            {
                if (!IsTargetFree(target))
                {
                    if (IsTargetEnemy(target))
                    {
                        if (Grid.Tiles[Position + target].PieceLocation.GetComponentInChildren<Piece>().GetType() == typeof(Pawn))
                        {
                            if (Grid.Tiles[Position + target].PieceLocation.GetComponentInChildren<Pawn>().HasJumped)
                            {
                                AttackTargets.Add(new Vector2(target.x, IsWhite ? 1 : -1));
                            }
                        }
                    }
                }
            }
        }
    }

    public override void Attack(ChessTile target)
    {

        if (target.PieceLocation.GetComponentInChildren<Piece>() == null)
        {
            DestroyImmediate(Grid.Tiles[new Vector2(target.Position.x, target.Position.y + (IsWhite ? -1 : 1))].PieceLocation.GetComponentInChildren<Piece>().gameObject);
        }
        else
        {
            DestroyImmediate(target.PieceLocation.GetComponentInChildren<Piece>().gameObject);
        }
        Move(target);
    }

    public override void Move(ChessTile target)
    {
        IsFirstMove = false;
        if (target.Position - Position == new Vector2(0, 2) || target.Position - Position == new Vector2(0, -2))
        {
            HasJumped = true;
        }
        base.Move(target);
    }
}
