using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/**
A Finite State Machine System based on Chapter 3.1 of Game Programming Gems 1 by Eric Dybsand
 
Written by Roberto Cezar Bianchini, July 2010
 
 
How to use:
	1. Place the labels for the transitions and the states of the Finite State System
	    in the corresponding enums.
 
	2. Write new class(es) inheriting from FSMState and fill each one with pairs (transition-state).
	    These pairs represent the state S2 the FSMSystem should be if while being on state S1, a
	    transition T is fired and state S1 has a transition from it to S2. Remember this is a Deterministic FSM. 
	    You can't have one transition leading to two different states.
 
	   Method Reason is used to determine which transition should be fired.
	   You can write the code to fire transitions in another place, and leave this method empty if you
	   feel it's more appropriate to your project.
 
	   Method Act has the code to perform the actions the NPC is supposed do if it's on this state.
	   You can write the code for the actions in another place, and leave this method empty if you
	   feel it's more appropriate to your project.
 
	3. Create an instance of FSMSystem class and add the states to it.
 
	4. Call Reason and Act (or whichever methods you have for firing transitions and making the NPCs
	     behave in your game) from your Update or FixedUpdate methods.
 
	Asynchronous transitions from Unity Engine, like OnTriggerEnter, SendMessage, can also be used, 
	just call the Method PerformTransition from your FSMSystem instance with the correct Transition 
	when the event occurs.
 
 
 
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE 
AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
 
 
/// <summary>
/// Place the labels for the Transitions in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum Transition
{
    NullTransition = 0, // Use this transition to represent a non-existing transition in your system
	Chase,
	Follow,
	SawPlayer,
	LostPlayer,
	NoHealth,
}
 
/// <summary>
/// Place the labels for the States in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum StateID
{
    NullStateID = 0, // Use this ID to represent a non-existing State in your system	
	ChasingPlayer,
	FollowingPath,
	Attacking,
	Dead,
}
 
/// <summary>
/// This class represents the States in the Finite State System.
/// Each state has a Dictionary with pairs (transition-state) showing
/// which state the FSM should be if a transition is fired while this state
/// is the current state.
/// Method Reason is used to determine which transition should be fired .
/// Method Act has the code to perform the actions the NPC is supposed do if it's on this state.
/// </summary>
public abstract class FSMState
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateID;
    public StateID ID 
	{ 
		get 
		{ 
			return stateID; 
		} 
	}
 
    public void AddTransition(Transition trans, StateID id)
    {
        // Check if anyone of the args is invalid
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed for a real transition");
            return;
        }
 
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
            return;
        }
 
        // Since this is a Deterministic FSM,
        //   check if the current transition was already inside the map
        if (map.ContainsKey(trans))
        {
            Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has transition " + trans.ToString() + 
                           "Impossible to assign to another state");
            return;
        }
 
        map.Add(trans, id);
    }
 
    /// <summary>
    /// This method deletes a pair transition-state from this state's map.
    /// If the transition was not inside the state's map, an ERROR message is printed.
    /// </summary>
    public void DeleteTransition(Transition trans)
    {
        // Check for NullTransition
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERROR: NullTransition is not allowed");
            return;
        }
 
        // Check if the pair is inside the map before deleting
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERROR: Transition " + trans.ToString() + " passed to " + stateID.ToString() + 
                       " was not on the state's transition list");
    }
 
    /// <summary>
    /// This method returns the new state the FSM should be if
    ///    this state receives a transition and 
    /// </summary>
    public StateID GetOutputState(Transition trans)
    {
        // Check if the map has this transition
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }
 
    /// <summary>
    /// This method is used to set up the State condition before entering it.
    /// It is called automatically by the FSMSystem class before assigning it
    /// to the current state.
    /// </summary>
    public virtual void DoBeforeEntering() { }
 
    /// <summary>
    /// This method is used to make anything necessary, as reseting variables
    /// before the FSMSystem changes to another one. It is called automatically
    /// by the FSMSystem before changing to a new state.
    /// </summary>
    public virtual void DoBeforeLeaving() { } 
 
    /// <summary>
    /// This method decides if the state should transition to another on its list
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    public abstract void Reason(GameObject player, GameObject npc);
 
    /// <summary>
    /// This method controls the behavior of the NPC in the game World.
    /// Every action, movement or communication the NPC does should be placed here
    /// NPC is a reference to the object that is controlled by this class
    /// </summary>
    public abstract void Act(GameObject player, GameObject npc);
 
} // class FSMState
 
 