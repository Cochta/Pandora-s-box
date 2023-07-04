using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGrid : MonoBehaviour
{

    private int _width = 8, _height = 8;
    [SerializeField] private ChessTile _tilePrefab;
    [SerializeField] private GameObject _pawnPrefab;
    [SerializeField] private GameObject _knightPrefab;
    [SerializeField] private GameObject _bishopPrefab;
    [SerializeField] private GameObject _rookPrefab;
    [SerializeField] private GameObject _kingPrefab;
    [SerializeField] private GameObject _queenPrefab;

    public Dictionary<Vector2, ChessTile> Tiles;

    public List<Piece> WhitePieces;
    public List<Piece> BlackPieces;
    public List<Pawn> Pawns = new List<Pawn>();

    public Piece SelectedPiece = null;
    public bool IsWhiteTurn = true;

    private void Start()
    {
        GenerateGrid();
    }
    public void GenerateGrid()
    {
        Tiles = new Dictionary<Vector2, ChessTile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                ChessTile tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                tile.name = $"Tile{x}{y}";
                tile.Position = new Vector2(x, y);
                tile.transform.parent = transform;

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (y % 2 == 0 && x % 2 != 0);
                tile.Init(isOffset);

                Tiles[new Vector2(x, y)] = tile;
            }
        }
        transform.position = new Vector3(-3.5f, -3.5f, 1);

        SpawnWhitePieces();
        SpawnBlackPieces();
        SetWhiteTargets();
        SetBlackTargets();
    }

    public void SpawnWhitePieces()
    {
        WhitePieces = new List<Piece>();
        for (int i = 0; i < _width; i++)
        {
            Pawn p = Instantiate(_pawnPrefab, Tiles[new(i, 1)].PieceLocation.transform).GetComponent<Pawn>();
            WhitePieces.Add(p);
            Pawns.Add(p);
        }

        WhitePieces.Add(Instantiate(_bishopPrefab, Tiles[new(2, 0)].PieceLocation.transform).GetComponent<Piece>());
        WhitePieces.Add(Instantiate(_bishopPrefab, Tiles[new(5, 0)].PieceLocation.transform).GetComponent<Piece>());

        WhitePieces.Add(Instantiate(_knightPrefab, Tiles[new(1, 0)].PieceLocation.transform).GetComponent<Piece>());
        WhitePieces.Add(Instantiate(_knightPrefab, Tiles[new(6, 0)].PieceLocation.transform).GetComponent<Piece>());

        WhitePieces.Add(Instantiate(_rookPrefab, Tiles[new(0, 0)].PieceLocation.transform).GetComponent<Piece>());
        WhitePieces.Add(Instantiate(_rookPrefab, Tiles[new(7, 0)].PieceLocation.transform).GetComponent<Piece>());

        WhitePieces.Add(Instantiate(_queenPrefab, Tiles[new(4, 0)].PieceLocation.transform).GetComponent<Piece>());

        WhitePieces.Add(Instantiate(_kingPrefab, Tiles[new(3, 0)].PieceLocation.transform).GetComponent<Piece>());

        foreach (var piece in WhitePieces)
        {
            piece.Grid = this;
            piece.IsWhite = true;
            piece.Position = piece.transform.parent.GetComponentInParent<ChessTile>().Position;
        }
    }
    public void SpawnBlackPieces()
    {
        BlackPieces = new List<Piece>();
        for (int i = 0; i < _width; i++)
        {
            Pawn p = Instantiate(_pawnPrefab, Tiles[new(i, 6)].PieceLocation.transform).GetComponent<Pawn>();
            BlackPieces.Add(p);
            Pawns.Add(p);
        }

        BlackPieces.Add(Instantiate(_bishopPrefab, Tiles[new(2, 7)].PieceLocation.transform).GetComponent<Piece>());
        BlackPieces.Add(Instantiate(_bishopPrefab, Tiles[new(5, 7)].PieceLocation.transform).GetComponent<Piece>());

        BlackPieces.Add(Instantiate(_knightPrefab, Tiles[new(6, 7)].PieceLocation.transform).GetComponent<Piece>());
        BlackPieces.Add(Instantiate(_knightPrefab, Tiles[new(1, 7)].PieceLocation.transform).GetComponent<Piece>());

        BlackPieces.Add(Instantiate(_rookPrefab, Tiles[new(0, 7)].PieceLocation.transform).GetComponent<Piece>());
        BlackPieces.Add(Instantiate(_rookPrefab, Tiles[new(7, 7)].PieceLocation.transform).GetComponent<Piece>());

        BlackPieces.Add(Instantiate(_queenPrefab, Tiles[new(4, 7)].PieceLocation.transform).GetComponent<Piece>());

        BlackPieces.Add(Instantiate(_kingPrefab, Tiles[new(3, 7)].PieceLocation.transform).GetComponent<Piece>());

        foreach (var piece in BlackPieces)
        {
            piece.Grid = this;
            piece.IsWhite = false;
            //piece.transform.Rotate(new(180, 0));
            piece.GetComponent<SpriteRenderer>().sprite = piece.BlackVersion;
            piece.Position = piece.transform.parent.GetComponentInParent<ChessTile>().Position;
        }
    }

    public void SetWhiteTargets()
    {
        foreach (var piece in WhitePieces)
        {
            piece.SetTargets();
        }
    }

    public void SetBlackTargets()
    {
        foreach (var piece in BlackPieces)
        {
            piece.SetTargets();
        }
    }

    public void DeSelectPieces()
    {
        foreach (var piece in WhitePieces)
        {
            piece.Selected = false;
        }
        foreach (var piece in BlackPieces)
        {
            piece.Selected = false;
        }
    }

    public void CancelHighlight()
    {
        foreach (var tile in Tiles)
        {
            tile.Value.CancelHighlight();
        }
    }
}
