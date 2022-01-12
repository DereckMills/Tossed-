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
        queue;
    //public Variables
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
        _weightGain = 1,

        _minimumSpawnDelay = 5,
        _maximumSpawnDelay = 20;

    public int
        _rightCount,
        _leftCount;

    //Private Variables
    bool
        isCenter,
        rightSpawnNext = true;
    float
        spawnTime = 1;
    int
        customersServed = 0;
    [SerializeField]
    CustomerLogic[]
        rightCustomers = { null, null, null },
        leftCustomers = { null, null, null };

    // Start is called before the first frame update
    void Start()
    {
        //Apply Singleton logic to this script
        if (!queue)
        {
            queue = this;
        }
        else if (queue != this)
        {
            Destroy(this.gameObject);
        }

        //Randomly choose which side gets the first customer.
        int value = Random.Range(0, 1);
        rightSpawnNext = value == 1;
    }

    // Update is called once per frame
    void Update() { 
        if (!PointManager.points._isGameOver)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime <= 0)
            {
                SpawnCustomer();
                spawnTime += Random.Range(_minimumSpawnDelay, _maximumSpawnDelay);
            }
        }
    }

    void SpawnCustomer()
    {
        //Always prioritize spawning a customer in the middle
        if (!isCenter)
        {
            Vector3 offset = new Vector3(0, _spawnYOffset);
            CustomerLogic newCust = Instantiate(_customerObj, _spawnLocation.position + offset, _spawnLocation.rotation).GetComponent<CustomerLogic>();
            newCust.gameObject.name = "Customer " + customersServed;
            customersServed++;
            newCust.Prep(CustomerLogic.Location.Center, -offset, GenerateOrder());
            isCenter = true;
        }
        //Then spawn one to the right
        else if (rightSpawnNext && _rightCount < 3)
        {
            _rightCount++;
            Vector3 offset = new Vector3(_spawnXOffset * 3, _spawnYOffset);
            CustomerLogic newCust = Instantiate(_customerObj, _spawnLocation.position + offset, _spawnLocation.rotation).GetComponent<CustomerLogic>();
            newCust.gameObject.name = "Customer " + customersServed;
            customersServed++;

            //Add the new Customer to the right Customer list
            for (int i = 0; i < rightCustomers.Length; i++)
            {
                if (rightCustomers[i] == null)
                {
                    rightCustomers[i] = newCust;
                    break;
                }
            }

            newCust.Prep(CustomerLogic.Location.Right, new Vector3(_spawnXOffset * -(3 -_rightCount), -offset.y), GenerateOrder());
            rightSpawnNext = false;
        }
        //Finally spawn to the left
        else if (_leftCount < 3)
        {
            _leftCount++;
            Vector3 offset = new Vector3(_spawnXOffset * -3, _spawnYOffset);
            CustomerLogic newCust = Instantiate(_customerObj, _spawnLocation.position + offset, _spawnLocation.rotation).GetComponent<CustomerLogic>();
            newCust.gameObject.name = "Customer " + customersServed;
            customersServed++;

            //Add the new Customer to the left Customer list
            for (int i = 0; i < leftCustomers.Length; i++)
            {
                if (leftCustomers[i] == null)
                {
                    leftCustomers[i] = newCust;
                    break;
                }
            }

            newCust.Prep(CustomerLogic.Location.Left, new Vector3(_spawnXOffset * (3-_leftCount),-offset.y), GenerateOrder());
            rightSpawnNext = true;
        }
    }

    public void AdjustCustomers(CustomerLogic.Location openSpot, CustomerLogic leaving)
    {
        //If there are no other Customers, open the center Spawn
        if (_rightCount == 0 && _leftCount == 0)
        {
            isCenter = false;
        }
        //If the missing Customer was in the Center...
        if (openSpot == CustomerLogic.Location.Center)
        {
            //If the right side has more customers, or if they are equal and the next would arrive on the right...
            if (_rightCount > _leftCount || (_rightCount == _leftCount && rightSpawnNext))
            {
                //Move all right customers down one
                foreach (CustomerLogic customer in rightCustomers)
                {
                    if (customer)
                        customer.MoveCustomer(-new Vector3(_spawnXOffset, 0));
                }
                //Assign the new center customer
                rightCustomers[0]._location = CustomerLogic.Location.Center;
                //Move all right customers down in the list
                for (int i = 0; i < rightCustomers.Length - 1; i++)
                {
                    rightCustomers[i] = rightCustomers[i + 1];
                }
                rightCustomers[rightCustomers.Length - 1] = null;
                _rightCount--;
            }
            //If the left side has more customers, or if they are equal and the next would arrive on the left
            else if (_leftCount > _rightCount || (_rightCount == _leftCount && !rightSpawnNext))
            {
                //Move all left customers down one
                foreach (CustomerLogic customer in leftCustomers)
                {
                    if (customer)
                        customer.MoveCustomer(new Vector3(_spawnXOffset, 0));
                }
                //Assign the new center customer
                leftCustomers[0]._location = CustomerLogic.Location.Center;
                //Move all right customers down in the list
                for (int i = 0; i < leftCustomers.Length - 1; i++)
                {
                    leftCustomers[i] = leftCustomers[i + 1];
                }
                leftCustomers[leftCustomers.Length - 1] = null;
                _leftCount--;
            }
        }
        //If the missing Customer was on the Right...
        else if (openSpot == CustomerLogic.Location.Right)
        {
            //Don't move customers until the one that left is found
            bool adjust = false;
            for (int i = 0; i < rightCustomers.Length; i++)
            {
                if (!adjust)
                {
                    if (rightCustomers[i] == leaving)
                        adjust = true;
                }
                //Once the one leaving is found, move the customer physically and in the array
                else
                {
                    if (rightCustomers[i])
                    {
                        rightCustomers[i].MoveCustomer(new Vector3(-_spawnXOffset, 0));
                    }
                    rightCustomers[i - 1] = rightCustomers[i];
                }
            }
            //Set the last customer in the array to null and reduce the right count
            rightCustomers[rightCustomers.Length - 1] = null;
            _rightCount--;
        }
        //If the missing Customer was on the Left...
        else if (openSpot == CustomerLogic.Location.Left)
        {
            //Don't move customers until the one that left is found
            bool adjust = false;
            for (int i = 0; i < leftCustomers.Length; i++)
            {
                if (!adjust)
                {
                    if (leftCustomers[i] == leaving)
                        adjust = true;
                }
                //Once the one leaving is found, move the customer physically and in the array
                else
                {
                    if (leftCustomers[i])
                    {
                        leftCustomers[i].MoveCustomer(new Vector3(_spawnXOffset, 0));
                    }
                    leftCustomers[i - 1] = leftCustomers[i];
                }
            }
            //Set the last customer in the array to null and reduce the right count
            leftCustomers[leftCustomers.Length - 1] = null;
            _leftCount--;
        }
    }

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
