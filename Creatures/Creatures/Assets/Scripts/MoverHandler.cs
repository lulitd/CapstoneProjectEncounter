using UnityEngine;
using System.Collections;

public class MoverHandler : MonoBehaviour {
	public GameObject prey;
	public float mSpeed = 3.0f;

//	private float nextActionTime = 0.0f;
//	public float wonderPeriod = 0.5f;
	
	void Start () {
	}
	
	void Update () {
		checkForMovement ();
	}
	
	void checkForMovement(){

		transform.LookAt(prey.transform.position);

		Vector3 difference = prey.transform.position - transform.position;

		transform.position += (difference * mSpeed * Time.deltaTime);

//		Collider[] colls = Physics.OverlapSphere (transform.position, .01f);
//		foreach (Collider coll in colls) {
//			coll.transform.RotateAround(prey.transform.position, Vector3.up, 60 * Time.deltaTime);
//			Debug.Log ("i can feel it");

//			while(Time.time > nextActionTime){
//				nextActionTime = Time.time + wonderPeriod;
//				GameObject obj = gameObject.GetComponentInChildren<GameObject>();
//				foreach(GameObject obj in objs){
//					obj.GetComponent<Rigidbody>().isKinematic = true;
//				}
//
//			}
//		}
		
	}


	
//	void OnCollisionEnter(Collision collision) {
//		Debug.Log ("i can feel it");
////		transform.RotateAround (prey.transform.position, Vector3.up, 20 * Time.deltaTime);
//	}
}


