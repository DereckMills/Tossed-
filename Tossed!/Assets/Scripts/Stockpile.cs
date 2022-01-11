using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Salad Stockpile Script
 * Created by: Dereck Mills
 * 
 * This script will allow the player to pickup ingredients from the sources.
 /**/

public class Stockpile : MonoBehaviour
{
    //Public Variables
    public SaladInventory.Ingredient 
        source = SaladInventory.Ingredient.Cabbage;

    //Private Variables
    [SerializeField]
    SpriteRenderer 
        selector;
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    //Interact method to be called from the Player script when interacting
    public void Interact(ChefController player)
    {
        if (player._collected[0] == SaladInventory.Ingredient.Empty)
            player._collected[0] = source;
        else if (player._collected[1] == SaladInventory.Ingredient.Empty && player._collected[0] != source)
            player._collected[1] = source;

        player.UpdateInventory();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the Player is near the ingredient source, change the selector color to that player's color.
        if (collision.tag == "Player")
        {
            selector.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
            collision.GetComponent<ChefController>()._interactable = this.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //When the Player leaves the ingredient source, hide the selector again.
        if (collision.tag == "Player")
        {
            selector.color = new Color(0,0,0,0);
            collision.GetComponent<ChefController>()._interactable = null;
        }
    }
}
