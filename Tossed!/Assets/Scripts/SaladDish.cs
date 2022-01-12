using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Salad Dish Script
 * Created by: Dereck Mills
 * 
 * This script will control the logic for the plate carrying the order
 * to the customer.
 /**/

public class SaladDish : MonoBehaviour
{
    //Public Variables
    public SaladInventory.Ingredient[] 
        _items = { SaladInventory.Ingredient.Empty, SaladInventory.Ingredient.Empty, SaladInventory.Ingredient.Empty };
    [SerializeField]
    public SpriteRenderer[] 
        _displayItems = new SpriteRenderer[3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Method for updating the displayed items on the plate.
    public void UpdateColors()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _displayItems[i].color = SaladInventory.saladLogic.ingredientColors[(int)_items[i]];
        }
    }
}
