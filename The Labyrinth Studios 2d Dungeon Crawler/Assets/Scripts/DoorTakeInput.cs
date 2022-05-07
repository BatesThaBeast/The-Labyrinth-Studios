using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTakeInput : Collectable , IDataPersistence
{
    //******************************************************************************************************************************************************
    //***********************************************************************GUID***************************************************************************
    //******************************************************************************************************************************************************
    [SerializeField] private string doorId;//will be used for saving game state
    [ContextMenu("Generate guid for id")]
    /*the context menu above uses the GenerateGuid() below to allow someone to generate a unique id for levers.
     All one has to do is click on a lever, expand the lever script in the inspector, right click the script and select
    Generate guid for id. This will create a unique id that will be used when the game's state is saved.*/
    private void GenerateGuid()
    {
        doorId = System.Guid.NewGuid().ToString();
    }
    //******************************************************************************************************************************************************
    //************************************************************DECLARING IDATAPERSISTENCE****************************************************************
    //******************************************************************************************************************************************************
    public void LoadData(GameData data)
    {
        data.doorLocked.TryGetValue(this.name, out Locked);
        if(Locked)
        {
            LockDoor();
            anim.SetBool("DoorUnlock", false);
        }
        else
        {
            UnlockDoor();
            anim.SetBool("DoorUnlock", true);
        }
        data.doorOpened.TryGetValue(this.name, out collected);
        if(collected)
        {
            DoorOn.Invoke();
            ActivatePortal.Invoke();
            anim.SetBool("DoorOpen", true);
        }
        else
        {
            anim.SetBool("DoorOpen", false);
        }
        
    }
    public void SaveData(GameData data)
    {
        if(data.doorLocked.ContainsKey(doorId))
        {
            data.doorLocked.Remove(doorId);
        }
        data.doorOpened.Add(this.name, collected);
        data.doorLocked.Add(this.name, Locked);
    }
    public UnityEvent DoorOn;
    public UnityEvent DoorOff;
    public UnityEvent DisplayText;
    public UnityEvent CloseText;
    public bool Locked = true;
    public UnityEvent ActivatePortal;
    private Animator anim;
    private void Awake()
    {
        anim = this.gameObject.GetComponentInParent<Animator>();
    }

    protected override void OnCollect()
    {
        
        if (Input.GetKeyDown("e") && Locked == false)
        {
            anim.SetBool("DoorUnlock", true);
            if (!collected)
            {
                collected = true;
                DoorOn.Invoke(); //runs the unity events to disable the door
                GameObject.Find("Door Open SFX").GetComponent<AudioSource>().Play();//plays open door sfx
                ActivatePortal.Invoke();
                anim.SetBool("DoorOpen", true);
            }
            else
            {
                collected = false;
                DoorOff.Invoke(); //enables the door
                ActivatePortal.Invoke();
                anim.SetBool("DoorOpen", false);
            }
        }
        else if (Input.GetKeyDown("e") && Locked == true)
        {
            DoorLocked();
        }

    }

    public void DoorLocked()
    {
        if (KeyManager.instance.KeyCount >= 1 && Locked == true)
        {
            UnlockDoor();
            KeyManager.instance.SubtractKey();         
            anim.SetBool("DoorUnlock", true);
        }
        else
            DisplayText.Invoke();
    }

    public void UnlockDoor()
    {
        Locked = false;
        DoorOff.Invoke();
    }

    public void LockDoor()
    {
        Locked = true;
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            CloseText.Invoke();
            Debug.Log("Im here.");
        }
    }
}