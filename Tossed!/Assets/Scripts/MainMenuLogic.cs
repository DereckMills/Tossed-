using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

/* MainMenu Script
 * Created by: Dereck Mills
 * 
 * This script will handle the handling of the Main Menu of the game
 /**/

public class MainMenuLogic : MonoBehaviour
{
    public GameObject
        _instructionPanel,
        _highScorePanel;

    public Text
        _highScoreDiplay;

    public string
        _fileLocation = "Assets/scores.txt";

    public int
        _scoresToSave = 10;

    // Start is called before the first frame update
    void Start()
    {
        if (!new FileInfo(_fileLocation).Exists)
        {
            string newSave = "";
            for (int i = 0; i < _scoresToSave; i++)
                newSave += "0\n";
            newSave = newSave.Substring(0, newSave.Length - 1);
            File.WriteAllText(_fileLocation, newSave);
        }

        string scores = "";
        StreamReader reader = new StreamReader(_fileLocation);

        while (!reader.EndOfStream)
        {
            scores += reader.ReadLine() + "\n";
        }
        reader.Close();

        scores = scores.Substring(0, scores.Length - 1);
        _highScoreDiplay.text = scores;

        _instructionPanel.SetActive(false);
        _highScorePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        //Reload the current scene
        SceneManager.LoadSceneAsync(1);
    }

    public void InstructionToggle()
    {
        _instructionPanel.SetActive(!_instructionPanel.activeInHierarchy);
    }

    public void HighScorePanel()
    {
        _highScorePanel.SetActive(!_highScorePanel.activeInHierarchy);
    }
}
