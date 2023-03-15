using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoseGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void ToMineSweeper()
    {
        SceneManager.LoadScene("MineSweeper");
    }

    // Update is called once per frame
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
