using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreatureManager : MonoBehaviour {
	public static CreatureManager Instance
	{
		get { return SingletonPerLevel<CreatureManager>.Instance; }
	}

	[SerializeField]
	Creature _prefab;

	[SerializeField]
	int _playersToInitialize = 30;

	public List<Creature> Creatures = new List<Creature>();


	void Start(){
		for (int i = 0; i < _playersToInitialize; i++)
		{
			Instantiate(_prefab);
		}
	}

	public void CapturedPrey(Creature c)
	{
		// StopGame();
		Debug.Log(string.Format("{0} {1} captured prey!", Time.time, c));
		c.Grow();
		var quarry = c.ForPursuit.Quarry.GetComponent<Creature>();
		quarry.Die();
		Destroy(c.ForPursuit.Quarry.gameObject); // TODO disable instead of destroy to improve performance?
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
