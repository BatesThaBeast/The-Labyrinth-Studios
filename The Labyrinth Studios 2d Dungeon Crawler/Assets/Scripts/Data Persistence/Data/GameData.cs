using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
/*This script will be the data that we actually want to store, whether it be the player's health, position, inventory, you name it
 In order to fully implement this class, one would need to also implement Data Persistence Interface into the scripts in which the desired data is being
 stored*/
[System.Serializable]
public class GameData
{
    public List<Item> storedItems;
    public int currentHealth;
    public int keyCount;
    public int arrowCount;
    public Vector3 playerPosition;
    public bool keyCollected;
    public bool hasDagger;
    public bool hasCrossbow;
    public bool holdingDagger;
    public bool holdingCrossbow;
    public bool usedAPortal;
    public SerializableDictionary<string, Vector3> playerPortalPosition;
    public SerializableDictionary<string, bool> chestOpened;
    public SerializableDictionary<string, bool> leversActivated;
    public SerializableDictionary<string, bool> doorUnlocked;
    public SerializableDictionary<string, bool> doorOpened;
    public SerializableDictionary<string, bool> plateOn;    
    public SerializableDictionary<string, Vector3> boxPosition;
    public SerializableDictionary<string, bool> boxHeld;
    public SerializableDictionary<string, bool> portal;
    public float LastHorizontalBox;
    public float LastVerticalBox;
    public float lastHorizontalPlayer;
    public float lastVerticalPlayer;
    public int currentScene;
    public GameData()//New Game values will be stored in this method
    {
        storedItems = new List<Item>();
        this.currentHealth = 3;//current health is loaded and saved in the player script. **IN PLAYER.cs**
        this.keyCount = 0;//How many keys does the player current have **IN KEYMANAGER.cs**
        this.arrowCount = 0;
        playerPosition = new Vector3(0f,0f,0f);//player position is loaded and saved in the player script **IN PLAYER.cs** 
        this.keyCollected = false;//keys that have been collected
        this.hasDagger = false;
        this.hasCrossbow = false;
        this.holdingDagger = false;
        this.holdingCrossbow = false;
        this.usedAPortal = false;
        playerPortalPosition = new SerializableDictionary<string, Vector3>();
        chestOpened = new SerializableDictionary<string, bool>();//chest that have been opened
        leversActivated = new SerializableDictionary<string, bool>();//levers activated are loaded and saved in the lever script **IN LEVER.cs**
        doorUnlocked = new SerializableDictionary<string, bool>();//Doors that have been unlocked **IN DOORTAKEINPUT.cs**
        doorOpened = new SerializableDictionary<string, bool>();//Doors that have been opened, **IN DOORTAKEINPUT.cs**
        plateOn = new SerializableDictionary<string, bool>();//pressure plate in the on or off stated ** IN PRESSUREPLATE.cs**         
        boxPosition = new SerializableDictionary<string, Vector3>();//box's position is loaded and saved in box script
        boxHeld = new SerializableDictionary<string, bool>();//will remember if the box is being held by player when the game is saved
        portal = new SerializableDictionary<string, bool>();
        /*Stores which way the player was facing*/
        this.LastHorizontalBox = 0f;// **IN PLAYER.cs** 
        this.LastVerticalBox = 0f;// **IN PLAYER.cs** 
        this.lastHorizontalPlayer = 0f;// **IN PLAYER.cs** 
        this.lastVerticalPlayer = 0f;// **IN PLAYER.cs** 
        /*--------------------------------------*/
        this.currentScene = 1;//This is the first scene
    }
}

