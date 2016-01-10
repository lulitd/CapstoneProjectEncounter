using UnityEngine;
using System.Collections;

public class Boid3D : MonoBehaviour {

	public float x, y, z, vx, vy, vz, ax, ay, az;
	int life;

	public float attr;

	public Flock3D flock;

	public Boid3D(){
		x = y = z = vx = vy = vz = ax = ay = az = 0.0f;
		life = 0;
		flock = null;
		attr = 0.0f;
	}

	public Boid3D(Flock3D _flock){
		x = y = z = vx = vy = vz = ax = ay = az = 0.0f;
		life = 0;
		attr = 0.0f;
		flock = _flock;
	}

	public Boid3D setFlock (Flock3D _flock){
		flock = _flock;
		return this;
	}

	public Boid3D setLoc(float lx, float ly, float lz){
		x = lx;
		y = ly;
		z = lz;
		return this;
	}

	public Boid3D setVel(float velx, float vely, float velz){
		vx = velx;
		vy = vely;
		vz = velz;
		return this;
	}

	public void bounds(){
		switch (flock.boundmode) {
		case 0:	// CLAMP, reverse direction
			if( x < flock.minX){
				x = flock.minX;
				vx = -vx;			
			}
			if( x > flock.maxX){
				x = flock.maxX;
				vx = -vx;			
			}

			if( y < flock.minY){
				y = flock.minY;
				vy = -vy;			
			}
			if( y > flock.maxY){
				y = flock.maxY;
				vy = -vy;			
			}

			if( z < flock.minZ){
				z = flock.minZ;
				vz = -vz;			
			}
			if( z > flock.maxZ){
				z = flock.maxZ;
				vz = -vz;			
			}
			break;
		case 1:	// WRAP, leave one side and come in from opposite side
			if( x < flock.minX){	x += flock.boundsWidth;	}
			if( x > flock.maxX){	x -= flock.boundsWidth;	}
			
			if( y < flock.minY){	y += flock.boundsHeight;}
			if( y > flock.maxY){	y -= flock.boundsHeight;}
			
			if( z < flock.minZ){	z += flock.boundsDepth;	}
			if( z > flock.maxZ){	z -= flock.boundsDepth;	}
			break;
		}
	}

	public void update(float amt){
		// reset acceleration on begin to draw
		ax = 0;
		ay = 0;
		az = 0;

		Vector3 v = new Vector3(0.0f, 0.0f, 0.0f);

		// TODO uncomment later
//		flockfull(amt, v);

		ax += v.x;
		ay += v.y;
		az += v.z;

		// limit force
		float distMaxForce = Mathf.Abs (ax) + Mathf.Abs (ay) + Mathf.Abs (az);
		if(distMaxForce > flock.maxForce){
			distMaxForce = flock.maxForce / distMaxForce;
			ax *= distMaxForce;
			ay *= distMaxForce;
			az *= distMaxForce;
		}

		vx += ax;
		vy += ay;
		vz += az;

		// limit speed
		float distMaxSpeed = Mathf.Abs (vx) + Mathf.Abs (vy) + Mathf.Abs (vz);
		if(distMaxSpeed > flock.maxSpeed){
			distMaxSpeed = flock.maxSpeed / distMaxSpeed;
			ax *= distMaxSpeed;
			ay *= distMaxSpeed;
			az *= distMaxSpeed;
		}

		x += vx * amt;
		y += vy * amt;
		z += vz * amt;

		bounds ();
	}

	public Vector3 steer(Vector3 target, float amt){	// TODO check if return is needed
		Vector3 dir = new Vector3 (0.0f, 0.0f, 0.0f);

		dir.x = target.x - x;
		dir.y = target.y - y;
		dir.z = target.z - z;

		float d = dir.magnitude;

		if (d > 2) {
			float invDist = 1.0f / d;
			dir.x *= invDist;
			dir.y *= invDist;
			dir.z *= invDist;

			// steer => desired - currentVel
			target.x = dir.x - vx;
			target.y = dir.y - vy;
			target.z = dir.z - vz;

			float distSteer = target.magnitude;
			if(distSteer > 0){
				float invDistSteer = amt / distSteer;
				target.x *= invDistSteer;
				target.y *= invDistSteer;
				target.z *= invDistSteer;
			}
		}
		return target;
	}


