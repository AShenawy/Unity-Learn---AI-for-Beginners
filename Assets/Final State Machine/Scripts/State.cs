using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE { IDLE, PATROL, PURSUE, ATTACK, SLEEP };

    public enum EVENT { ENTER, UPDATE, EXIT };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected Transform player;
    protected State nextState;

    private float visDist = 10f;
    private float visAngle = 30f;
    private float shootDist = 7f;
    
    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }

    public virtual void Update() { stage = EVENT.UPDATE; }

    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if(stage == EVENT.ENTER)
            Enter();

        if (stage == EVENT.UPDATE)
            Update();

        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;       // Since this state is exiting, we need the next state that will be called
        }
        
        return this;    // If not exiting, return this state
    }
}
