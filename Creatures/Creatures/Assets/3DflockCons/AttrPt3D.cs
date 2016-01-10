using UnityEngine;
using System.Collections;

public class AttrPt3D : MonoBehaviour {
	
	public float x, y, z, force, sensorDist;

	public AttrPt3D(){
		x = y = z = force = sensorDist = 0.0f;
	}

	public AttrPt3D(float _x, float _y, float _z, float _f, float _s){
		x = _x;
		y = _y;
		z = _z;
		force = _f;
		sensorDist = _s;
	}

}
