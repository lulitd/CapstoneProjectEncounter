using UnityEngine;
using System.Collections;

public class GhostChase : MonoBehaviour {

	public GameObject mCreature;

	void Start () {
	
	}
	
	void Update () {
		move();
	}

	void move(){
		Vector3 creaturePosition = mCreature.transform.position;
		Vector3 myPosition = transform.position;

		Vector3 difference = myPosition - creaturePosition;

		transform.Translate (
			(difference.x / 100) * -1,
			(difference.y / 100) * -1,
			(difference.z / 100) * -1
		);
	}
}
