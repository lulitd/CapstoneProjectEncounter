using UnityEngine;
using System.Collections;


/*
	An example of choppy rotation
 */
public class RandomWiggle : MonoBehaviour {
	private int count;

	public float minX, maxX, minY, maxY, minZ, maxZ;

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		count = 0;
		rb = this.GetComponentInParent<Rigidbody>();

		rb.maxAngularVelocity = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {

//		rotating around z axis
//		transform.RotateAround (Vector3.zero, Vector3.up, Random.Range(5,20)*Time.deltaTime);

		// change to a random rotation every 100 counts
		if (count % 100 == 1) {

//			Debug.Log("angular vel: " + rb.angularVelocity);

//			transform.rotation = Random.rotation;
			transform.rotation = Quaternion.Slerp(transform.rotation, Random.rotation, Time.time * 2);
			count = count - 100;
		}
//		transform.Translate (Vector3.forward * Time.deltaTime);

		count ++;
	}

}
