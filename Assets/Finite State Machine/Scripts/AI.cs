using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This script will be attached to the AI character/NPC
public class AI : MonoBehaviour
{
    public Transform player;        // reference to human player character, will be set from inspector
    private NavMeshAgent agent;     // reference to the navigation mesh agent controlling the AI char
    private Animator anim;      // reference to animator controlling AI char's animations
    private State currentState;     // reference to the state the AI is currently in

    // Start is called before the first frame update
    void Start()
    {
        // initialize private variables
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Set AI's starting state as Idle
        currentState = new Idle(this.gameObject, agent, anim, player);
    }

    // Update is called once per frame
    void Update()
    {
        // go through the state and automatically update current state based on game play behaviour 
        currentState = currentState.Process();
    }
}
