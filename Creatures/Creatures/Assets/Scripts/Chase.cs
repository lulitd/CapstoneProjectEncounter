using UnityEngine;
using System.Collections;

public class Chase : MonoBehaviour {
	public GameObject prey;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 creaturePosition = prey.transform.position;
		Vector3 myPosition = transform.position;
		
		Vector3 difference = myPosition - creaturePosition;
		
		transform.Translate (
			(difference.x / 100) * -1,
			(difference.y / 100) * -1,
			(difference.z / 100) * -1
			);
	}
}
