using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MSGrid : MonoBehaviour
{
    [SerializeField] private MSTile _msTilePrefab;
    private GameObject _grid;

    private Dictionary<Vector2, MSTile> _tiles;
    public List<MSTile> TilesListToBomb = new List<MSTile>();
    public List<MSTile> TilesList = new List<MSTile>();

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
            _bombNumberText.text = (NBBombs - FlagedBombs).ToString("000");
        }
        _timerText.text = _timer.ToString("000");

    }

    public void NewGame(int width, int height)
    {
        _bombNumberText.text = (NBBombs - FlagedBombs).ToString("000");
        IsGameStart = false;
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
        _grid.name = "MSGrid";
        _grid.transform.parent = transform;

        _tiles = new Dictionary<Vector2, MSTile>();
        TilesListToBomb = new List<MSTile>();
        TilesList = new List<MSTile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MSTile msTile = Instantiate(_msTilePrefab, new Vector3((x - width / 2f) + 0.5f, (y - height / 2f) - 1f), Quaternion.identity);
                msTile.name = $"MSTile{x}{y}";
                msTile.Position = new Vector2(x, y);

                msTile.transform.parent = _grid.transform;

                _tiles[new Vector2(x, y)] = msTile;
                TilesListToBomb.Add(msTile);
                TilesList.Add(msTile);
            }
        }
        _grid.transform.localScale = new Vector3(0.5f, 0.5f);
    }

    public void FillBombs(int nbBomb)
    {
        System.Random rnd = new System.Random();

        for (int i = 0; i < nbBomb; i++)
        {
            MSTile tempMsTile = TilesListToBomb[rnd.Next(0, TilesListToBomb.Count)];
            tempMsTile.HasBomb = true;
            TilesListToBomb.Remove(tempMsTile);
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
            MSTile value = tile; // envie de crever
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
