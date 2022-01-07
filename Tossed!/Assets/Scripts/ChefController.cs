using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Player Control Script
 * Created by: Dereck Mills
 * 
 * This script will contain all of the code required to control the two(2) player characters
 * in Tossed! 
 * 
 * Requirements: Input Theme struct must be set in Inspector before first play.
 /**/

public class ChefController : MonoBehaviour
{
    //Public Variables
    public InputTheme _inputs;
    public float _speed = 5;

    //Private Variables
    float yMove = 0, xMove = 0;
    [SerializeField]
    int _playerID = 0;

    //Accessors
    public int PlayerID
    {
        get { return _playerID; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //If the game would start without Input Keys being assigned...
        if (_inputs._up == KeyCode.None)
        {
            //Apply Arrow Key controls for the second Player 
            if (_playerID == 1)
            {
                _inputs = new InputTheme(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Alpha0);
            }
            //Apply WASD controls for all other instances
            else
            {
                _inputs = new InputTheme(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Space);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vertical movement controlled by the KeyCodes saved in the player's InputTheme struct.
        if (Input.GetKey(_inputs._up))
        {
            yMove = 1;
        }
        else if (Input.GetKey(_inputs._down))
        {
            yMove = -1;
        }

        //Horizontal movement controlled by the KeyCodes saved in the player's InputTheme struct.
        if (Input.GetKey(_inputs._right))
        {
            xMove = 1;
        }
        else if (Input.GetKey(_inputs._left))
        {
            xMove = -1;
        }

        //Apply the movement to the player's position if one of the inputs are pressed.
        if (yMove != 0 || xMove != 0)
        {
            transform.position += new Vector3(xMove, yMove) * _speed * Time.deltaTime;
        }
        yMove = xMove = 0;
    }
}

[System.Serializable]
public struct InputTheme
{
    //KeyCodes that represent the buttons the players will hit in order to control their characters.
    public KeyCode
        _up,
        _down,
        _left,
        _right,
        _interact;

    //Contructor method to allow the editting of the struct for custom keybindings.
    public InputTheme(KeyCode up, KeyCode down, KeyCode left, KeyCode right, KeyCode interact)
    {
        _up = up;
        _down = down;
        _right = right;
        _left = left;
        _interact = interact;
    }
}
