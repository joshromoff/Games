using UnityEngine;
using System.Collections;

public class elevator : MonoBehaviour {
	
	//public GameObject Elevator;
	public GameObject LandingPoint;
	public GameObject player;
	bool trigger = false;
	// Update is called once per frame
	void OnMouseOver(){

		trigger = true;
		}
	void OnMouseExit(){
		
		trigger = false;
	}
	void OnMouseDown(){
		this.animation.Play ();

		}

	void OnGui(){
				if (trigger)
						GUI.Box (new Rect (300, 300, 200, 20), "click to go to the dungeon");
		}
}
