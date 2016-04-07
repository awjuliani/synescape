using UnityEngine;
using System.Collections;
using System.IO;  

public class visualize : MonoBehaviour {
	float myTime;
	// Use this for initialization
	void Start () {
		myTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		myTime += Time.deltaTime;

		if (myTime > 1) {
			Destroy (this.gameObject);
		}
	}
}
