using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MSTile : MonoBehaviour
{
    [SerializeField] private Sprite _revealed;
    [SerializeField] private Sprite _bomb;
    [SerializeField] private Sprite _flag;
    [SerializeField] private Sprite _questionMark;

    [SerializeField] private SpriteRenderer _tileSR;
    [SerializeField] private SpriteRenderer _symbolSR;
    public TextMeshPro _numberText;

    public BoxCollider2D _col;

    public bool IsFirstClick = true;

    public bool HasBomb = false;

    public bool IsFlaged = false;
    public bool IsQuestionMarked = false;

    public bool IsRevealed = false;

    public List<MSTile> NearbyTiles;
    public int NearbyBombs;

    public Vector2 Position;

    public void Reveal()
    {
        _symbolSR.sprite = null;
        IsRevealed = true;
        _tileSR.sprite = _revealed;

        if (IsFirstClick)
        {
            HasBomb = false;
            var grid = transform.parent.GetComponentInParent<MSGrid>();
            grid.GetNearbyTiles();
            grid.TilesListToBomb.Remove(this);
            grid.IsGameStart = true;
            foreach (MSTile tile in grid.TilesListToBomb)
            {
                tile.IsFirstClick = false;
            }
            foreach (MSTile tile in NearbyTiles)
            {
                HasBomb = false;
                grid.TilesListToBomb.Remove(tile);
            }
            grid.FillBombs(grid.NBBombs);
            grid.GetNearbyBombs();
        }

        if (HasBomb)
        {
            _symbolSR.sprite = _bomb;
            transform.parent.GetComponentInParent<MSGrid>().RevealAllBombs();

        }
        else if (NearbyBombs == 0)
        {
            RevealNearby();
            transform.parent.GetComponentInParent<MSGrid>().RevealedTiles++;
        }
        else
        {
            _numberText.text = NearbyBombs.ToString();
            SetColor();
            transform.parent.GetComponentInParent<MSGrid>().RevealedTiles++;
        }
    }

    private void SetColor()
    {
        if (NearbyBombs == 2)
            _numberText.color = Color.green;
        else if (NearbyBombs == 3)
            _numberText.color = Color.red;
        else if (NearbyBombs == 4)
            _numberText.color = Color.black;
        else if (NearbyBombs == 5)
            _numberText.color = Color.yellow;
        else if (NearbyBombs == 6)
            _numberText.color = Color.cyan;
        else if (NearbyBombs == 7)
            _numberText.color = Color.black;
    }

    private void RevealNearby()
    {
        foreach (var tile in NearbyTiles)
        {
            if (!tile.IsRevealed && !tile.IsFlaged)
            {
                tile.Reveal();
            }
        }
    }

    private int GetNearbyFlags()
    {
        int nbFlags = 0;
        foreach (var tile in NearbyTiles)
        {
            if (tile.IsFlaged)
                nbFlags++;
        }
        return nbFlags;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !IsFlaged)
        {
            if (HasBomb)
            {
                _tileSR.color = Color.red;
            }
            if (IsRevealed && GetNearbyFlags() == NearbyBombs)
                RevealNearby();
            else if (!IsRevealed)
                Reveal();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!IsFlaged && !IsQuestionMarked && !IsRevealed)
            {
                _symbolSR.sprite = _flag;
                IsFlaged = true;
                transform.parent.GetComponentInParent<MSGrid>().FlagedBombs++;

            }
            else if (IsFlaged)
            {
                _symbolSR.sprite = _questionMark;
                IsQuestionMarked = true;
                IsFlaged = false;
                transform.parent.GetComponentInParent<MSGrid>().FlagedBombs--;
            }
            else if (IsQuestionMarked)
            {
                _symbolSR.sprite = null;
                IsQuestionMarked = false;
            }
        }

    }
}
