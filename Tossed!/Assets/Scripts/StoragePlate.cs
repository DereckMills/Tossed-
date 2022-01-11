using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Storage Plate Script
 * Created by: Dereck Mills
 * 
 * This script will add logic to the plate object which will take chopped items from
 * the cutting board so they can be taken to customers.
 /**/

public class StoragePlate : MonoBehaviour
{
    //Public Variables
    public int 
        _playerID = 0;
    public GameObject
        _saladbowl;

    //Private Variables
    [SerializeField]
    SpriteRenderer 
        _selector,
        _ingredient;
    [SerializeField]
    CuttingBoard 
        board;

    SaladInventory.Ingredient heldItem = SaladInventory.Ingredient.Empty;


    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    //Interact method to be called from the Player script when interacting
    public void Interact(ChefController player)
    {
        //If there is no item on the plate, the cutting board has been chopped, and there is an item on the cutting board...
        if (heldItem == SaladInventory.Ingredient.Empty && !board.ChopReady && board._bowl[0] != SaladInventory.Ingredient.Empty)
        {
            //Create a plate for the player to hold
            SaladDish bowl = Instantiate(_saladbowl, player.transform).GetComponent<SaladDish>();
            //Fill the plate with the items from the cutting board
            for (int i = 0; i < board._bowl.Length; i++)
            {
                bowl._items[i] = board._bowl[i];
            }
            bowl.UpdateColors();

            //Remove items from the cutting board
            board.CleanBoard();

            //Place the item from the player's hand onto the plate for storage.
            if (player._collected[0] != SaladInventory.Ingredient.Empty)
            {
                heldItem = player._collected[0];
                player._collected[0] = player._collected[1];
                player._collected[1] = SaladInventory.Ingredient.Empty;

                player.UpdateInventory();
            }
            //Assign the bowl to the player's script for future reference
            player._bowl = bowl.gameObject;

        }
        //If there is an item on the storage place...
        else if (heldItem != SaladInventory.Ingredient.Empty)
        {
            //Remove the item from the plate in the edge case where the player has picked up the item stored on the plate
            if (player._collected[0] == heldItem || player._collected[1] == heldItem)
            {
                heldItem = SaladInventory.Ingredient.Empty;
            }
            //Otherwise assign the item on the plate to the first open hand if there is one.
            else if (player._collected[0] == SaladInventory.Ingredient.Empty)
            {
                player._collected[0] = heldItem;
                player.UpdateInventory();
                heldItem = SaladInventory.Ingredient.Empty;
            }
            else if (player._collected[1] == SaladInventory.Ingredient.Empty)
            {
                player._collected[1] = heldItem;
                player.UpdateInventory();
                heldItem = SaladInventory.Ingredient.Empty;
            }
        }
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

    void UpdateColors()
    {
        _ingredient.color = SaladInventory.saladLogic.ingredientColors[(int)heldItem];
    }
}
