using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    // Create a delegate to pass through methods to return one of our 3 statuses

    public delegate Status Tick();          // one turnover for the behaviour tree - a freame for the behavior tree - like update
    public Tick ProcessMethod;

    public Leaf() { }

    public Leaf( string n, Tick pm)
    {
        name = n;
        ProcessMethod = pm;
    }

    public override Status Process()
    {
        Debug.Log("Running Leaf: " + name);
        if (ProcessMethod != null)
            return ProcessMethod();
        return Status.FAILURE;
    }

}
