using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Customer Logic Script
 * Created by: Dereck Mills
 * 
 * This script controls the Customers that have been spawned in by the Master Script
 * 
 * Requirements: --
 /**/

public class CustomerLogic : MonoBehaviour
{
    public enum Location { Left, Center, Right };
    public Location 
        _location = Location.Center;
    public float 
        _moveSpeed = 5;
    public SpriteRenderer[] orderIndictors = new SpriteRenderer[3];

    //Private Variables
    bool isMoving;
    Vector3 targetLocation;
    [SerializeField]
    SpriteRenderer selector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
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
            else if (targetLocation.x != transform.position.x)
            {
                if (targetLocation.x > transform.position.x)
                {
                    transform.position += new Vector3(_moveSpeed,0) * Time.deltaTime;
                    if (transform.position.x > targetLocation.x)
                        transform.position = new Vector3(targetLocation.x,transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position -= new Vector3(_moveSpeed,0) * Time.deltaTime;
                    if (transform.position.x < targetLocation.x)
                        transform.position = new Vector3(targetLocation.x, transform.position.y, transform.position.z);
                }
            }
            else
            {
                isMoving = false;
            }
        }
        
    }

    public void Prep(Location side, Vector3 moveOffset, SaladInventory.Ingredient[] newOrder)
    {
        _location = side;
        isMoving = true;
        targetLocation = transform.position + moveOffset;

        for(int i = 0; i < newOrder.Length; i++)
        {
            orderIndictors[i].color = SaladInventory.saladLogic.ingredientColors[(int)newOrder[i]];
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

    }
}
