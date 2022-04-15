using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lint1 : Enemy1
{
    private Rigidbody2D thisRigidbody;
    public Transform target;//will be our player
    public float chaseRadius;//the radius in which the enemy will activate and chase the player
    public float attackRadius;//how far away we want the enemy to stop from the player
    public float knockBackRadius;//this will initiate the knockback routine
    public Vector2 homePosition;//the enemy's home position so it can return after chasing
    

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;//stores the player as the target
        homePosition = this.gameObject.transform.position;//stores the home position of the enemy
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();        
    }
    

    //CheckDistance will do as it's called, check the distance between the Player and the Enemy. If the Player is within range, the Enemy will pursue
    //the player
    void CheckDistance()
    {
        Vector2 temp;
        //this checks to see if player is in chase range but outside of "attack range"
        //attack range is used so the enemy doesn't try to go through the player
        if (Vector2.Distance(transform.position, target.position) <= chaseRadius
             && Vector2.Distance(transform.position, target.position) >= attackRadius)
        {
            //moves the lint enemy towards the player
            temp = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            thisRigidbody.MovePosition(temp);
        }         
       else
        {
            //makes the lint enemy return to it's home position
            temp = Vector2.MoveTowards(transform.position, homePosition, (moveSpeed / 1.2f) * Time.deltaTime);
            thisRigidbody.MovePosition(temp);
        }
    }   

}
