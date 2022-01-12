using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Trash Can Script
 * Created by: Dereck Mills
 * 
 * This script will control the logic for the trash can where the player can dispose
 * of ingredients and orders they don't need anymore.
 /**/

public class TrashCan : MonoBehaviour
{
    //Public Variables
    public int
        _ingredientPenalty = -10;
    //Private Variables
    [SerializeField]
    SpriteRenderer 
        _selector;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void Interact(ChefController player)
    {
        //If the player is holding a bowl...
        if (player._bowl)
        {
            //Destroy the Bowl and remove points 
            int pointsLost = 0;
            foreach(SaladInventory.Ingredient item in player._bowl.GetComponent<SaladDish>()._items)
            {
                if (item != SaladInventory.Ingredient.Empty)
                    pointsLost += _ingredientPenalty;
            }
            PointManager.points.AdjustPoints(pointsLost, player.PlayerID);

            Destroy(player._bowl.gameObject);
            player._bowl = null;
        //If the player doesn't have a bowl, but has something in their hand...
        } else if (player._collected[0] != SaladInventory.Ingredient.Empty){
            //Remove the main item and remove points
            PointManager.points.AdjustPoints(_ingredientPenalty, player.PlayerID);
            //If the player has a second item, move that to the main hand.
            if (player._collected[1] != SaladInventory.Ingredient.Empty)
            {
                player._collected[0] = player._collected[1];
                player._collected[1] = SaladInventory.Ingredient.Empty;
            }
            else
            {
                player._collected[0] = SaladInventory.Ingredient.Empty;
            }
            //Update the player's items
            player.UpdateInventory();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the Player is near the ingredient source, change the selector color to that player's color.
        if (collision.tag == "Player")
        {
            _selector.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
            collision.GetComponent<ChefController>()._interactable = this.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //When the Player leaves the ingredient source, hide the selector again.
        if (collision.tag == "Player")
        {
            _selector.color = new Color(0, 0, 0, 0);
            collision.GetComponent<ChefController>()._interactable = null;
        }
    }
}
