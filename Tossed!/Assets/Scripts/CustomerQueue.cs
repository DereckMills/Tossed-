using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Customer Queue Script
 * Created by: Dereck Mills
 * 
 * This script will control the spawning of customers as well as their locations
 * 
 * Requirements: --
 /**/

public class CustomerQueue : MonoBehaviour
{
    public static CustomerQueue queue;

    //Public Variable
    public GameObject _customerObj;
    public Transform _spawnLocation;

    public float
        _spawnXOffset = 1.25f,
        _spawnYOffset = 2,
        
        _oneIngredientWeight = 60,
        _twoIngredientWeight = 25,
        _threeIngredientWeight = 15,
        
        _weightPunish = .9f;

    public int
        _rightCount,
        _leftCount;

    //Private Variables
    bool
        isCenter;

    // Start is called before the first frame update
    void Start()
    {
        if (!isCenter)
        {
            Vector3 offset = new Vector3(0, _spawnYOffset);
            CustomerLogic newCust = Instantiate(_customerObj, _spawnLocation.position + offset, _spawnLocation.rotation).GetComponent<CustomerLogic>();

            newCust.Prep(CustomerLogic.Location.Center, -offset, GenerateOrder());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    SaladInventory.Ingredient[] GenerateOrder()
    {
        SaladInventory.Ingredient[] newOrder = new SaladInventory.Ingredient[3];

        float totalWeight = _oneIngredientWeight + _twoIngredientWeight + _threeIngredientWeight;

        float value = Random.Range(0, totalWeight);

        if (value - _oneIngredientWeight < 0)
        {
            newOrder[0] = (SaladInventory.Ingredient)Random.Range(0, 5);
            newOrder[1] = newOrder[2] = SaladInventory.Ingredient.Empty;

            _twoIngredientWeight += 1;
            _threeIngredientWeight += 1;
            _oneIngredientWeight *= _weightPunish;

            return newOrder;
        }

        value -= _oneIngredientWeight;
        if (value - _twoIngredientWeight < 0)
        {
            newOrder[0] = newOrder[1] = (SaladInventory.Ingredient)Random.Range(0, 5);
            while (newOrder[1] == newOrder[0])
                newOrder[1] = (SaladInventory.Ingredient)Random.Range(0, 5);
                
            newOrder[2] = SaladInventory.Ingredient.Empty;

            _oneIngredientWeight += 1;
            _threeIngredientWeight += 1;
            _twoIngredientWeight *= _weightPunish;

            return newOrder;
        }
        else
        {
            newOrder[0] = newOrder[1] = newOrder[2] = (SaladInventory.Ingredient)Random.Range(0, 5);
            while (newOrder[1] == newOrder[0])
                newOrder[1] = (SaladInventory.Ingredient)Random.Range(0, 5);
            while (newOrder[2] == newOrder[0] || newOrder[2] == newOrder[1])
                newOrder[2] = (SaladInventory.Ingredient)Random.Range(0, 5);

            _oneIngredientWeight += 1;
            _twoIngredientWeight += 1;
            _threeIngredientWeight *= _weightPunish;

            return newOrder;
        }
    }
}
