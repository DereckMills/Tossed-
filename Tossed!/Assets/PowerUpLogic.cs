using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PowerUp Script
 * Created by: Dereck Mills
 * 
 * This script will handle all of the logic and interactions with the player's PowerUps
 /**/

public class PowerUpLogic : MonoBehaviour
{
    public enum PowerUp { Speed, Points, Time}
    public PowerUp 
        type;

    //Public Variables
    public int 
        _playerID,
        _pointBonus = 25;

    public float
        _speedBonus = .5f,
        _timeBonus = 10;

    [SerializeField]
    SpriteRenderer 
        selector;
    [SerializeField]
    List<Color>
        powerUpColors = new List<Color>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Prep (int PlayerID, Color selectorColor, PowerUp newType)
    {
        _playerID = PlayerID;
        selector.color = selectorColor;
        type = newType;
        gameObject.GetComponent<SpriteRenderer>().color = powerUpColors[(int)type];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the Player is near the ingredient source, change the selector color to that player's color.
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<ChefController>().PlayerID == _playerID)
            {
                if (type == PowerUp.Points)
                    PointManager.points.AdjustPoints(_pointBonus, _playerID);
                else if (type == PowerUp.Time)
                    collision.GetComponent<ChefController>()._timeLimit += _timeBonus;
                else if (type == PowerUp.Speed)
                {
                    IEnumerator powerUp = collision.GetComponent<ChefController>().SpeedBonus(_speedBonus);
                    StartCoroutine(powerUp);
                }

                Destroy(this.gameObject);    
            }
        }
    }
}
