using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Salad Inventory Script
 * Created by: Dereck Mills
 * 
 * This script will handle any logic involved with salads and their 
 * ingredients.
 /**/

public class SaladInventory : MonoBehaviour
{
    //Public Variables
    public enum Ingredient { Lettuce, Tomato, Pepper, Kale, Cabbage, Carrot, Empty }

    public List<GameObject> 
        ingredientSource = new List<GameObject>();

    public static SaladInventory 
        saladLogic;

    public List<Color> 
        ingredientColors = new List<Color>();

    //Private Variables

    // Start is called before the first frame update
    void Start()
    {
        //Apply Singleton logic to this script
        if (!saladLogic)
            saladLogic = this;
        else if (this != saladLogic)
            Destroy(this.gameObject);

        //Fill out the colors for each ingredient from their source object
        foreach (GameObject ingredient in ingredientSource)
        {
            ingredientColors.Add(ingredient.GetComponent<SpriteRenderer>().color);
        }
        //Add one more Color for Empty
        ingredientColors.Add(new Color(0, 0, 0, 0));
    }

    // Update is called once per frame
    void Update() { }
}
