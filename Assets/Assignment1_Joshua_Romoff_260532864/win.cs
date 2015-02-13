using UnityEngine;
using System.Collections;

public class win : MonoBehaviour {
	bool winner = false;

	void OnTriggerEnter(Collider collision){
		//if our player collides with the final floor print the win message
		if (collision.gameObject.name == "Player")
				winner = true;
		}
	void OnGUI(){
				if (winner) {
						GUI.Box (new Rect (0, 0, Screen.width / 2, Screen.height / 2), "YOU WIN!, now follow the path out back to the elevator to take you back to the terrain.");
			
				}
		}
}
