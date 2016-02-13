using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (2)) {
						float h = 100.0f * Input.GetAxis ("Mouse X");
						float v = 100.0f * Input.GetAxis ("Mouse Y");
						transform.Translate (h, v, 0);
				}
	}
}
