using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This class is responsible for the AI patrolling state
public class Patrol : State
{
    private int currentIndex;      // reference to the waypoints index counter

    // setup inherited constructor
    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;    // set the name of this state
        agent.speed = 2f;   // set speed of the navmesh agent when patrolling
        agent.isStopped = false;    // set whether the npc will move or stop along the waypoint path 
    }

    public override void Enter()
    {
        currentIndex = 0;       // set the waypoint index to start at beginning of patrol path
        anim.SetTrigger("isWalking");   // trigger the animation for walking
        base.Enter();
    }

    public override void Update()
    {
        // check if AI agent has arrived to a waypoint
        if (agent.remainingDistance < 1)
        {
            // check if the npc has finished walking through all waypoints
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
                currentIndex = 0;   // Reset the patrol path
            else
                currentIndex++;     // set next waypoint as destination

            // move the npc agent to waypoint at the current index count
            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }

        // if AI saw the player, then exit patrol state and start chasing
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");    // Reset the triggert set to make sure it's clear before leaving state
        base.Exit();
    }
}
