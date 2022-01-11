using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* GameOver Screen Script
 * Created by: Dereck Mills
 * 
 * This script will handle the logic that operates the GameOver Screen.
 /**/

public class GameOverScreen : MonoBehaviour
{
    public Image
        background;

    public Text
        player1Score,
        player2Score,
        victoryBanner;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        //Reload the current scene
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMain()
    {
        //Load the active Scene
        SceneManager.LoadSceneAsync(0);
    }

    public void GameOver(List<int> playerScores, Color winner)
    {
        player1Score.text = playerScores[0].ToString();
        player2Score.text = playerScores[1].ToString();
        background.color = new Color(winner.r, winner.g, winner.b, background.color.a);

        if (playerScores[0] > playerScores[1])
            victoryBanner.text = "Player 1 is the Winner!";
        else if (playerScores[1] > playerScores[0])
            victoryBanner.text = "Player 2 is the Winner!";
        else
            victoryBanner.text = "The game results in a Tie!";
    }
}
