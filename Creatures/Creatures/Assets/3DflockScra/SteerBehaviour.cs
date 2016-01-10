using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteerWiggler))]
public abstract class SteerBehaviour : MonoBehaviour {

	public float Weight = 1;

	protected SteerWiggler wiggler;

	public abstract Vector3 GetVelocity();


	// Use this for initialization
	void Start () {
		wiggler = GetComponent<SteerWiggler>();
		wiggler.RegisterSteerBehaviour(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onDestroy(){
		if (wiggler != null)
			wiggler.DeregisterSteerBehaviour(this);
	}
}
