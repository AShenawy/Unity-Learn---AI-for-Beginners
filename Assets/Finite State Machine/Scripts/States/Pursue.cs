using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

// This class is responsible for the AI pursuing/chasing state
public class Pursue : State
{
    // setup inherited constructor
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;    // set the name of this state
        agent.speed = 5f;       // set speed of the navmesh agent when pursuing
        agent.isStopped = false;    // set whether the npc will move or stop while in this state 
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");  // trigger the running animation
        base.Enter();
    }

    public override void Update()
    {
        // start chasing the player
        agent.SetDestination(player.position);

        // check if AI character has finished setting a path to player 
        if (agent.hasPath)
        {
            // check if AI is within attack range of player, then move to Attack state
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            // if AI lost sight of the player, then exit pursue stat and go Idle
            else if (!CanSeePlayer())
            {
                nextState = new Idle(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");    // Reset the triggert set to make sure it's clear before leaving state
        base.Exit();
    }
}
