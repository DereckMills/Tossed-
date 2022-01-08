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
    public InputTheme 
        _inputs;
    public float 
        _speed = 5;
    [HideInInspector]
    public GameObject 
        _interactable,
        _bowl;
    public SaladInventory.Ingredient[] 
        _collected = { SaladInventory.Ingredient.Empty, SaladInventory.Ingredient.Empty};

    public SpriteRenderer 
        mainInventory,
        secondaryInventory;

    //Private Variables
    float 
        yMove = 0, 
        xMove = 0;
    Rigidbody2D 
        _collider;
    bool
        isCarrying;


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

        _collider = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_inputs._interact) && _interactable)
        {
            _interactable.SendMessage("Interact", this);
        }
    }

    private void LateUpdate()
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
            //_collider.position += new Vector2(xMove, yMove) * _speed * Time.deltaTime;
            _collider.MovePosition(_collider.position + new Vector2(xMove, yMove) * _speed * Time.deltaTime);
        }
        yMove = xMove = 0;
    }

    public void UpdateInventory()
    {
        mainInventory.color = SaladInventory.saladLogic.ingredientColors[(int)_collected[0]];
        secondaryInventory.color = SaladInventory.saladLogic.ingredientColors[(int)_collected[1]];
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
