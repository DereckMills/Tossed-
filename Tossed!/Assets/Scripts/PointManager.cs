using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Point Manager Script
 * Created by: Dereck Mills
 * 
 * This script will handle and manage the point system between the two players. It also manages the Game Over State.
 /**/

public class PointManager : MonoBehaviour
{
    //Static Variables
    public static PointManager 
        points;

    //Public Variable
    public List<ChefController> 
        _players = new List<ChefController>();
    public bool
        _isGameOver;

    //Private Variables
    List<int>
        _pointRecord;

    [SerializeField]
    GameObject
        gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        //Apply Singleton logic to this script
        if (!points)
        {
            points = this;
        }
        else if (points != this)
        {
            Destroy(this.gameObject);
        }

        _pointRecord = new List<int>();
        foreach (ChefController player in _players)
            _pointRecord.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver)
        {
            //Check for GameOver
            bool gameOver = true;
            foreach (ChefController player in _players)
                if (!player.TimeOut)
                    gameOver = false;
            if (gameOver)
            {
                _isGameOver = true;
                Color winningColor = Color.grey;

                if (_pointRecord[0] > _pointRecord[1])
                    winningColor = _players[0].GetComponent<SpriteRenderer>().color;
                else if (_pointRecord[1] > _pointRecord[0])
                    winningColor = _players[1].GetComponent<SpriteRenderer>().color;

                gameOverScreen.SetActive(true);
                gameOverScreen.GetComponent<GameOverScreen>().GameOver(_pointRecord, winningColor);
            }
        }
    }

    public int PointRequest(int playerID)
    {
        //If the playerID is valid, return the points assigned to that player
        if (playerID < _pointRecord.Count && playerID >= 0)
            return _pointRecord[playerID];
        //Otherwise return a default error
        return -9999;
    }

    public void AdjustPoints(int pointChange, int playerID)
    {
        Debug.Log("Point Change: Player " + playerID + " with " + pointChange);
        //If the playerID is negative, apply the point change to all players
        if (playerID < 0)
        {
            for (int i = 0; i < _pointRecord.Count; i++)
                _pointRecord[i] += pointChange;
        }
        //Otherwise, make sure the playerID is less than the number of players
        else if (playerID < _pointRecord.Count)
            _pointRecord[playerID] += pointChange;

    }
}
