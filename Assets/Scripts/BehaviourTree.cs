using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
    public BehaviourTree()
    {
        name = "Tree";
    }

    public BehaviourTree(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Debug.Log("Running Tree: " + name);
        return children[currentChild].Process();
    }


    struct NodeLevel  // a way to structure the PrintTree so that on the debug screen we have indentations for each level
    {
        public int level;
        public Node node;
    }

    public void PrintTree()  // Debug Print Out of Tree structure
    {
        string treePrintout = "";
        Stack<NodeLevel> nodeStack = new();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel { level = 0, node = currentNode } );   // Gets root of tree in the stack

        while (nodeStack.Count !=0)   // loops through children and parents of stack
        {
            NodeLevel nextNode = nodeStack.Pop();   // Grab hold root of current tree
            treePrintout += new string('-', nextNode.level) + nextNode.node.name + "\n";   //  Adds a dash to line to indicate the level the node is on 
            for(int i = nextNode.node.children.Count - 1; i >=0; i-- )  // loops around children of node backwards so that they are in correct order
            {
                nodeStack.Push(new NodeLevel { level = nextNode.level + 1, node = nextNode.node.children[i] } );
            }
        }

        Debug.Log(treePrintout);
    }
}
