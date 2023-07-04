using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSUI : MonoBehaviour
{
    [SerializeField] private MSGrid _msGrid;

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
        _msGrid.NBBombs = 10;
        _msGrid.NewGame(8, 8);

        lastPlayed = 1;
    }
    public void Medium()
    {
        _msGrid.NBBombs = 40;
        _msGrid.NewGame(16, 16);
        lastPlayed = 2;
    }
    public void Expert()
    {
        _msGrid.NBBombs = 99;
        _msGrid.NewGame(32, 16);
        lastPlayed = 3;
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
