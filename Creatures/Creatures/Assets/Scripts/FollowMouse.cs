using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
	RaycastHit hit;
	private Camera cam;
	
	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mouse = Input.mousePosition;
		Vector3 pos = cam.ScreenToWorldPoint (new Vector3(mouse.x, mouse.y, 0.0f));

		transform.position = pos;

//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		if(Physics.Raycast(ray, out hit)) {
//			Vector3 pos = hit.point;
//			pos.Set(hit.point.x, hit.point.y, -8.5f);
//			transform.position = pos;
////			transform.LookAt(hit.point);
//		}
//		else {
//			Vector3 pos = ray.origin + ray.direction * -8.5f;
//			pos.Set(pos.x, pos.y, -8.5f);
//			transform.position = pos;
////			transform.LookAt(ray.origin + ray.direction * -8.5f);
//		}
	}
}
