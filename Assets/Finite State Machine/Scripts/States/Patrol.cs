using System;
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
        // make AI start patrol at the nearest waypoint
        float lastDist = Mathf.Infinity;
        
        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];

            // calculate the distance between AI and each waypoint
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);

            // if distance is less than last calculated distance, make it current index and current last distance to compare against
            if(distance < lastDist)
            {
                currentIndex = i - 1;   // subtract 1 since in Update we already add 1 to current index
                lastDist = distance;
            }
        }

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
