using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    private int lastPlayed;

    void Start()
    {
        Medium();
    }
    public void NewGame()
    {
        if (lastPlayed == 1)
            Easy();
        else if (lastPlayed == 2)
            Medium();
        else if (lastPlayed == 3)
            Expert();
    }
    public void Easy()
    {
        _grid.NewGame(8, 8);
        _grid.NBBombs = 10;
        lastPlayed = 1;
    }
    public void Medium()
    {
        _grid.NewGame(16, 16);
        _grid.NBBombs = 40;
        lastPlayed = 2;
    }
    public void Expert()
    {
        _grid.NewGame(32, 16);
        _grid.NBBombs = 99;
        lastPlayed = 3;
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
