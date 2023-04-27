using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence (string n)
    {
        name = n;
    }

    public override Status Process()   //  Runs like an Update
    {
        Debug.Log("Running Sequence: " + name);
        Status childstatus = children[currentChild].Process();  // Gets status of running the current child process
        if (childstatus == Status.RUNNING) return Status.RUNNING;   // if still running continue running
        if (childstatus == Status.FAILURE)  // if failed returns a failure
            return childstatus;

        currentChild++;     // go to next child node
        if(currentChild >= children.Count)  // start a loop to the next child if there are still more nodes
        {
            currentChild = 0;       // reset node to top node as we have gone through all the nodes
            return Status.SUCCESS;  //  returns as success as ImageEffectAllowedInSceneView nodes have succedded
        }

        return Status.RUNNING;
    }


}
