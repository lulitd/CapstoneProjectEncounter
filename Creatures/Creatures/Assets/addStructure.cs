using UnityEngine;
using System.Collections;

public class addStructure : MonoBehaviour {

//	public static int positionNum;
	public int positionNum;

	public GameObject prefab;

	Vector3 loc1, loc2, loc3, loc4;
	Vector3 spwanLoc;

	GameObject thisParticles;

	// Use this for initialization
	void Start () {
		loc1 = new Vector3 (8, 6, 0);
		loc2 = new Vector3 (-8, 6, 0);
		loc3 = new Vector3 (-8, -6, 0);
		loc4 = new Vector3 (8, -6, 0);

		thisParticles = (GameObject)Instantiate (prefab, spwanLoc, Quaternion.identity);	

	}

	public void setLocationNum(int loc){
		positionNum = loc;
		Debug.Log ("location is set via function : " + positionNum);
//
//		var script = gameObject.GetComponent<addStructure>();
//		script.setLocationNum ();
	}
	
	// Update is called once per frame
	void Update () {
		// check for incoming message
		// positionNum = incomingMess;

		if (positionNum == 1) {
//			Debug.Log("position 1");
			spwanLoc = loc1;
		} else if (positionNum == 2) {
//			Debug.Log("position 2");
			spwanLoc = loc2;
		} else if (positionNum == 3) {
//			Debug.Log("position 3");
			spwanLoc = loc3;
		} else if (positionNum == 4) {
//			Debug.Log("position 4");
			spwanLoc = loc4;
		} else {
//			Debug.Log("spawnLoc is not assigned :< set to origin");
//			spwanLoc = new Vector3(0, 0, 0); // set to a far away position
			spwanLoc = loc1;
		}
//		Debug.Log("position: " + positionNum);
		thisParticles.gameObject.transform.position = spwanLoc;
	}
}
