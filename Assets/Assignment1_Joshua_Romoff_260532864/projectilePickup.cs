using UnityEngine;
using System.Collections;

public class projectilePickup : MonoBehaviour {
	bool shooting = false;
	bool GameOver = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision){
		//if we are shooting
		if (shooting) {
			//figure out which door we shouuld hit. this.gameobject.name is the appropriate color				
			string door = "TriggerWallSouth" + this.gameObject.name;
			//if we hit the appropriate object
			if (collision.gameObject.name == door)
					//destroy the object
					Destroy (collision.gameObject);
			else {
					//otherwise its game over
					GameOver = true;

			}
			//we make the object a trigger so it will not collide with us anymore
			this.gameObject.collider.isTrigger = true;
			//and we dont render it
			this.renderer.enabled = false;

		}
				
	}
	//functions used by the player controller and pick scripts to figure out what is happening
	public void isShooting(){
		shooting = true;
		}

	public bool isGameOver(){
		return GameOver;
		}


}
