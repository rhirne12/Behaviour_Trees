using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    public GameObject diamond;
    public GameObject van;
    public GameObject backDoor;
    public GameObject frontDoor;
    NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING };   // states agent could be in
    ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;

    [Range(0, 1000)]
    public int money = 800;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        // Setup our nodes in the tree
        tree = new BehaviourTree();
        Sequence steal = new("Steal Something");
        Leaf hasGotMoney = new("HasGotMoney", HasMoney);
        Leaf goToBackDoor = new("Go To Back Door", GoToBackDoor);
        Leaf goToFrontDoor = new("Go To Front Door", GoToFrontDoor);
        Leaf goToDiamond = new("Go To Diamond", GoToDiamond);
        Leaf goToVan = new("Go To Van", GoToVan);
        
        // Create a Selector to choose an open door and add nodes to Selector the tree
        Selector openDoor = new("Open Door");

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);


        // Add nodes to the Sequence Tree in the proper order
        // need to add in starting at the bottom and work way up
        steal.AddChild(hasGotMoney);
        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        // steal.AddChild(goToBackDoor);
        steal.AddChild(goToVan);
        tree.AddChild(steal);


        tree.PrintTree();   // Debug Log print tree        

        Time.timeScale = 5;

    }

    public Node.Status HasMoney()
    {
        if (money >= 500)
            return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }

    public Node.Status GoToDiamond()
    {
        return RobItem(diamond);
    }

    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            diamond.SetActive(false);
            Debug.Log("Money: " + this.money);
            this.money += 525;
        }
        return s;
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);   // Sends agent to door
        if (s == Node.Status.SUCCESS)               // Agent made it to the door
        {
            if (!door.GetComponent<Lock>().isLocked)  // If door is not locked
            {
                door.SetActive(false);    // Get rid of the door as an obstacle
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;     // otherwise return failure and continue to next door
        }
        else
            return s;
    }

    public Node.Status RobItem(GameObject steal)
    {
        Node.Status s = GoToLocation(steal.transform.position);  // Sends agent to item to steal
        if (s == Node.Status.SUCCESS)   // Agent made it to the item
        {
            if (!steal.GetComponent<Stolen>().isStolen)
            {
                steal.GetComponent<Stolen>().isStolen = true;
                steal.transform.parent = this.transform;
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        else
            return s;
    }

    // Traversing NavMesh

    Node.Status GoToLocation(Vector3 destination) // Method to go to destination and return correct Status of Agent and Node
    {
        float distanceToTarget = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distanceToTarget < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;
    }

    // Update is called once per frame
    void Update()
    {
        if (treeStatus != Node.Status.SUCCESS)
            treeStatus = tree.Process();
    }
}
