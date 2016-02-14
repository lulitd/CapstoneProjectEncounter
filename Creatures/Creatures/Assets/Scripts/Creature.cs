using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnitySteer.Behaviors;

[RequireComponent(typeof (AutonomousVehicle))]
public class Creature : MonoBehaviour {

	#region parameters
	public enum CreatureState{
		Neutral,	// high energy level
		Pursuer,	// medium energy level
		Prey		// low energy level
	}

	private CreatureState _state = CreatureState.Neutral;

	private float _energyLevel = 100.0f;	// TODO connect energylevel with state

	private Material _baseMaterial;

	[SerializeField] private float _originalRadius = 0.3f;
	[SerializeField] private float _sizeVariance = 0.2f;

	[SerializeField] private Renderer _renderer;
	[SerializeField] private Material _glowMaterial;
	[SerializeField] private Material _preyMaterial;
	#endregion

	public AutonomousVehicle Vehicle { get; set; }
	public SteerForPursuit ForPursuit { get; private set; }
	public SteerForNeighborGroup ForNeighbors { get; private set; }
	public SteerForWander ForWander { get; private set; }
	public SteerForEvasion ForEvasion { get; private set; }

	public float OriginalSpeed { get; private set; }
	public float OriginalTurnTime { get; private set; }

	public CreatureState State{
		get { return _state; }
		set { SetState(value); }
	}

	public TrailRenderer Trail { get; private set; }

	public void ChangeSize(float percent){
		Vehicle = GetComponent<AutonomousVehicle>();
		Vehicle.MaxSpeed *= 1 + percent;				// bigger size, faster max speed
		Vehicle.TurnTime *= 1 - (2 * percent);			// bigger size, slower turn time
		Vehicle.Transform.localScale *= 1 - (2 * percent);	// change local scale
		Vehicle.ScaleRadiusWithTransform(_originalRadius);	// scale original radius

		OriginalSpeed = Vehicle.MaxSpeed;
		OriginalTurnTime = Vehicle.TurnTime;
	}

	public void Grow(){		// show a zoom in/zoom out scaling when creature grow
		ChangeSize(-0.4f);
		_renderer.material = _glowMaterial;
		var tween = Go.to(transform, 0.3f, new GoTweenConfig().scale(2f, true).setIterations(8, GoLoopType.PingPong));
		tween.setOnCompleteHandler(x => _renderer.material = _baseMaterial);
	}

	public void Die(){ // TODO maybe instead of destroy, disable object to improve performance?
		transform.scaleTo(1.5f, 0.1f).setOnCompleteHandler(x => Destroy(gameObject)); 
	}

	// initialize components
	private void Awake(){
		_sizeVariance = Mathf.Clamp(_sizeVariance, 0, 0.45f);

		var difference = Random.Range(-_sizeVariance, _sizeVariance);
		ChangeSize(difference);

		ForPursuit = GetComponent<SteerForPursuit>();
		ForNeighbors = GetComponent<SteerForNeighborGroup>();
		ForWander = GetComponent<SteerForWander>();
		ForEvasion = GetComponent<SteerForEvasion>();

		ForPursuit.OnArrival += OnReachPrey;

		Trail = GetComponent<TrailRenderer>();
		_baseMaterial = _renderer.material;
	}

	// check energy state to determine state
	private void checkState(){
		if (_energyLevel <= 10.0f) { // death?
			Debug.Log("energy level < 10");

		} else if (_energyLevel > 10.0f && _energyLevel < 20.0f) { // prey
			_state = CreatureState.Prey;
		} else if (_energyLevel >= 20.0f && _energyLevel < 30.0f) { // pursuer
			_state = CreatureState.Pursuer;
		} else if (_energyLevel > 30.0f && _energyLevel < 100.0f) { // neutral
			_state = CreatureState.Neutral;
		}
	}

	private void SetState(CreatureState state){
		Vehicle.MaxSpeed = OriginalSpeed;
		Vehicle.TurnTime = OriginalTurnTime;
		_state = state;
		switch (_state){
		case CreatureState.Neutral:
			_renderer.material = _baseMaterial;
			_renderer.material.color = Color.white;
			break;
		case CreatureState.Prey:
			Vehicle.MaxSpeed *= 1.75f;
			Vehicle.TurnTime *= 0.95f;
			_renderer.material = _preyMaterial;
			_renderer.material.color = Color.yellow;
			break;
		case CreatureState.Pursuer:
			Vehicle.MaxSpeed *= 2f;
			_renderer.material.color = Color.red;
			break;
		}
		ForPursuit.enabled = State == CreatureState.Pursuer;
		ForWander.enabled = State == CreatureState.Neutral;
		ForNeighbors.enabled = State != CreatureState.Prey;
		ForEvasion.enabled = State == CreatureState.Prey;
		Trail.enabled = State == CreatureState.Prey;
	}

	private void OnEnable(){	// TODO test
		CreatureManager.Instance.Creatures.Add(this);
	}

	private void OnDisable(){	// TODO test
		// Debug.Log(string.Format("{0} OnDisable called on {1}", Time.time, this));
		CreatureManager.Instance.Creatures.Remove(this);
	}

	private void OnReachPrey(Steering steering){	// TODO
		CreatureManager.Instance.CapturedPrey(this);

	}
	
	// Update is called once per frame
	void Update () {
		checkState ();
		if (State == CreatureState.Prey)
		{
			_energyLevel -= 0.7f;
			var closest =
				CreatureManager.Instance.Creatures.Where(x => x != this)
					.OrderBy(x => (x.Vehicle.Position - Vehicle.Position).sqrMagnitude)
					.First();
			ForEvasion.Menace = closest.Vehicle;

		}else if (State == CreatureState.Neutral){
			_energyLevel -= 0.5f;


		}else if (State == CreatureState.Pursuer){
			_energyLevel -= 1.0f;
			Debug.Log ("creature update: energyLevel: " + _energyLevel);
		}
	}


}
