using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class NPCControl : MonoBehaviour
{
    public GameObject player;
    public Transform[] path;
    private FSMSystem fsm;
 
    public void SetTransition(Transition t) { fsm.PerformTransition(t); }
 
    public void Start()
    {
        MakeFSM();
    }
 
    public void FixedUpdate()
    {
        fsm.CurrentState.Reason(player, gameObject);
        fsm.CurrentState.Act(player, gameObject);
    }
 
	// The NPC has two states: FollowPath and ChasePlayer
	// If it's on the first state and SawPlayer transition is fired, it changes to ChasePlayer
	// If it's on ChasePlayerState and LostPlayer transition is fired, it returns to FollowPath
    private void MakeFSM()
    {
        FollowPathState follow = new FollowPathState(path);
        follow.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);
 
        ChasePlayerState chase = new ChasePlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowingPath);
 
        fsm = new FSMSystem();
        fsm.AddState(follow);
        fsm.AddState(chase);
    }
}

