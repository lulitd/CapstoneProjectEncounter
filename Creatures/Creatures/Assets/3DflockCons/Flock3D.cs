using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flock3D : MonoBehaviour {

	public List<Boid3D> boids;
	public List<AttrPt3D> attrPts;
	public int boidsSize, attrPtsSize;

	// forces
	public float forceSeparate, forceAlign, forceCohesion;
	public float radiusSeparation, radiusAlign, radiusCohesion;
	public float maxTurn, maxSpeed, maxForce;

	// bounds
	public float minX, minY, minZ, maxX, maxY, maxZ, boundsWidth, boundsHeight, boundsDepth;
	public int boundmode;

	// not sure what you are but keeping you here for now
	public float dt;
	public float attraction, attractionDeviation;

	public Flock3D(){
		forceSeparate = forceAlign = forceCohesion = radiusSeparation = radiusAlign = radiusCohesion = 
				maxTurn = maxSpeed = maxForce =
					minX = minY = maxX = maxY = boundsWidth = boundsHeight = 0.0f;
		minZ = maxZ = boundsDepth = 0.0f;
		boundmode = 0;
		dt = 1.0f;
		attraction = attractionDeviation = 0.0f;
	}

	public void clear(){
		clearBoids ();
		clearAttrPts ();
	}

	public void clearBoids(){
		while(boidsSize > 0)	boids.Clear();
	}

	public void clearAttrPts(){
		while(attrPtsSize > 0)	attrPts.Clear();
	}


	public Flock3D setup(int num, float lx, float ly, float lz, float dev){
		for(int i = 0; i < num; i++){
			Boid3D b = new Boid3D(this);
			
			// boids need to be scatters or else doesn't work
			b.setLoc(lx + Random.Range(-dev, dev),
			         ly + Random.Range(-dev, dev),
			         lz + Random.Range(-dev, dev));
			
			boids.Add(b);
		}
		defaultValues ();
		return this;
	}

	public void defaultValues(){
		boundmode = 0;
		forceSeparate = 55.0f;
		forceAlign = 12.0f;
		forceCohesion = 7.0f;

		radiusSeparation = 50.0f;
		radiusAlign = 100.0f;
		radiusCohesion = 75.0f;

		maxSpeed = 2.0f;
		maxForce = 10000.0f;
		attraction = 1.0f;
		attractionDeviation = 0.0f;

		setBounds (0, 0, -700, 700, 700, 0);
	}

	public Flock3D add(float lx, float ly, float lz){
		Boid3D b = new Boid3D ();
		b.setFlock (this);
		b.setLoc (lx, ly, lz);
		b.attr = attraction + Random.Range (-attractionDeviation, attractionDeviation);
		boids.Add (b);
		return this;
	}

	// --------------------------------------------------------Setters------------------
	// not sure what does "dt" mean so leave you here first
	public Flock3D setDt(float d){
		dt = d;
		return this;
	}

	public Flock3D setMaxTurn(float mt){
		maxTurn = mt;
		return this;
	}

	public Flock3D setMaxSpeed(float ms){
		maxSpeed = ms;
		return this;
	}

	public Flock3D setMaxForce(float mf){
		maxForce = mf;
		return this;
	}
	
	public Flock3D setAttraction(float a){
		attraction = a;
		doAttraction ();
		return this;
	}

	public void doAttraction(){
		for(int i=0; i<boidsSize; i++){
			boids[i].attr = attraction + Random.Range(-attractionDeviation, attractionDeviation);
		}
	}

	public Flock3D setSeparate(float sForce){
		this.forceSeparate = sForce;
		return this;
	}

	public Flock3D setAlign(float aForce){
		this.forceAlign = aForce;
		return this;
	}

	public Flock3D setCohesion(float cForce){
		this.forceCohesion = cForce;
		return this;
	}

	public Flock3D setradiusSeparation(float sDist){
		this.radiusSeparation = sDist;
		return this;
	}

	public Flock3D setradiusAlign(float aDist){
		this.radiusAlign = aDist;
		return this;
	}

	public Flock3D setradiusCohesion(float cDist){
		this.radiusCohesion = cDist;
		return this;
	}

	public Flock3D setBoundmode(int mode){
		this.boundmode = mode;
		return this;
	}

	public Flock3D setBounds(float minx, float miny, float minz, float maxx, float maxy, float maxz){
		minX = minx;
		minY = miny;
		minZ = minz;
		maxX = maxx;
		maxY = maxy;
		maxZ = maxx;
		boundsWidth = maxX - minX;
		boundsHeight = maxY - minY;
		boundsDepth = maxZ = minZ;
		return this;
	}

	// --------------------------------------------------------Getters------------------
	public float getMaxTurn(){	return maxTurn;	}
	public float getMaxSpeed(){	return maxSpeed;	}
	public float getMaxForce(){	return maxForce;	}

	public float getSeparate(){	return forceSeparate;	}
	public float getAlign(){	return forceAlign;	}
	public float getCohesion(){	return forceCohesion;	}

	public float getradiusSeparation(){return radiusSeparation;	}
	public float getradiusAlign(){	return radiusAlign;	}
	public float getradiusCohesion(){	return radiusCohesion;	}

	public int getBoundmode(){	return boundmode;	}

	public int size(){	return boids.Count;	}

	public Boid3D getBoid(int idx){	return boids [idx]; }

	// update
	public void update(){
		boidsSize = boids.Count;
		for(int i=0; i<boidsSize; i++){
			boids[i].update(dt);
		}
	}

	//	attraction points
	public Flock3D addAttrPt(float x, float y, float z, float force, float sensorDist){
		AttrPt3D ap = new AttrPt3D (x, y, z, force, sensorDist);
		attrPts.Add (ap);
		return this;
	}

	public List<AttrPt3D> getAttrPts(){		return attrPts;	}

	public bool hasAttrPts(){	return attrPts.Count > 0;	}

	public void changeAttrPt(int id, float x, float y, float z, float force, float sensorDist){
		AttrPt3D ap = attrPts [id];
		if (ap != null) {
			ap.x = x;
			ap.y = y;
			ap.z = z;
			ap.force = force;
			ap.sensorDist = sensorDist;
		} else {
			Debug.Log("attaction point3D is null at id: " + id + "/n");
		}
	}

	public void removeFirstBoid(){
		if(boids.Count > 0){
			boids.RemoveAt(0);
		}
	}

	public void removeLastBoid(){
		if(boids.Count > 0){
			boids.RemoveAt(boids.Count-1);
		}
	}

	public void removeBoid(int idx){
		if(boids.Count > 0){
			boids.RemoveAt(idx);
		}
	}

	public void removeAttrPt(int idx){
		if(attrPts.Count > 0){
			attrPts.RemoveAt(idx);
		}
	}

}
