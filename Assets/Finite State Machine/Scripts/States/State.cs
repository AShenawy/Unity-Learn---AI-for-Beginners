using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This script acts as base for the state machine for AI character
public class State
{
    public enum STATE { IDLE, PATROL, PURSUE, ATTACK, SLEEP };  // set enum for all AI states in game play

    public enum EVENT { ENTER, UPDATE, EXIT };  // reference to the 3 stages a state node can be in

    public STATE name;  // reference to the state's name (for child classes)
    protected EVENT stage;  // reference to current stage the state is in
    protected GameObject npc;   // reference to the AI npc/character
    protected NavMeshAgent agent;   // reference to the navigation mesh agent controlling the AI char
    protected Animator anim;    // reference to animator controlling AI char's animations
    protected Transform player;     // reference to human player character
    protected State nextState;  // reference to the AI's next state

    private float visDist = 10f;    // distance at which AI 'sees' the player
    private float visAngle = 45f;   // angle range at which AI can see player in front of it. This value is half the complete view triangle
    private float shootDist = 7f;   // distance at which AI will start attacking/shooting player
    
    // setup class constructor for required components when calling class and/or its children
    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    // The Enter, Update and Exit methods are meant to be overridden by child classes
    public virtual void Enter() { stage = EVENT.UPDATE; }   // ensure the stage moves to Update after performing all functionality

    public virtual void Update() { stage = EVENT.UPDATE; }  // Update should stay running until it is forced to move to another stage

    public virtual void Exit() { stage = EVENT.EXIT; }  // ensure the stage is set to Exit when reaching state's end to perform necessary exit behaviour

    // Perfom the functionality depending on which stage the state is at
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
        
        return this;    // If not exiting the state, return itself
    }

    public bool CanSeePlayer()
    {
        // calculate player location and angle relative to AI character
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        // if player is within visibility distance & view angle, then AI can see player
        if (direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }

        return false;
    }

    public bool CanAttackPlayer()
    {
        // calculate how far the player is from AI character
        float playerDist = (player.position - npc.transform.position).magnitude;
        
        if (playerDist < shootDist)     // if player is close enough, AI can shoot them
            return true;

        return false;
    }
}
