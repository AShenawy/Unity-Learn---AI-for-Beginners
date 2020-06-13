using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    // setup inherited constructor
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;  // set name of this state
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");  // trigger the idle animation
        base.Enter();
    }

    public override void Update()
    {
        // Set a check to exit the Update stage and go to next one (Exit), otherwise will be stuck in Update forever
        // This condition creates 10% chance to exit Update stage
        if(Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        base.Update();
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");    // Reset the triggert set to make sure it's clear before leaving state
        base.Exit();
    }

}
