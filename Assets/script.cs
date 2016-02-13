using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script : MonoBehaviour {
	
	// Use this for initialization
	GameObject bird;
	GameObject spBird;
	int numberOfBirds = 30;
	float distance = 100.0f;	
	float boundary = 70.0f;
	int speed = 10;

	float alignmentWeight = 1.0f;
	float cohesionWeight = 1.0f;
	float separationWeight = 1.0f;
	Animator anm;
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

	private float flAngle(GameObject b) {
		return 57.3f*Mathf.Atan (Mathf.Sqrt(b.GetComponent<Rigidbody>().velocity.x*b.GetComponent<Rigidbody>().velocity.x + b.GetComponent<Rigidbody>().velocity.z*b.GetComponent<Rigidbody>().velocity.z) /Mathf.Abs(b.GetComponent<Rigidbody>().velocity.y));
		}
	
	private Vector3 alignmentRule(GameObject b)  {
		Vector3 tempV = new Vector3 (0, 0, 0);
		int neighborsCount = 0;
		foreach (GameObject b1 in birdList){
			if (!b.Equals(b1)) {
				if (distTo (b, b1) < distance) {
					tempV.x += b1.GetComponent<Rigidbody>().velocity.x;
					tempV.y += b1.GetComponent<Rigidbody>().velocity.y;
					tempV.z += b1.GetComponent<Rigidbody>().velocity.z;
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
		tempV = new Vector3 (tempV.x -= b.transform.position.x, tempV.y -= b.transform.position.y, 
		                     tempV.z -= b.transform.position.z);
		tempV.Normalize();
		return tempV*7;
		
	}
	
	private Vector3 separationRule(GameObject b)  {
		Vector3 tempV = new Vector3 (0, 0, 0);
		int neighborsCount = 0;
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
		
		if (b.transform.position.x < -boundary) 
		{
			tempV.x = 1.0f;
		}
		if (b.transform.position.x > boundary)
		{
			tempV.x = -1.0f;
		}
		if (b.transform.position.y < -boundary)
		{
			tempV.y = 2.0f;
		}
		if (b.transform.position.y > boundary)
		{
			tempV.y = -2.0f;
		}		
		if (b.transform.position.z < -boundary)
		{
			tempV.z = 3.0f;
		}		if (b.transform.position.z > 0f)
		{
			tempV.z = -1.0f;
		}
		
		return tempV;
	}

	private Vector3 obstacleAvoid (GameObject b, GameObject o) {
		Vector3 ahead = b.transform.position + b.GetComponent<Rigidbody>().velocity.normalized;
		Vector3 av = ahead - o.transform.position;
		return av;


	}
	
	
	
	
	void Start () {
		
		birdList = new List<GameObject> ();
		foreach(GameObject bird in GameObject.FindGameObjectsWithTag("seagull"))
		{
			if(bird.name == "seagull")
			{
				birdList.Add(bird);
				bird.GetComponent<Rigidbody>().velocity = new Vector3 (Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));

			}
		}


		
		Debug.Log (birdList.Count);

	}
	
	// Update is called once per frame	
	void Update () {


		foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("obstacle")) {
			if (gameObject.GetComponent<gui>().EnableObstacles) {
				Vector3 temppos = obstacle.transform.position;
				temppos.z = 150;
				obstacle.transform.position = temppos;
			}

			else {
				Vector3 temppos = obstacle.transform.position;
				temppos.z = 10000;
				obstacle.transform.position = temppos;
			}

		}


		foreach (GameObject b in birdList) {
		
		if (b.GetComponent<Rigidbody>().velocity.y>0)
			b.GetComponent<Animation>().Play();

			Vector3 alignment = alignmentRule(b);
			Vector3 cohesion = cohesionRule(b);
			Vector3 separation = separationRule(b);
			Vector3 wallavoid = wallAvoid(b); 


			Vector3 obstacleavoid = new Vector3 (0, 0, 0);
			Debug.DrawRay (b.transform.position, b.GetComponent<Rigidbody>().velocity, Color.red);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(b.transform.position, b.GetComponent<Rigidbody>().velocity, out hit, 100)) {
				obstacleavoid = obstacleAvoid(b, hit.collider.gameObject).normalized*500;
			
				Debug.Log("cube");
			}

			alignmentWeight = gameObject.GetComponent<gui>().AlignmentValue;
			cohesionWeight = gameObject.GetComponent<gui>().CohesionValue;
			separationWeight = gameObject.GetComponent<gui>().SeparationValue;
			distance = gameObject.GetComponent<gui>().DistanceValue;


			Vector3 tempV = b.GetComponent<Rigidbody>().velocity;
			tempV += alignment* alignmentWeight + cohesion* cohesionWeight + separation* separationWeight + obstacleavoid;
			tempV = tempV.normalized*speed;
		

			tempV += wallavoid;
		
			

			if (flAngle (b)<20) {

				if (b.GetComponent<Rigidbody>().velocity.y>0) {
				tempV.y-=10.0f;
				
					Debug.Log("up");
				}
				else {
					tempV.y+=10.0f;

					Debug.Log("down");
				}

			}

//	

 	
			b.transform.rotation = Quaternion.LookRotation(b.GetComponent<Rigidbody>().velocity);
			b.GetComponent<Rigidbody>().velocity+= tempV;
			Vector3 velocity = b.GetComponent<Rigidbody>().velocity;
			float magnitude = velocity.magnitude;

			if (magnitude>70) {
				velocity *= (70.0f / b.GetComponent<Rigidbody>().velocity.magnitude);
				b.GetComponent<Rigidbody>().velocity = velocity;

			}


			
		}
		
		
		
		
		
		}
}
