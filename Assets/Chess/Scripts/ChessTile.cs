using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessTile : MonoBehaviour
{
    private BoxCollider2D _col;
    public SpriteRenderer _sr;
    public GameObject PieceLocation;
    [SerializeField] private Color _colorOdd;
    [SerializeField] private Color _colorEven;
    [SerializeField] private Color _attackHighlightColor;
    [SerializeField] private Color _moveHighlightColor;
    [SerializeField] private Color _highlightColor;

    private Color _originColor;
    private Color _baseColor;

    public Vector2 Position;
    public ChessGrid Grid;

    public void Init(bool isOffset)
    {
        _col = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = isOffset ? _colorEven : _colorOdd;
        _originColor = _sr.color;
        Grid = GetComponentInParent<ChessGrid>();
    }

    public void OnMouseEnter()
    {
        if (_baseColor == _highlightColor)
        {
            _baseColor = _originColor;
        }
        _baseColor = _sr.color;
        Highlight();
        if (HasPiece())
        {
            if (IsTurn())
            {
                HighlightMove();
                HighlightAttack();
            }
        }
    }
    public void OnMouseDown()
    {
        Grid.CancelHighlight();

        bool HasPerformed = false;
        if (Grid.SelectedPiece != null)
        {
            HasPerformed = Grid.SelectedPiece.PerformAction(this);
        }
        if (HasPerformed)
        {
            Grid.IsWhiteTurn = !Grid.IsWhiteTurn;
            _baseColor = _originColor;
            _sr.color = _originColor;
        }
        Grid.DeSelectPieces();
        Grid.SelectedPiece = null;

        if (HasPiece() && !HasPerformed)
        {
            if (IsTurn())
            {
                Piece piece = PieceLocation.GetComponentInChildren<Piece>();
                piece.Selected = true;
                Grid.SelectedPiece = piece;
            }
            OnMouseEnter();
        }
        Grid.SetWhiteTargets();
        Grid.SetBlackTargets();
    }
    public void OnMouseExit()
    {
        CancelHighlight();

        _sr.color = _baseColor;
    }
    private void HighlightMove()
    {
        Piece piece = PieceLocation.GetComponentInChildren<Piece>();
        foreach (var target in piece.MoveTargets)
        {
            Grid.Tiles[Position + target]._sr.color = _moveHighlightColor;
        }
    }
    private void HighlightAttack()
    {
        Piece piece = PieceLocation.GetComponentInChildren<Piece>();
        foreach (var target in piece.AttackTargets)
        {
            Grid.Tiles[Position + target]._sr.color = _attackHighlightColor;
        }
    }
    public void CancelHighlight()
    {
        bool selected = false;
        if (HasPiece())
        {
            Piece piece = PieceLocation.GetComponentInChildren<Piece>();
            if (!piece.Selected)
            {
                foreach (var target in piece.MoveTargets)
                {
                    Grid.Tiles[Position + target]._sr.color = Grid.Tiles[Position + target]._originColor;
                }
                foreach (var target in piece.AttackTargets)
                {
                    Grid.Tiles[Position + target]._sr.color = Grid.Tiles[Position + target]._originColor;
                }
            }
            else
            {
                selected = true;
            }
        }

        if (!selected)
            _sr.color = _originColor;
    }
    private void Highlight()
    {
        _sr.color = _highlightColor;
    }
    private bool HasPiece()
    {
        return PieceLocation.GetComponentInChildren<Piece>() != null;
    }
    private bool IsTurn()
    {
        return PieceLocation.GetComponentInChildren<Piece>().IsWhite == Grid.IsWhiteTurn;
    }
}
