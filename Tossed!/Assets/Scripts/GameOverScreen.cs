using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

/* GameOver Screen Script
 * Created by: Dereck Mills
 * 
 * This script will handle the logic that operates the GameOver Screen.
 /**/

public class GameOverScreen : MonoBehaviour
{
    public Image
        background;

    public string
        _fileLocation = "Assets/scores.txt";

    public int
        scoresToSave = 10;

    public Text
        victoryBanner,
        highScorePanel1,
        highScorePanel2;

    // Start is called before the first frame update
    void Start()
    {
        if(!new FileInfo(_fileLocation).Exists)
        {
            string newSave = "";
            for (int i = 0; i < scoresToSave; i++)
                newSave += "0\n";
            newSave = newSave.Substring(0, newSave.Length - 1);
            File.WriteAllText(_fileLocation, newSave);
        }

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
        StreamReader reader = new StreamReader(_fileLocation);

        List<string> scores = new List<string>();
        while (!reader.EndOfStream)
        {
            scores.Add(reader.ReadLine());
        }
        reader.Close();
        CheckScores(playerScores[0], scores);
        CheckScores(playerScores[1], scores);

        string panel1 = "";
        string panel2 = "";

        for (int i = 0; i < scores.Count / 2; i++)
            panel1 += scores[i] + "\n";

        for (int i = scores.Count / 2; i < scores.Count; i++)
            panel2 += scores[i] + "\n";

        highScorePanel1.text = panel1;
        highScorePanel2.text = panel2;
        background.color = new Color(winner.r, winner.g, winner.b, background.color.a);

        if (playerScores[0] > playerScores[1])
            victoryBanner.text = "Player 1 is the Winner with " + playerScores[0] + " points!";
        else if (playerScores[1] > playerScores[0])
            victoryBanner.text = "Player 2 is the Winner with " + playerScores[1] + " points!";
        else
            victoryBanner.text = "The game results in a Tie!";

        StreamWriter writer = new StreamWriter(_fileLocation);
        for (int i = 0; i < scores.Count; i++)
        {
            writer.WriteLine(scores[i]);
        }
        writer.Close();
    }

    void CheckScores(int newScore, List<string> oldScores)
    {
        int tempScore = newScore;
        for (int i = 0; i < oldScores.Count; i++)
        {
            if (newScore > int.Parse(oldScores[i]))
            {
                tempScore = int.Parse(oldScores[i]);
                oldScores[i] = newScore.ToString();
                newScore = tempScore;
            }
        }
    }
}
