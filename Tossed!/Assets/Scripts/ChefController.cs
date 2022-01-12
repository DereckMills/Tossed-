using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        _choppingTime = 1,
        _timeLimit = 150,
        _speedBonusTime = 10;
    [HideInInspector]
    public GameObject
        _interactable,
        _bowl;
    public SaladInventory.Ingredient[]
        _collected = { SaladInventory.Ingredient.Empty, SaladInventory.Ingredient.Empty };

    public SpriteRenderer
        _mainInventory,
        _secondaryInventory;

    public Slider
        _progressBar;
    public Text
        _infoPanel;

    //Private Variables
    float
        yMove = 0,
        xMove = 0,
        chopTimeRemaining = 0,
        speedBonus,
        speedBonusRemaining;

    Rigidbody2D
        _collider;
    bool
        isCarrying,
        isChopping,
        isTimeOut,
        isSpeed;


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

    public bool TimeOut
    {
        get { return isTimeOut; }
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

        _progressBar.gameObject.SetActive(false);
        _collider = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update the UI
        if (!isTimeOut)
        {
            _timeLimit -= Time.deltaTime;
            if (_timeLimit <= 0)
            {
                _timeLimit = 0;
                isTimeOut = true;
            }
        }
        _infoPanel.text = "Time Remaining: " + _timeLimit.ToString("f0") + "\nScore: " + PointManager.points.PointRequest(_playerID);
        //If the player is not actively chopping..
        if (!isChopping && !isTimeOut)
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
                if (!isSpeed)
                {
                    //_collider.position += new Vector2(xMove, yMove) * _speed * Time.deltaTime;
                    _collider.MovePosition(_collider.position + new Vector2(xMove, yMove) * _speed * Time.deltaTime);
                }
                else
                {
                    //_collider.position += new Vector2(xMove, yMove) * _speed * Time.deltaTime * speedBonus;
                    _collider.MovePosition(_collider.position + new Vector2(xMove, yMove) * _speed * Time.deltaTime * speedBonus);
                }
            }
            yMove = xMove = 0;
        }
    }

    //Coroutine to control the chop time at the Chopping table
    public IEnumerator ChopTime()
    {
        if (!isChopping)
        {
            _progressBar.gameObject.SetActive(true);
            _progressBar.value = 0;
            isChopping = true;
            for (chopTimeRemaining = _choppingTime; chopTimeRemaining > 0; chopTimeRemaining -= Time.deltaTime)
            {
                _progressBar.value = (_choppingTime - chopTimeRemaining) / _choppingTime;
                yield return null;
            }
            isChopping = false;
            _progressBar.gameObject.SetActive(false);
            if (_interactable)
            {
                _interactable.GetComponent<CuttingBoard>().Chopped();
            }
        }
    }

    //Coroutine to control the chop time at the Chopping table
    public IEnumerator SpeedBonus(float bonus)
    {
        if (isSpeed)
        {
            speedBonusRemaining += _speedBonusTime;
        }
        if (!isSpeed)
        {
            isSpeed = true;
            speedBonus = bonus;
            for (speedBonusRemaining = _speedBonusTime; speedBonusRemaining > 0; speedBonusRemaining -= Time.deltaTime)
            {
                yield return null;
            }
            isSpeed = false;
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
