using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SnakeGrid : MonoBehaviour
{
    [SerializeField] private SnakeTile _snakeTilePrefab;
    [SerializeField] private GameObject _snakePrefab;
    [SerializeField] private GameObject _applePrefab;

    private Apple _apple;

    private Snake _snakeHead;
    public List<Snake> _snakeBody;
    private GameObject _grid;

    private Dictionary<Vector2, SnakeTile> _tiles;

    private float _time = 0;

    private Vector2 _direction = Vector2.up;

    private void Start()
    {
        GenerateGrid(25, 25);
        SpawnSnake(new Vector2(12, 5));
        SpawnApple();
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (Input.GetKeyDown((KeyCode.UpArrow)) && _direction != Vector2.down)
            _direction = Vector2.up;
        if (Input.GetKeyDown((KeyCode.DownArrow)) && _direction != Vector2.up)
            _direction = Vector2.down;
        if (Input.GetKeyDown((KeyCode.RightArrow)) && _direction != Vector2.left)
            _direction = Vector2.right;
        if (Input.GetKeyDown((KeyCode.LeftArrow)) && _direction != Vector2.right)
            _direction = Vector2.left;

        if (_time >= 0.1f)
        {
            SnakeMove();
            _time = 0;
        }
    }

    private void GenerateGrid(int width, int height)
    {
        _grid = new GameObject();
        _grid.name = "SnakeGrid";
        _grid.transform.parent = transform;

        _tiles = new Dictionary<Vector2, SnakeTile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SnakeTile tile = Instantiate(_snakeTilePrefab, new Vector3((x - width / 2f) + 0.5f, (y - height / 2f) - 1f), Quaternion.identity);
                tile.name = $"MSTile{x}{y}";
                tile.Position = new Vector2(x, y);

                tile.transform.parent = _grid.transform;

                _tiles[new Vector2(x, y)] = tile;
            }
        }

        _grid.transform.localScale = new Vector3(0.35f, 0.35f);
    }
    private void SpawnSnake(Vector2 position)
    {
        _snakeBody = new List<Snake>();
        _snakeHead = Instantiate(_snakePrefab, _tiles[position].transform).GetComponent<Snake>();
        _snakeHead.name = $"Snake{0}";
        _snakeHead.SetPosition();
        _snakeBody.Add(_snakeHead);
        GrowSnake();
        GrowSnake();
    }

    private void GrowSnake()
    {
        var part = Instantiate(_snakePrefab, _tiles[_snakeBody[_snakeBody.Count - 1].Position].transform).GetComponent<Snake>();
        part.name = $"Snake{(_snakeBody.Count - 1)}";
        part.SetPosition();
        _snakeBody.Add(part);
    }

    private void SnakeMove()
    {
        SnakeTile value;
        if (!_tiles.TryGetValue(_snakeHead.Position + _direction, out value))
        {
            Debug.Log("Perdu");
            return;
        }

        for (int i = 0; i < _snakeBody.Count; i++)
        {
            if (i == 0)
            {
                if (_tiles[_snakeHead.Position + _direction].GetComponentInChildren<Snake>() != null)
                {
                    Debug.Log("Perdu");
                }
                _snakeHead.LastPosition = _snakeHead.Position;
                _snakeHead.transform.parent = _tiles[_snakeHead.Position + _direction].transform;

                if (_apple.Position == _snakeHead.Position)
                {
                    GrowSnake();
                    Destroy(_apple.gameObject);
                    SpawnApple();
                }
            }
            else
            {
                _snakeBody[i].LastPosition = _snakeBody[i].Position;
                _snakeBody[i].transform.parent = _tiles[_snakeBody[i - 1].LastPosition].transform;
            }
            _snakeBody[i].transform.position = _snakeBody[i].transform.parent.position;
            _snakeBody[i].SetPosition();
        }
    }

    private void SpawnApple()
    {
        System.Random rnd = new System.Random();
        var tile = _tiles[new Vector2(rnd.Next(0, 24), rnd.Next(0, 24))];
        _apple = Instantiate(_applePrefab, tile.transform).GetComponent<Apple>();
        _apple.transform.parent = tile.transform;
        _apple.SetPosition();
    }
}
