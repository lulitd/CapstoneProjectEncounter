using UnityEngine;
using System.Collections;

public class GhostStateMachine : MonoBehaviour {

	enum States{
		OBSERVED,
		UNOBSERVED
	};
	States mCurrentState = States.UNOBSERVED;

	public GameObject mCreature;
	private CreatureMovementHandler creatureScript;
	private GhostChase ghostScript;

	void Start () {
		creatureScript = mCreature.GetComponent<CreatureMovementHandler>();
		ghostScript = GetComponent<GhostChase>();

		changeState (States.OBSERVED);
	}
	

	void Update () {
		if (transform.position.x <= mCreature.transform.position.x &&
			creatureScript.getLeft ()) {
			changeState (States.OBSERVED);
		} else if (transform.position.x >= mCreature.transform.position.x &&
			creatureScript.getRight ()) {
			changeState (States.OBSERVED);
		} else {
			changeState(States.UNOBSERVED);
		}
	}

	void changeState(States newState){
		if (mCurrentState == newState) {
			return;
		}

		switch(newState){
			case States.OBSERVED:{
				ghostScript.enabled = false;
				break;
			}
			case States.UNOBSERVED:{
				ghostScript.enabled = true;
				break;
			}
			default:{
				return;
			}
		}
		mCurrentState = newState;
	}
}
