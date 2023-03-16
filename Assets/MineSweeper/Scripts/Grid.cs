using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    private GameObject _grid;

    private Dictionary<Vector2, Tile> _tiles;
    public List<Tile> TilesListToBomb = new List<Tile>();
    public List<Tile> TilesList = new List<Tile>();

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _bombNumberText;

    public int NBBombs;

    public int FlagedBombs;
    private int size;

    public int RevealedTiles;

    public bool IsGameStart = false;

    private float _timer;

    private void Update()
    {
        if (RevealedTiles == size - NBBombs)
        {
            foreach (var tile in TilesList)
            {
                tile._col.enabled = false;
            }
            IsGameStart = false;
            _bombNumberText.text = "You \n Win !";
        }
        if (IsGameStart)
        {
            _timer += Time.deltaTime;
            _timerText.text = _timer.ToString("000");
            _bombNumberText.text = (NBBombs - FlagedBombs).ToString("000");
        }
    }

    public void NewGame(int width, int height)
    {
        _timer = 0;
        FlagedBombs = 0;
        RevealedTiles = 0;
        size = width * height;
        if (_grid != null)
        {
            Destroy(_grid);
        }

        GenerateGrid(width, height);
    }

    private void GenerateGrid(int width, int height)
    {
        _grid = new GameObject();
        _grid.name = "Grid";
        _grid.transform.parent = transform;

        _tiles = new Dictionary<Vector2, Tile>();
        TilesListToBomb = new List<Tile>();
        TilesList = new List<Tile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = Instantiate(_tilePrefab, new Vector3((x - width / 2f) + 0.5f, (y - height / 2f) - 1f), Quaternion.identity);
                tile.name = $"Tile{x}{y}";
                tile.Position = new Vector2(x, y);

                tile.transform.parent = _grid.transform;

                _tiles[new Vector2(x, y)] = tile;
                TilesListToBomb.Add(tile);
                TilesList.Add(tile);
            }
        }
        _grid.transform.localScale = new Vector3(0.5f, 0.5f);
    }

    public void FillBombs(int nbBomb)
    {
        System.Random rnd = new System.Random();

        for (int i = 0; i < nbBomb; i++)
        {
            Tile tempTile = TilesListToBomb[rnd.Next(0, TilesListToBomb.Count)];
            tempTile.HasBomb = true;
            TilesListToBomb.Remove(tempTile);
        }
    }

    public void GetNearbyTiles()
    {
        List<Vector2> targets = new List<Vector2>();
        targets.Add(new Vector2(1, 1));
        targets.Add(new Vector2(1, 0));
        targets.Add(new Vector2(1, -1));
        targets.Add(new Vector2(-1, -1));
        targets.Add(new Vector2(-1, 0));
        targets.Add(new Vector2(-1, 1));
        targets.Add(new Vector2(0, 1));
        targets.Add(new Vector2(0, -1));

        foreach (var tile in TilesListToBomb)
        {
            Tile value = tile; // envie de crever
            foreach (var target in targets)
            {
                if (_tiles.TryGetValue(tile.Position + target, out value))
                {
                    tile.NearbyTiles.Add(_tiles[tile.Position + target]);
                }
            }
        }
    }

    public void GetNearbyBombs()
    {

        foreach (var tile in TilesList)
        {
            foreach (var nearbytile in tile.NearbyTiles)
            {
                if (nearbytile.HasBomb)
                {
                    tile.NearbyBombs++;
                }
            }
        }
    }

    public void RevealAllBombs()
    {
        IsGameStart = false;
        _bombNumberText.text = "You \n Lose !";
        foreach (var tile in TilesList)
        {
            if (!tile.HasBomb && tile.IsFlaged)
            {
                tile._numberText.color = Color.red;
                tile._numberText.text = "X";
                tile._numberText.fontSize = 15;
            }

            if (tile.HasBomb && tile.IsFlaged)
            {

            }
            else if (tile.HasBomb && !tile.IsRevealed)
            {
                tile.Reveal();
            }
            tile._col.enabled = false;
        }
    }
}
