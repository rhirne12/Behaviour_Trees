using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status { SUCCESS, RUNNING, FAILURE };   // any node can be 1 of 3 statuses
    public Status status;                               // stores the returned value of STATUS
    public List<Node> children = new();                 // Gets list of Children Nodes for the node
    public int currentChild = 0;                        // Looks at very first child node.  Reads nodes left to right
    public string name;                                 // give each node a name.  Helps with debugging

    public Node()
    {
        // Empty Node
    }

    public Node(string n)
    {
        name = n;           //  Sets name of node
    }

    public virtual Status Process()
    {
        Debug.Log("Running Node: " + name);
        return children[currentChild].Process();
    }

    public void AddChild(Node n)
    {
        children.Add(n);        // Adds child nodes to the list - 0, 1, 2, etc... order you want them to execute in should be ordered they are put in the list
    }
}
