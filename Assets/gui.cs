using UnityEngine;
using System.Collections;

public class gui : MonoBehaviour {
	
	private float alignmentValue = 1.0f;
	private float cohesionValue = 1.0f;
	private float separationValue = 1.0f;
	private float distanceValue = 70.0f;

	public float AlignmentValue
	{
		get { return this.alignmentValue; }

	}
	public float CohesionValue
	{
		get { return this.cohesionValue; }
		
	}
	public float SeparationValue
	{
		get { return this.separationValue; }
		
	}
	public float DistanceValue
	{
		get { return this.distanceValue; }
		
	}
	
	void OnGUI () {
		GUI.Label(new Rect(Screen.width- 120, 25, 150, 30), "Alignment Weight");
		GUI.Label(new Rect(Screen.width- 120, 65, 150, 30), "Cohesion Weight");
		GUI.Label(new Rect(Screen.width- 120, 105, 150, 30), "Separation Weight");
		GUI.Label(new Rect(Screen.width- 140, 145, 150, 30), "Distance to Neigbours");

		alignmentValue = GUI.HorizontalSlider (new Rect (Screen.width- 120, 45, 100, 30), alignmentValue, 0.1f, 2.0f);
		cohesionValue = GUI.HorizontalSlider (new Rect (Screen.width- 120, 85, 100, 30), cohesionValue, 0.1f, 2.0f);
		separationValue = GUI.HorizontalSlider (new Rect (Screen.width- 120, 125, 100, 30), separationValue, 0.1f, 2.0f);
		distanceValue = GUI.HorizontalSlider (new Rect (Screen.width- 120, 165, 100, 30), distanceValue, 10.0f, 150.0f);

	}
	}


