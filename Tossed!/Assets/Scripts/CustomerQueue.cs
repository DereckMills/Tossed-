using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Customer Queue Script
 * Created by: Dereck Mills
 * 
 * This script will control the spawning of customers as well as their locations
 /**/

public class CustomerQueue : MonoBehaviour
{
    //Public Variable
    public static CustomerQueue 
        _queue;
    public GameObject 
        _customerObj;
    public Transform 
        _spawnLocation;

    public float
        _spawnXOffset = 1.25f,
        _spawnYOffset = 2,
        
        _oneIngredientWeight = 60,
        _twoIngredientWeight = 25,
        _threeIngredientWeight = 15,
        
        _weightPunish = .9f,
        _weightGain = 1;

    public int
        _rightCount,
        _leftCount;

    //Private Variables
    bool
        isCenter;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn the first customer
        if (!isCenter)
        {
            Vector3 offset = new Vector3(0, _spawnYOffset);
            CustomerLogic newCust = Instantiate(_customerObj, _spawnLocation.position + offset, _spawnLocation.rotation).GetComponent<CustomerLogic>();

            newCust.Prep(CustomerLogic.Location.Center, -offset, GenerateOrder());
        }
    }

    // Update is called once per frame
    void Update() { }

    //Method to generate a random order for the customer to spawn with.
    SaladInventory.Ingredient[] GenerateOrder()
    {
        SaladInventory.Ingredient[] newOrder = new SaladInventory.Ingredient[3];

        //Compile all the weights for the number of ingredients
        float totalWeight = _oneIngredientWeight + _twoIngredientWeight + _threeIngredientWeight;

        //Generate a Value within the range of the weights
        float value = Random.Range(0, totalWeight);

        //If the value is less than the weight of (1) Item...
        if (value - _oneIngredientWeight < 0)
        {
            //Generate a random item in the first slot, and nothing in the second two.
            newOrder[0] = (SaladInventory.Ingredient)Random.Range(0, 5);
            newOrder[1] = newOrder[2] = SaladInventory.Ingredient.Empty;

            //Increase the weight of (2) and (3) items, and reduce the weight of (1) item
            _twoIngredientWeight += _weightGain;
            _threeIngredientWeight += _weightGain;
            _oneIngredientWeight *= _weightPunish;

            return newOrder;
        }

        //Remove the weight of (1) item from the value and check to see if its less than the weight of (2) items
        value -= _oneIngredientWeight;
        if (value - _twoIngredientWeight < 0)
        {
            //Generate a random item in the first two slots, and then generate a random item in the second slot until it is different from the first.
            newOrder[0] = newOrder[1] = (SaladInventory.Ingredient)Random.Range(0, 5);
            while (newOrder[1] == newOrder[0])
                newOrder[1] = (SaladInventory.Ingredient)Random.Range(0, 5);
            //Assign an empty item to the last slot
            newOrder[2] = SaladInventory.Ingredient.Empty;

            //Increase the weight of (1) and (3) items, and reduce the weight of (2) items
            _oneIngredientWeight += 1;
            _threeIngredientWeight += 1;
            _twoIngredientWeight *= _weightPunish;

            return newOrder;
        }
        //Default to generating (3) items if (1) and (2) were not selected
        else
        {
            //Generate a random item in all three slots, and then re-roll the second and third until they are unique
            newOrder[0] = newOrder[1] = newOrder[2] = (SaladInventory.Ingredient)Random.Range(0, 5);
            while (newOrder[1] == newOrder[0])
                newOrder[1] = (SaladInventory.Ingredient)Random.Range(0, 5);
            while (newOrder[2] == newOrder[0] || newOrder[2] == newOrder[1])
                newOrder[2] = (SaladInventory.Ingredient)Random.Range(0, 5);

            //Increase the weight of (1) and (2) items, and reduce the weight of (3) items
            _oneIngredientWeight += 1;
            _twoIngredientWeight += 1;
            _threeIngredientWeight *= _weightPunish;

            return newOrder;
        }
    }
}
