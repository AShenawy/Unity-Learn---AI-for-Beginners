using UnityEngine;
using System.Collections;
using UnityEngine.AI;

// This class is responsible for the AI attacking state
public class Attack : State
{
    private float rotationSpeed = 7f;   // how fast the AI rotates to look at player
    private AudioSource shootAudio;

    // setup inherited constructor
    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;    // set the name of this state
        agent.isStopped = true;     // set whether the npc will move or stop while in this state 
        shootAudio = npc.GetComponent<AudioSource>();    // reference to the audio component on AI character
    }

    public override void Enter()
    {
        anim.SetTrigger("isShooting");  // trigger attack animation
        shootAudio.Play();   // play the shooting sfx
        base.Enter();
    }

    public override void Update()
    {
        // get the directional vector between AI and player
        Vector3 direction = player.position - npc.transform.position;
        // remove vertical component value to avoid tilting AI character
        direction.y = 0;

        // rotate AI character to face player while shooting
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

        // if player is out of AI's attack range but AI can still see them, then move to chasing state
        if (!CanAttackPlayer() && CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        // if AI lost sight of the player, then exit state and go idle
        else if (!CanSeePlayer())
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");    // Reset the triggert set to make sure it's clear before leaving state
        shootAudio.Stop();      // stop playing shooting sfx
        base.Exit();
    }
}
