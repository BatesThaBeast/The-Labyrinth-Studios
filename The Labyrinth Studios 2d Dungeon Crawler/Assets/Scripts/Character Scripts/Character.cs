using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;



public enum CharacterType//State machine for the type of character
{
    player, lintEnemy, ratEnemy, ratBoss, spiderEnemy, npc
}
public class Character : MonoBehaviour, IDamageable, IKillable, IPushable 
{    

    //******************************************************************************************************************************************************
    //************************************************************DECLARING IDamageable*********************************************************************
    //******************************************************************************************************************************************************
    public void Damage(int damage, Collider2D obj)//Author Johnathan Bates
    {
        
        GameObject temp = obj.gameObject;//reference to the gameobject attatched to obj      
            {
            if (damage > 0)
            {
                GameObject.Find("Hit Sfx").GetComponent<AudioSource>().Play();//sfx for getting hit 
            }
            if (temp.GetComponent<Character>().currentHealth > 0)//check if they still have health
            {
                if (obj.CompareTag("Fighter"))//Enemy damaged, will set state to stagger
                {
                    temp.GetComponent<Enemy>().currentState = EnemyState.stagger;
                }
                temp.GetComponent<Character>().currentHealth -= damage;//compute damage 
                if (temp.GetComponent<Character>().currentHealth <= 0)//check if they should be dead
                { Kill(obj.gameObject); }//KILL THEM!!!!
            }
        }
    }
    //******************************************************************************************************************************************************
    //************************************************************DECLARING IKILLABLE***********************************************************************
    //******************************************************************************************************************************************************
    public void Kill(GameObject obj)//Author Johnathan Bates
    {
        GameObject temp = obj;//reference to the gameobject
        if (obj.CompareTag("Player"))//check if it is the player that has died
        {
            temp.GetComponent<Player>().PlayerDied.Invoke();//kills player
        }
        else if (obj.CompareTag("Boss"))
        {
            SceneManager.LoadSceneAsync("EndGame");
        }
        else if(obj.CompareTag("BossSummon"))
        {            
            GameObject theBoss = GameObject.Find("RatPlagueDrBoss");
            theBoss.GetComponent<RatBoss>().currentMinionAmount--;
            if (dropChance > 75)
            {
                Instantiate(dropHeart, obj.transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f));
            }
            if(dropChance < 75 && dropChance > 60)
            {
                Instantiate(dropArrow, obj.transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f));
            }
            if(dropChance < 11 )
            {
                Instantiate(dropPotion, obj.transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f));
            }
            Destroy(obj);
            
        }
        else if (obj.CompareTag("Fighter"))
        {
            if (dropChance > 75)
            {
                Instantiate(dropHeart, obj.transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f));
            }
            if (dropChance < 75 && dropChance > 60)
            {
                Instantiate(dropArrow, obj.transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f));
            }
            if (dropChance < 11)
            {
                Instantiate(dropPotion, obj.transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f));
            }
            temp.gameObject.SetActive(false); temp.GetComponent<Enemy>().currentState = EnemyState.dead;            

        }   //kills everything else   
    }
    //******************************************************************************************************************************************************
    //************************************************************DECLARING IMOVABLE************************************************************************
    //******************************************************************************************************************************************************

   
    //******************************************************************************************************************************************************
    //************************************************************DECLARING IPUSHABLE***********************************************************************
    //******************************************************************************************************************************************************
    public float thrust { get;  set; }//how hard an object is pushed back
    public float pushTime { get; set; }//how long the push back last
    public void Push(Collider2D obj)//Author Johnathan Bates
    {
        thrust = 5f;//used to determine the thrust force
        pushTime = .16f;//used to determine how long the push is happening
        Rigidbody2D character = obj.GetComponent<Rigidbody2D>();//reference to the Rigidbody component
            if (character != null)//makes sure the object hasn't already been destroyed
            {           
                Vector2 difference = character.transform.position - transform.position;//not sure lol
                difference = difference.normalized * this.thrust;//this is where the thrust will change how far something is pushed back
            if (character != null)
            {
                character.AddForce(difference, ForceMode2D.Impulse);//the actual push occurs here
            }
                StartCoroutine(PushCo(character));
                if(character.CompareTag("Player"))
                {
                    character.GetComponent<Player>().currentState = PlayerState.walk;
                }
            }
    }   
    public IEnumerator PushCo(Rigidbody2D character)//Author Johnathan Bates
    {
        beingPushed = true;
        if (character != null)
        {
            if (character.CompareTag("Player"))
            {
                character.GetComponent<Animator>().SetBool("staggered", true);
                character.GetComponent<Player>().currentState = PlayerState.stagger;
                character.GetComponent<Animator>().SetFloat("moveX", character.GetComponent<Player>().lastFacingHorizontal);//allows movement animation
                character.GetComponent<Animator>().SetFloat("moveY", character.GetComponent<Player>().lastFacingVertical);//allows movement animation
            }            
            yield return new WaitForSeconds(this.pushTime);//how long the push last
        }        
        if (character != null)//check to see the character isn't destroyed/set to null
        {
            if (character.CompareTag("Player"))
            {
                character.GetComponent<Animator>().SetBool("staggered", false);
                character.GetComponent<Player>().currentState = PlayerState.walk;
            }
            character.velocity = Vector2.zero;//stops the push
        }
        beingPushed = false;
    }
    //******************************************************************************************************************************************************
    //********************************************************CHARACTER CLASS ATTRIBUTES********************************************************************
    //******************************************************************************************************************************************************
    public Rigidbody2D thisBody;//Use this in a child class or inspector to initialize RigidBody
    protected Animator anim;//Use this in a child class or inspector to initialize Animator
    protected Vector2 movement;//Use this in a child class to control movement 
    public float moveSpeed;//Use this in a child class or inspector tocontrol movement speed
    public CharacterType charType;//Use this in the Inspector to initialize what type of character it is
    public int attackDamage;//Use this in a child class or inspector to initialize RigidBody
    public int currentHealth;//Use this in a child class or inspector to initialize health
    public int maxHealth;
    public GameObject dropHeart;
    public GameObject dropArrow;
    public GameObject dropPotion;
    protected int dropChance;
    protected int itemChance;
    protected bool beingPushed = false;
    System.Random random;
    
    void FixedUpdate()
    {
        random = new System.Random();
        dropChance = random.Next(0,101);
        itemChance = random.Next(0, 4);
    }
}
