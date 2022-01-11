using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Player Control Script
 * Created by: Dereck Mills
 * 
 * This script will contain all of the code required to control the two(2) player characters
 * in Tossed! 
 /**/

public class ChefController : MonoBehaviour
{
    //Public Variables
    public InputTheme
        _inputs;
    public float
        _speed = 5,
        _choppingTime = 1;
    [HideInInspector]
    public GameObject
        _interactable,
        _bowl;
    public SaladInventory.Ingredient[]
        _collected = { SaladInventory.Ingredient.Empty, SaladInventory.Ingredient.Empty };

    public SpriteRenderer
        _mainInventory,
        _secondaryInventory;

    //Private Variables
    float
        yMove = 0,
        xMove = 0,
        chopTimeRemaining = 0;
    Rigidbody2D
        _collider;
    bool
        isCarrying,
        isChopping;


    [SerializeField]
    int _playerID = 0;

    //Accessors
    public int PlayerID
    {
        get { return _playerID; }
    }

    public bool Carrying
    {
        set { isCarrying = value; }
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
                _inputs = new InputTheme(KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Alpha0, KeyCode.RightControl);
            }
            //Apply WASD controls for all other instances
            else
            {
                _inputs = new InputTheme(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Space, KeyCode.LeftShift);
            }
        }

        _collider = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is not actively chopping..
        if (!isChopping)
        {
            //Attempt to interact if there is an interactable object nearby
            if (Input.GetKeyDown(_inputs._interact) && _interactable)
            {
                _interactable.SendMessage("Interact", this);
            }
            //Attempt to swap items
            if (Input.GetKeyDown(_inputs._swap))
            {
                if (_collected[0] != SaladInventory.Ingredient.Empty && _collected[1] != SaladInventory.Ingredient.Empty)
                {
                    SaladInventory.Ingredient temp = _collected[1];
                    _collected[1] = _collected[0];
                    _collected[0] = temp;
                    UpdateInventory();
                }
            }
        
            _collider.velocity = Vector3.zero;
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
    }

    //Coroutine to control the chop time at the Chopping table
    public IEnumerator ChopTime()
    {
        if (!isChopping)
        {
            isChopping = true;
            for (chopTimeRemaining = _choppingTime; chopTimeRemaining > 0; chopTimeRemaining -= Time.deltaTime)
            {
                yield return null;
            }
            isChopping = false;
            if (_interactable)
            {
                _interactable.GetComponent<CuttingBoard>().Chopped();
            }
        }

    }

    //Foreach diplay item, change it to the color of the ingredient in that slot
    public void UpdateInventory()
    {
        _mainInventory.color = SaladInventory.saladLogic.ingredientColors[(int)_collected[0]];
        _secondaryInventory.color = SaladInventory.saladLogic.ingredientColors[(int)_collected[1]];
    }
}

//Data object to control the character's input controls
[System.Serializable]
public struct InputTheme
{
    //KeyCodes that represent the buttons the players will hit in order to control their characters.
    public KeyCode
        _up,
        _down,
        _left,
        _right,
        _interact,
        _swap;

    //Contructor method to allow the editting of the struct for custom keybindings.
    public InputTheme(KeyCode up, KeyCode down, KeyCode left, KeyCode right, KeyCode interact, KeyCode swap)
    {
        _up = up;
        _down = down;
        _right = right;
        _left = left;
        _interact = interact;
        _swap = swap;
    }
}
