using UnityEngine;
using System.Collections;

public class BoidManager : MonoBehaviour {

	public Transform boid;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < 10; i++){
			Instantiate (boid, new Vector3(i * 2.0f, 2.0f, 0), Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
