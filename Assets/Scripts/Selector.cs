using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Debug.Log("Running Selector: " + name);
        Status childstatus = children[currentChild].Process();  // Gets status of running the current child process
        if (childstatus == Status.RUNNING) return Status.RUNNING;   // if still running continue running
        if (childstatus == Status.SUCCESS)  // if failed returns a failure
        {
            currentChild = 0;       // reset node to top node as we have gone through all the nodes
            return Status.SUCCESS;  //  returns as success as ImageEffectAllowedInSceneView nodes have succedded
        }

        currentChild++;
        if (currentChild >= children.Count)  // start a loop to the next child if there are still more nodes
        {
            currentChild = 0;       // reset node to top node as we have gone through all the nodes
            return Status.FAILURE;  //  returns as success as ImageEffectAllowedInSceneView nodes have succedded
        }

        return Status.RUNNING;
        
    }
}
