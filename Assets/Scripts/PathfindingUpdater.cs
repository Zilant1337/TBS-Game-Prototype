using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        DestructableCrate.OnAnyCrateDestroyed += DestructableCrate_OnAnyCrateDestroyed;
    }

    private void DestructableCrate_OnAnyCrateDestroyed(object sender, EventArgs e)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructableCrate.GetGridPosition(),true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
