using UnityEngine;
using System.Collections;

public class demoManager : MonoBehaviour {

	[SerializeField]
	int _playersToInitialize = 30;

	[SerializeField]
	GameObject _prefab;

	// Use this for initialization
	void Start () {

		for (int i = 0; i < _playersToInitialize; i++)
		{
			Instantiate(_prefab);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
