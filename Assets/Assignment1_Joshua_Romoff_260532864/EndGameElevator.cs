using UnityEngine;
using System.Collections;

public class EndGameElevator : MonoBehaviour {
	bool beginAscent = false;
	public GameObject exit;
	float speed = 1.0f;
	float startTime;
	float lengthLeft;
	float distanceCovered = 0.0f;
	float fractionCompleted = 0.0f;
	Vector3 start;
	Vector3 offsety = new Vector3 (0, 1.5f, 0);
	bool elevator = true;

	void Start () {

	}
	

	void Update () {
		//if our player has reached us begin elevating, stop when weve reached the target which is the terrain
		if (beginAscent && fractionCompleted < .99f) {
			//keep track of our distance covered and the fraction completed
			distanceCovered=(Time.time - startTime)*speed;
			fractionCompleted = distanceCovered/lengthLeft;
			//lerp to the new postion
			this.transform.position = Vector3.Lerp (start,exit.transform.position+offsety,fractionCompleted);
				}
		//if were done then render the cover of the exit and stop ascent
		if (fractionCompleted > .99f) { 
			exit.renderer.enabled = true;
			beginAscent = false;
		}
			
	}

	void OnCollisionEnter(Collision collision){
		//if we have reached the elevator floor
		if (elevator && collision.gameObject.name == "exitFloor") {
					//dont render the exit cover and begin our ascent
					exit.renderer.enabled = false;
					beginAscent = true;
					startTime = Time.time;
					lengthLeft = Vector3.Distance (this.transform.position, exit.transform.position+offsety);
					elevator = false;
					start = this.transform.position;
		}
		}
	public bool getAscent(){
		return beginAscent;
		}


		

}
