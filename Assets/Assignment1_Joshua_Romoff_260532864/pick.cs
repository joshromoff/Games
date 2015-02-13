using UnityEngine;
using System.Collections;

public class pick : MonoBehaviour {

		GameObject mainCamera;
		GameObject player;
		public GameObject projectilePoint;
		bool carrying = false;
		GameObject carriedObject;
		public float distance;
		public float lerpOffset;
		projectilePickup pp;
		bool GameOver=false;
		// Use this for initialization
		void Start () {
			//set our camera to be accessible
			mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");

			//set our player
			player = GameObject.FindGameObjectWithTag("Player");
		}
		
		void FixedUpdate(){
			//if we are carring our object and we left click, shoot the projectile. and run the projectile collision script pp.isShooting.
			if (carrying) {
					if (Input.GetKeyDown (KeyCode.Mouse0)) {
							
						pp.isShooting();
						carrying = false;
						carriedObject.rigidbody.isKinematic = false;
						carriedObject.rigidbody.useGravity = true;
						carriedObject.rigidbody.constraints = RigidbodyConstraints.None;
						carriedObject.rigidbody.velocity = (mainCamera.transform.forward * 20);
						carriedObject.transform.SetParent (null);
						
						
						
					}
		
			}
			//if we didnt hit anything, our pp.isgameover will tell us. and we print the message to the player
			if(pp!=null){
				if(pp.isGameOver()) 
					GameOver =true;
			}

	}
	// Update is called once per frame
		void Update () {
			//if were carrying and we click comma, drop the object
			if (carrying) {
				if(Input.GetKeyDown (KeyCode.Comma)){
					carrying = false;
					carriedObject.rigidbody.isKinematic = false;
					carriedObject.rigidbody.useGravity = true;
					carriedObject.transform.SetParent (null);
					carriedObject.rigidbody.constraints = RigidbodyConstraints.None;
					
					
			}
			} else {
				//otherwise we need to run our pickup function, to see if we can pickup
				pickup();
			}
			
		}
		void pickup(){
			//only pickup if we have period clicked
			if(Input.GetKeyDown (KeyCode.Period)){
				int width = Screen.width/2;
				int height = Screen.height/2;
				//shoot a ray from the midpoint 
				Ray ray = mainCamera.camera.ScreenPointToRay(new Vector3 (width,height));
				RaycastHit hit;
				//if we hit something
				if(Physics.Raycast (ray,out hit)){
					pp = hit.collider.GetComponent<projectilePickup>();
					//if its pickupable then pick it up
					if(pp!=null){
						carrying = true;
						carriedObject = pp.gameObject;
						carriedObject.rigidbody.isKinematic = true;
						carriedObject.rigidbody.useGravity = false;
						//transform the podition of the object to the camera, distance controls how far away the object is
						carriedObject.transform.position = Vector3.Lerp(carriedObject.transform.position,projectilePoint.transform.position /*+ mainCamera.transform.forward * distance*/, Time.deltaTime *lerpOffset);
					    carriedObject.transform.SetParent (mainCamera.transform);
				}
				}
				
			}
		}
	void OnGUI(){
		//print a box with a message saying the game is oevr if the user shot and missed the appropriate target
		if (GameOver) {
			GUI.Box(new Rect(0,0,Screen.width/2,Screen.height/2),"GameOver: You need to shoot the projectile at the same colour wall, you can continue exploring but you cannot win");

		}
	}

	}

