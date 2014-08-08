using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script : MonoBehaviour {
	
	// Use this for initialization
	GameObject bird;
	int numberOfBirds = 30;
	float distance = 70.0f;	
	float boundary = 150.0f;
	int speed = 10;

	float alignmentWeight = 1.0f;
	float cohesionWeight = 1.0f;
	float separationWeight = 1.0f;

	List<GameObject> birdList;
	
	
	private float distTo(GameObject b, GameObject b1) {
		
		float xDist = b1.transform.position.x - b.transform.position.x; 
		xDist = xDist * xDist;
		float yDist = b1.transform.position.y - b.transform.position.y;
		yDist = yDist * yDist;
		float zDist = b1.transform.position.z - b.transform.position.z;
		zDist = zDist * zDist;
		float dist = Mathf.Sqrt (xDist + yDist + zDist);
		return dist;
	}
	
	private Vector3 alignmentRule(GameObject b)  {
		Vector3 tempV = new Vector3 (0, 0, 0);
//		Vector3 tempV = b.rigidbody.velocity;
		int neighborsCount = 0;
		//	foreach (GameObject b1 in birdList) {
		foreach (GameObject b1 in birdList){
			if (!b.Equals(b1)) {
				if (distTo (b, b1) < distance) {
					tempV.x += b1.rigidbody.velocity.x;
					tempV.y += b1.rigidbody.velocity.y;
					tempV.z += b1.rigidbody.velocity.z;
					neighborsCount++;
				}
			} 
		}
		if (neighborsCount == 0)
			return tempV;
		tempV.x /= neighborsCount;
		tempV.y /= neighborsCount;
		tempV.z /= neighborsCount;
		tempV.Normalize();
		return tempV*7;
		
	}
	
	private Vector3 cohesionRule(GameObject b)  {
		Vector3 tempV = new Vector3 (0, 0, 0);
		int neighborsCount = 0;
		//	foreach (GameObject b1 in birdList) {
		foreach (GameObject b1 in birdList){
			if (!b.Equals(b1)) {
				if (distTo (b, b1) < distance) {
					if (distTo(b, b1) > 10 ) {
					tempV.x+=b1.transform.position.x; //cohesion
					tempV.y+=b1.transform.position.y;
					tempV.z+=b1.transform.position.z;
					}
					neighborsCount++;
				}
			}
		}
		if (neighborsCount == 0)
			return tempV;
		tempV.x /= neighborsCount;
		tempV.y /= neighborsCount;
		tempV.z /= neighborsCount;
		tempV = new Vector3 (tempV.x -= b.transform.position.x, tempV.y -= b.transform.position.y, tempV.z -= b.transform.position.z);
		tempV.Normalize();
		return tempV*7;
		
	}	
	
	private Vector3 separationRule(GameObject b)  {
		Vector3 tempV = new Vector3 (0, 0, 0);
		int neighborsCount = 0;
		//	foreach (GameObject b1 in birdList) {
		foreach (GameObject b1 in birdList){
			if (!b.Equals(b1)) {
				if (distTo (b, b1) < distance/2) {
					tempV.x+=b1.transform.position.x - b.transform.position.x; //separation
					tempV.y+=b1.transform.position.y - b.transform.position.y;
					tempV.z+=b1.transform.position.z - b.transform.position.z;
					neighborsCount++;
				}
			}
		}
		if (neighborsCount == 0)
			return tempV;
		tempV.x /= neighborsCount;
		tempV.y /= neighborsCount;
		tempV.z /= neighborsCount;
		tempV.x*=-1; 
		tempV.y*=-1;
		tempV.z*=-1;
		tempV.Normalize();
		return tempV*7;
		
		
	}
	
	private Vector3 wallAvoid(GameObject b) {
		Vector3 tempV = new Vector3 (0, 0, 0);
		//	tempV = b.rigidbody.velocity;
		
		if (b.transform.position.x < -boundary) 
		{
			tempV.x = 3.0f;
		}
		if (b.transform.position.x > boundary)
		{

			tempV.x = -3.0f;

		}
		if (b.transform.position.y < -boundary)
		{

			tempV.y = 3.0f;

		}
		if (b.transform.position.y > boundary)
		{

			tempV.y = -3.0f;

		}		if (b.transform.position.z < -boundary)
		{

			tempV.z = 3.0f;

		}		if (b.transform.position.z > boundary)
		{

			tempV.z = -3.0f;
		
		}
	//		b.rigidbody.velocity = tempV;
		
		return tempV;
	}
	
	
	
	
	
	void Start () {
		
		birdList = new List<GameObject> ();

		foreach(GameObject bird in GameObject.FindGameObjectsWithTag("seagull"))
		{
			if(bird.name == "seagull")
			{
				birdList.Add(bird);
				bird.rigidbody.velocity = new Vector3 (Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
			}
		}

//		for (int i = 0; i<numberOfBirds; i++) {
//			bird = GameObject.Find("seagull"+i);
//			birdList.Add(bird);
//			bird.rigidbody.velocity = new Vector3 (Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
//			
//		}
		
		Debug.Log (birdList.Count);

	}
	
	// Update is called once per frame	
	void Update () {
		
		//	foreach (GameObject b in birdList) {
		foreach (GameObject b in birdList) {
			Vector3 alignment = alignmentRule(b);
			Vector3 cohesion = cohesionRule(b);
			Vector3 separation = separationRule(b);
			Vector3 wallavoid = wallAvoid(b); 

			alignmentWeight = gameObject.GetComponent<gui>().AlignmentValue;
			cohesionWeight = gameObject.GetComponent<gui>().CohesionValue;
			separationWeight = gameObject.GetComponent<gui>().SeparationValue;
			distance = gameObject.GetComponent<gui>().DistanceValue;

			
			Vector3 tempV = b.rigidbody.velocity;
			tempV.x += alignment.x* alignmentWeight + cohesion.x* cohesionWeight + separation.x* separationWeight;
			tempV.y += alignment.y* alignmentWeight + cohesion.y* cohesionWeight + separation.y* separationWeight;
			tempV.z += alignment.z* alignmentWeight + cohesion.z* cohesionWeight + separation.z* separationWeight;
			tempV = tempV.normalized*speed;
		

			tempV.x += wallavoid.x;
			tempV.y += wallavoid.y;
			tempV.z += wallavoid.z;
			
			

			
	    	if (Random.Range(0,300)==1)
				tempV = new Vector3 (Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));


			b.transform.rotation = Quaternion.LookRotation(b.rigidbody.velocity);

			b.rigidbody.velocity+= tempV;
			Vector3 velocity = b.rigidbody.velocity;
			float magnitude = velocity.magnitude;

			if (magnitude>70) {
				velocity *= (70.0f / b.rigidbody.velocity.magnitude);
				b.rigidbody.velocity = velocity;

			}


			
		}
		
		
		
		
		
		}
}
