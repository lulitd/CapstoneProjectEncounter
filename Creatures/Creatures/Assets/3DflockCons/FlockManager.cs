using UnityEngine;
using System.Collections;

public class FlockManager : MonoBehaviour {

	public Flock3D flock;

	private Camera cam;
	private float scrWidth, scrHeight;

	[Range (0, 400)]	public float maxForce = 400.0f;
	[Range (0, 30)]		public float sensorDist = 30;

	// Use this for initialization
	void Start () {

		cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		scrWidth = cam.pixelWidth;
		scrHeight = cam.pixelHeight;

		// attraction points
		for(int i=0; i<10; i++){
			float x = Random.Range(0,scrWidth);
			float y = Random.Range(0,scrHeight);
			float z = Random.Range(0, 10);
			float force = Random.Range (-maxForce, maxForce);
			flock.addAttrPt (x, y, z, force, sensorDist);
		}

		// flock
		flock.setup (20, scrWidth/2, scrHeight/2, scrWidth/2, 100);

		flock.setBounds (0, 0, -scrWidth, scrWidth, scrHeight, 0);

		flock.setBoundmode (1).setDt (13.5f);

	}
	
	// Update is called once per frame
	void Update () {
		flock.update ();
	}
}
