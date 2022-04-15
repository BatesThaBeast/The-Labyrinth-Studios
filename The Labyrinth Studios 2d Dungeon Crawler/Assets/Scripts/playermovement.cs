using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class playermovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    Vector2 movement;
    public Animator animator;
    public Transform interactor;
    public UnityEvent PlayerDied;

    // Code below belongs in player script, but cannot be applied to player
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {            
            health -= damage;
            Debug.Log("Lose heart");
            if(health == 0)
            {
                PlayerDied.Invoke();
                Debug.Log("Player Died");
                gameObject.GetComponent<GameOver>().Death();
            }
        }
        else if (health <= 0)
        {
            
            PlayerDied.Invoke();
            Debug.Log("Player Died");
            gameObject.GetComponent<GameOver>().Death();
        }
    }
    // Code above belongs in player, but player script is not applied to player yet
    void Update()
    {
        // Code below belongs in player script file
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        // Code above belongs in player script file
        movement = Vector2.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        if(movement != Vector2.zero)
        {
            moveCharacter();
        }
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                animator.SetFloat("LastHorizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
            }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            interactor.localRotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            interactor.localRotation = Quaternion.Euler(0, 0, -90);
        }
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            interactor.localRotation = Quaternion.Euler(0, 0, 180);
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            interactor.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    private void moveCharacter()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
   
}
