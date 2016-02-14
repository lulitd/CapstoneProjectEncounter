using UnityEngine;
using System.Collections;

public class CreatureMovementHandler : MonoBehaviour {

	public float mSpeed = 3.0f;
	private bool mFacingLeft;
	private bool mFacingRight;

	void Start () {
		mFacingRight = true;
		mFacingLeft = !mFacingRight;
	}
	
	void Update () {
		checkForMovement ();
	}

	void checkForMovement(){
		float hVal = Input.GetAxis ("Horizontal");

		// update which direction is facing
		if(hVal < 0){
			if(mFacingRight){
				transform.Rotate(0, 180, 0);
				mFacingRight = !mFacingRight;
				mFacingLeft = !mFacingLeft;
			}

		}else if(hVal > 0){
			if(mFacingLeft){
				transform.Rotate(0, 180, 0);
				mFacingRight = !mFacingRight;
				mFacingLeft = !mFacingLeft;
			}
		}

		transform.position += new Vector3 (hVal * mSpeed * Time.deltaTime, 0.0f, 0.0f);

	}

	public bool getLeft(){
		return mFacingLeft;
	}

	public bool getRight(){
		return mFacingRight;
	}
}


