using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Customer Logic Script
 * Created by: Dereck Mills
 * 
 * This script controls the Customers that have been spawned in by the Master Script
 /**/

public class CustomerLogic : MonoBehaviour
{
    //public Variables
    public enum Location { Left, Center, Right };
    public Location 
        _location = Location.Center;
    public float
        _moveSpeed = 5,
        _timePerIngredient = 10,
        _angryPenalty = 1.2f,
        _spawnPickupPercent = .3f;

    public int
        _badOrderPenalty = -50,
        _correctOrderBase = 10,
        _exponentialIncrease = 3;

    public SpriteRenderer[] 
        orderIndictors = new SpriteRenderer[3];

    public Slider
        _progressBar;

    public Image
        _barColor;

    public Color
        _progressGood,
        _progressCaution,
        _progressRisk;

    //Private Variables
    bool 
        isMoving,
        isAngry,
        angryAtPlayer1,
        angryAtPlayer2;

    Vector3 
        targetLocation;
    float
        timeRemaining,
        timeLimit = 10;
    [SerializeField]
    SpriteRenderer 
        selector;
    SaladInventory.Ingredient[] 
        order;
    // Start is called before the first frame update
    void Start() {
        _progressBar.transform.parent = GameObject.FindWithTag("Canvas").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PointManager.points._isGameOver)
        {
            //If the Customer is flagged moving...
            if (isMoving)
            {
                //Move vertically first
                if (targetLocation.y != transform.position.y)
                {
                    if (targetLocation.y > transform.position.y)
                    {
                        transform.position += new Vector3(0, _moveSpeed) * Time.deltaTime;
                        if (transform.position.y > targetLocation.y)
                            transform.position = new Vector3(transform.position.x, targetLocation.y, transform.position.z);
                    }
                    else
                    {
                        transform.position -= new Vector3(0, _moveSpeed) * Time.deltaTime;
                        if (transform.position.y < targetLocation.y)
                            transform.position = new Vector3(transform.position.x, targetLocation.y, transform.position.z);
                    }
                }
                //Then move horizontally
                else if (targetLocation.x != transform.position.x)
                {
                    if (targetLocation.x > transform.position.x)
                    {
                        transform.position += new Vector3(_moveSpeed, 0) * Time.deltaTime;
                        if (transform.position.x > targetLocation.x)
                            transform.position = new Vector3(targetLocation.x, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        transform.position -= new Vector3(_moveSpeed, 0) * Time.deltaTime;
                        if (transform.position.x < targetLocation.x)
                            transform.position = new Vector3(targetLocation.x, transform.position.y, transform.position.z);
                    }
                }
                //If the player is at the target location, stop moving
                else
                {
                    isMoving = false;
                }
            }
            else
            {
                //If the progress bar is not active, turn it on
                if (!_progressBar.gameObject.activeInHierarchy)
                    _progressBar.gameObject.SetActive(true);
                //If the customer is angry, make them leave faster
                if (isAngry)
                    timeRemaining -= Time.deltaTime * _angryPenalty;
                else
                    timeRemaining -= Time.deltaTime;

                _progressBar.value = timeRemaining / timeLimit;

                //Change the color of the bar based on the progress
                if (_progressBar.value <= _spawnPickupPercent)
                    _barColor.color = _progressRisk;
                else if (_progressBar.value <= 1 - _spawnPickupPercent)
                    _barColor.color = _progressCaution;
                else
                    _barColor.color = _progressGood;

                if (timeRemaining <= 0)
                {
                    if (!isAngry)
                        PointManager.points.AdjustPoints(_badOrderPenalty, -1);
                    else
                    {
                        if (angryAtPlayer1)
                            PointManager.points.AdjustPoints(_badOrderPenalty * 2, 0);
                        if (angryAtPlayer2)
                            PointManager.points.AdjustPoints(_badOrderPenalty * 2, 1);
                    }
                    Leave();
                }
            }
        }
    }

    //Fill out customer information from the generator
    public void Prep(Location side, Vector3 moveOffset, SaladInventory.Ingredient[] newOrder)
    {
        //Spawn the customer above the screen, in the moving state, and on the side it was told.
        _location = side;
        isMoving = true;
        targetLocation = transform.position + moveOffset;

        //Change the order colors to match what plate they want
        for(int i = 0; i < newOrder.Length; i++)
        {
            orderIndictors[i].color = SaladInventory.saladLogic.ingredientColors[(int)newOrder[i]];
            if(newOrder[i] != SaladInventory.Ingredient.Empty)
            {
                timeLimit += _timePerIngredient;
            }
        }

        timeRemaining = timeLimit;
        order = newOrder;
    }

    //Tell the Customer where they are going
    public void MoveCustomer(Vector3 offset)
    {
        targetLocation += offset;
        isMoving = true;
    }

    //Interact method to be called from the Player script when interacting
    public void Interact(ChefController player)
    {
        //If the player has a plate...
        if (player._bowl)
        {
            //Save the item list to an array to be reference
            SaladInventory.Ingredient[] plate = player._bowl.GetComponent<SaladDish>()._items;

            //If the plate has a second item and the order doesn't
            if (order[1] == SaladInventory.Ingredient.Empty && plate[1] != SaladInventory.Ingredient.Empty)
            {
                isAngry = true;
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;

                if (player.PlayerID == 0)
                    angryAtPlayer1 = true;
                if (player.PlayerID == 1)
                    angryAtPlayer2 = true;
            }
            //If the plate has a third item and the order doesn't
            else if (order[2] == SaladInventory.Ingredient.Empty && plate[2] != SaladInventory.Ingredient.Empty)
            {
                isAngry = true;
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;

                if (player.PlayerID == 0)
                    angryAtPlayer1 = true;
                if (player.PlayerID == 1)
                    angryAtPlayer2 = true;
            }
            //Check to see if the plate has all the items from the order
            else
            {
                //Start with the assumption that all items are correct
                bool correct = true;
                int orderNum = -1;
                //For each item in the order...
                for (int i = 0; i < order.Length; i++)
                {
                    //If the order ingredient is not empty...
                    if (order[i] != SaladInventory.Ingredient.Empty)
                    {
                        //Check to see if it is found on the plate.
                        bool found = false;
                        orderNum++;
                        foreach (SaladInventory.Ingredient item in plate)
                        {
                            if (item == order[i])
                                found = true;
                        }
                        //If not found, end the check and remove points from the player
                        if (!found)
                        {
                            isAngry = true;
                            gameObject.GetComponent<SpriteRenderer>().color = Color.red;

                            if (player.PlayerID == 0)
                                angryAtPlayer1 = true;
                            if (player.PlayerID == 1)
                                angryAtPlayer2 = true;

                            correct = false;
                            break;
                        }
                    }
                }
                //If the entire order is correct, add points and remove the customer
                if (correct)
                {
                    if (timeRemaining/timeLimit >= _spawnPickupPercent)
                    {
                        CustomerQueue.queue.GeneratePowerUp(player);
                    }
                    PointManager.points.AdjustPoints(_correctOrderBase * (int)Mathf.Pow(_exponentialIncrease,orderNum), player.PlayerID);
                    Leave();
                }
            }
            //Once the order has been checked, remove the plate
            Destroy(player._bowl);
            player._bowl = null;
        }
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
            selector.color = new Color(0, 0, 0, 0);
            collision.GetComponent<ChefController>()._interactable = null;
        }
    }

    void UpdateColors()
    {

    }

    void Leave()
    {
        CustomerQueue.queue.AdjustCustomers(_location, this);
        Destroy(_progressBar.gameObject);
        Destroy(this.gameObject);
    }
}
