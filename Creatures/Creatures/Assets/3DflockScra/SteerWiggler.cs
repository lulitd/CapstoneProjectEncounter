using UnityEngine;
using System.Collections.Generic;

public class SteerWiggler : MonoBehaviour {

//	public float mass = 10;	//TODO add mass
//	public float friction = .05f; // maybe add later
	
	public Vector3 currentLoc;
	public Vector3 currentVel;
	public Vector3 currentAcc;
	public float maxSpeed = 4.0f;
	public float maxForce = 1.0f;

	// for bounds
	public int boundmode = 1;
	public float minX = -5.0f;
	public float maxX = 5.0f;
	public float minY = -5.0f;
	public float maxY = 5.0f;
	public float minZ = -5.0f;
	public float maxZ = 5.0f;

	public GameObject targetGameObj;

	// wander parameters
	private float wanderTheta = 0.0f;
	private float wanderPhi = 0.0f;
	private float wanderPsi = 0.0f;
	public float wanderR = 25.0f;	// radius of "wander sphere"
	public float wanderD = 10.0f;	// distance to "wander sphere" from current location
	public float wanderStep = 0.3f;

	[HideInInspector]
	public static List<SteerWiggler> AgentList = new List<SteerWiggler>();
	List<SteerBehaviour> behaviours = new List<SteerBehaviour>();
	
	public void RegisterSteerBehaviour(SteerBehaviour behaviour){ behaviours.Add(behaviour);	}
	
	public void DeregisterSteerBehaviour(SteerBehaviour behaviour){ behaviours.Remove(behaviour);	}

	// Use this for initialization
	void Start () {
		AgentList.Add (this);

		currentLoc = transform.position;
		currentVel = new Vector3 (0.0f, 0.0f, 0.0f);
		currentAcc = new Vector3 (0.0f, 0.0f, 0.0f);

		wanderTheta = 0.0f;
	}

	// Update is called once per frame
	void Update () {
		wander ();

		currentVel += currentAcc;

		// limit to maximum force
		if (currentVel.magnitude > maxSpeed)
			currentVel = currentVel.normalized * maxSpeed;
	
		//	currentLoc += currentVel;
		Debug.Log ("currentLoc: " + currentLoc);
		Debug.Log ("position x: " + transform.position.x);

		currentLoc = currentLoc + (Vector3)currentVel * Time.deltaTime;
		currentAcc *= 0;

		// adjust transform to rotate in the direction of velocity
		transform.LookAt (currentVel .normalized);
		transform.Translate (currentLoc, Space.World);

		bounds (boundmode);

//		wigglerTransform.RotateAround (Vector3.zero, Vector3.up, 20*Time.deltaTime);
	}

	// ---------------------------- steering behaviour components ---------------------------

	/* A method that calculates and applies a steering force towards a target. 
	/	STEER = DESIRED - VELOCITY
	*/
	void seek(Vector3 target){		
		Vector3 desired = target - currentVel;
		desired.Normalize ();

		desired *= maxSpeed;

		// calculate steer force
		Vector3 steer = desired - currentVel;
		steer = Vector3.ClampMagnitude (steer, maxForce);	// similar to limit in processing
		if (steer.magnitude > maxForce)
			steer = steer.normalized * maxForce;

		applyForce (steer);
	}

	/* generate arrive force for boid. "Adjust speed depending on the distance to target"
	 * */
	Vector3 arrive(Vector3 target){
		Vector3 desired = target - currentVel;
		desired.Normalize();

		float d = desired.magnitude;
		if (d < 100) {
			float m = mapValue (d, 0, 100, 0, maxSpeed);
			desired *= m;
		} else {
			desired *= maxSpeed;
		}

		Vector3 steer = desired - currentVel;
		steer = Vector3.ClampMagnitude (steer, maxForce);	// limit force
		return steer;
	}

	/* generate a random wandering force for boid. 
		result can be controlled by wanderStep, wanderR, and wanderD
	 */
	void wander(){
		//	random from AS example
		//	wanderTheta += -wanderStep + Random.Range (0, 1.0f) * wanderStep * 2;

		// random from processing example
		wanderTheta += Random.Range (-wanderStep, wanderStep);
		wanderPhi += Random.Range (-wanderStep, wanderStep);
		wanderPsi +=  Random.Range (-wanderStep, wanderStep);

		// calculate the new location to steer towards on the wander circle
		Vector3 sphereLoc = currentVel.normalized;
		sphereLoc *= wanderD;
		sphereLoc += currentLoc;	// make it relative to boid's location

		Vector3 sphereOffset = new Vector3 (wanderR * Mathf.Cos(wanderTheta), wanderR * Mathf.Sin(wanderPhi), wanderR * Mathf.Cos (wanderPsi));
		Vector3 target = sphereLoc + sphereOffset;

		seek (target);
	}


	void applyForce(Vector3 force){
		// add mass here if we want A = F / M
		currentAcc += force;
	}

	void bounds(int boundmode){
		switch (boundmode) {
		case 0:	// CLAMP, reverse direction
			if(currentLoc.x < minX){	
				currentLoc.x = minX;
				currentVel.x = -currentVel.x;
				transform.LookAt (currentVel .normalized);
			}
			if(currentLoc.x > maxX){
				currentLoc.x = maxX;
				currentVel.x = -currentVel.x;
				transform.LookAt (currentVel .normalized);
			}
			if(currentLoc.y < minY){
				currentLoc.y = minY;
				currentVel.y = -currentVel.y;
				transform.LookAt (currentVel .normalized);
			}
			if(currentLoc.y > maxY){
				currentLoc.y = minY;
				currentVel.y = -currentVel.y;
				transform.LookAt (currentVel .normalized);
			}
			if(currentLoc.z < minZ){
				currentLoc.z = minZ;
				currentVel.z = -currentVel.z;
				transform.LookAt (currentVel .normalized);
			}
			if(currentLoc.z > maxZ){	
				currentLoc.z = minZ;
				currentVel.z = -currentVel.z;
				transform.LookAt (currentVel .normalized);
			}
			break;

		case 1:	// WRAP
			if(currentLoc.x < minX){	
				currentLoc.x += (maxX - minX);
			}
			if(currentLoc.x > maxX){
				currentLoc.x -= (maxX - minX);
			}
			if(currentLoc.y < minY){
				currentLoc.y += (maxY - minY);
			}
			if(currentLoc.y > maxY){
				currentLoc.y -= (maxY - minY);
			}
			if(currentLoc.z < minZ){
				currentLoc.z += (maxZ - minZ);
			}
			if(currentLoc.z > maxZ){
				currentLoc.z -= (maxZ - minZ);
			}
			break;
		}
	}

	/* mapping an input from domain dMin to dMax to a range rMin to rMax
	 */
	float mapValue(float input, float dMin, float dMax, float rMin, float rMax){
		return (input - dMin) / (dMax - dMin) * (rMax - rMin) + rMin;
	}

}
