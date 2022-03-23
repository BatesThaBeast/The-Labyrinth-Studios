using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : Collectable
{
    public UnityEvent KeyCollect;

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            KeyCollect.Invoke();
        }
    }

}