using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Piece : MonoBehaviour
{
    public Vector2 Position { get; set; }
    public List<Vector2> MoveTargets { get; set; }
    public List<Vector2> AttackTargets { get; set; }

    public bool IsWhite;
    public bool Selected = false;
    public Sprite BlackVersion;

    public ChessGrid Grid;

    public virtual void SetTargets()
    {

    }

    public virtual void Attack(ChessTile target)
    {
        DestroyImmediate(target.PieceLocation.GetComponentInChildren<Piece>().gameObject);
        Move(target);
    }
    public virtual void Move(ChessTile target)
    {
        Position = target.Position;
        transform.parent = target.PieceLocation.transform;
        transform.position = target.transform.position;
    }

    public bool PerformAction(ChessTile target)
    {
        foreach (var pawn in Grid.Pawns)
        {
            pawn.HasJumped = false;
        }
       
        if (AttackTargets.Contains(target.Position - Position))
        {
            Attack(target);
            return true;
        }
        else if (MoveTargets.Contains(target.Position - Position))
        {
            Move(target);
            return true;
        }
        return false;
    }
    protected bool IsTargetOnGrid(Vector2 target)
    {
        return Grid.Tiles.ContainsKey(Position + target);
    }
    protected bool IsTargetFree(Vector2 target)
    {
        if (Grid.Tiles[Position + target].PieceLocation.GetComponentInChildren<Piece>() == null)
        {
            return true;
        }

        return false;
    }
    protected bool IsTargetEnemy(Vector2 target)
    {
        if (Grid.Tiles[Position + target].PieceLocation.GetComponentInChildren<Piece>().IsWhite == !IsWhite)
        {
            return true;
        }

        return false;
    }
}
