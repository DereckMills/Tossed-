using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Cutting Board Script
 * Created by: Dereck Mills
 * 
 * This script will add the logic to the Cutting board
 * where the player will add ingredients and then chop them.
 /**/

public class CuttingBoard : MonoBehaviour
{
    //Public Variables
    public int
        _playerID = 0;
    public Sprite
        _rawItem,
        _choppedItem;
    public SaladInventory.Ingredient[]
        _bowl = { SaladInventory.Ingredient.Empty, SaladInventory.Ingredient.Empty, SaladInventory.Ingredient.Empty };

    //Private Variables
    int 
        itemCount = 0;

    bool 
        isChopReady;

    [SerializeField]
    SpriteRenderer
        _selector;
    [SerializeField]
    SpriteRenderer[]
        _displayItems = new SpriteRenderer[3];

    //Accessors
    public bool ChopReady
    {
        set { isChopReady = value; }
        get { return isChopReady; }
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    //Interact method to be called from the Player script when interacting
    public void Interact(ChefController player)
    {
        //If there is not an ingredient ready to be chopped...
        if (!isChopReady)
        {
            //And the player has an item in hand...
            if (player._collected[0] != SaladInventory.Ingredient.Empty)
            {
                //And the Cutting Board is not full...
                if (itemCount > 2)
                    return;

                //Check to see if the player has already added this ingredient
                foreach (SaladInventory.Ingredient item in _bowl)
                {
                    if (player._collected[0] == item)
                        return;
                }

                //Find the first space on the cutting board
                if (_bowl[itemCount] == SaladInventory.Ingredient.Empty && _bowl[itemCount] != player._collected[0])
                {
                    //Add the item from the player's hand, move the secondary item to the player's main hand, and remove the last item
                    _bowl[itemCount] = player._collected[0];
                    player._collected[0] = player._collected[1];
                    player._collected[1] = SaladInventory.Ingredient.Empty;
                    //Update the Cutting board and player inventory
                    UpdateColors();
                    player.UpdateInventory();
                    //Increment the item count and declare the board is ready to chop
                    itemCount++;
                    isChopReady = true;
                }
            }
        }
        else
        {
            //If the board is ready to be chopped, begin the Coroutine from the player script
            StartCoroutine(player.ChopTime());
        }
    }

    //Method to be called when the player is done chopping to update the item icons
    public void Chopped()
    {
        isChopReady = false;
        for (int i = 0; i < _bowl.Length; i++)
        {
            if (_bowl[i] != SaladInventory.Ingredient.Empty)
            {
                _displayItems[i].sprite = _choppedItem;
            }
        }
    }

    //Method to be called when the player wants to add the chopped items to a plate to serve the customer.
    public void CleanBoard()
    {
        foreach (SpriteRenderer item in _displayItems)
        {
            item.sprite = _rawItem;
        }

        for (int i = 0; i < _bowl.Length; i++)
        {
            _bowl[i] = SaladInventory.Ingredient.Empty;
        }
        itemCount = 0;

        UpdateColors();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the Player is near the ingredient source, change the selector color to that player's color.
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<ChefController>().PlayerID == _playerID)
            {
                _selector.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
                collision.GetComponent<ChefController>()._interactable = this.gameObject;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //When the Player leaves the ingredient source, hide the selector again.
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<ChefController>().PlayerID == _playerID)
            {
                _selector.color = new Color(0, 0, 0, 0);
                collision.GetComponent<ChefController>()._interactable = null;
            }
        }
    }

    //Method to be called to update the colors of the items on the cutting board.
    void UpdateColors()
    {
        for (int i = 0; i < _bowl.Length; i++)
        {
            _displayItems[i].color = SaladInventory.saladLogic.ingredientColors[(int)_bowl[i]];
        }
    }
}
