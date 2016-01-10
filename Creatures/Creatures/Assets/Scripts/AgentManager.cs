using UnityEngine;
using System.Collections;

public class AgentManager : MonoBehaviour {
	public GameObject boidPrefab;
	private Steer2D.Seek leadBoid;
	public int population = 50;
	private GameObject target;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Foods");

		for (int i=0; i<population; i++) {
			if(i==0){
				leadBoid = Instantiate(boidPrefab, new Vector3(i * 2.0f, Random.Range(-5.0f, 5.0f), 0), Quaternion.identity) as Steer2D.Seek;
			}
			Instantiate(boidPrefab, new Vector3(i * 2.0f, Random.Range(-5.0f, 5.0f), 0), Quaternion.identity);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			target.SendMessage("addSeeker",leadBoid);
		}
	}
}