	public Vector3 fullFlock(float amt, Vector3 vec){
		Vector3 sep, ali, coh, attrForce;
		sep = new Vector3 (0.0f, 0.0f, 0.0f);
		ali = new Vector3 (0.0f, 0.0f, 0.0f);
		coh = new Vector3 (0.0f, 0.0f, 0.0f);
		attrForce = new Vector3 (0.0f, 0.0f, 0.0f);

		int countSep = 0, countAli = 0, countCoh= 0;

		float separateRad = flock.radiusSeparation;
		float alignRad = flock.radiusAlign;
		float cohesionRad = flock.radiusCohesion;
		float invD = 0;

		// main full loop track all forces on boid from other boids
		for (int i = 0; i < flock.boids.Count; i++) {
			Boid3D other = flock.boids[i];
			float dx = other.x - x;
			float dy = other.y - y;
			float dz = other.z - z;
			float d = Mathf.Abs (dx) + Mathf.Abs (dy) + Mathf.Abs (dz);

			if (d <= 1e-7)
				continue;
			// sep
			if (d < separateRad) {
				countSep++;
				invD = 1.0f / d;
				sep.x -= dx * invD;
				sep.y -= dy * invD;
				sep.z -= dz * invD;
			}
			
			// ali
			if (d < alignRad) {
				countAli++;
				ali.x += other.vx;
				ali.y += other.vy;
				ali.z += other.vz;
			}

			// coh
			if (d < cohesionRad) {
				countCoh++;
				coh.x += other.x;
				coh.y += other.y;
				coh.z += other.z;
			}
		}

		if (countSep > 0) {
			float invForSep = flock.forceSeparate / (float) countSep;
			sep.x *= invForSep;
			sep.y *= invForSep;
			sep.z *= invForSep;
		}
		if (countAli > 0) {
			float invForAli = flock.forceAlign / (float) countAli;
			ali.x *= invForAli;
			ali.y *= invForAli;
			ali.z *= invForAli;
		}
		if (countCoh > 0) {
			float invForCoh = flock.forceCohesion / (float) countCoh;
			coh.x *= invForCoh;
			coh.y *= invForCoh;
			coh.z *= invForCoh;
			coh = steer(coh, 1);
		}

		// other forces
		if (flock.hasAttrPts ()) {
			for (int i = 0; i < flock.attrPts.Count; i++) {
				AttrPt3D ap = flock.attrPts[i];
				float dx = ap.x - x;
				float dy = ap.y - y;
				float dz = ap.z - z;
				float d = Mathf.Abs (dx) + Mathf.Abs (dy) + Mathf.Abs (dz);

				if (d <= 1e-7)
					continue;
				if (d > ap.sensorDist)
					continue;
				
				// inbounds, calc
				float invForce = ap.force  / d  * attr;
				dx *= invForce;
				dy *= invForce;
				dz *= invForce;
				
				attrForce.x += dx;
				attrForce.y += dy;
				attrForce.z += dz;
			}
		}

		vec.x = sep.x + ali.x + coh.x + attrForce.x;
		vec.y = sep.y + ali.y + coh.y + attrForce.y;
		vec.x = sep.z + ali.z + coh.z + attrForce.z;

		float dist = vec.magnitude;
		if(dist > 0){
			float invDist = amt / dist;
			vec.x *= invDist;
			vec.y *= invDist;
			vec.z *= invDist;
		}

		vec.x *= amt;
		vec.y *= amt;
		vec.z *= amt;

		return vec;
	}

}
