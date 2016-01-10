using UnityEngine;
using System.Collections;

public class Prey : MonoBehaviour {
	public float radius;
	public GameObject predator;


	// TODO decrease radius when eaten

	// Use this for initialization
	void Start () {
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
