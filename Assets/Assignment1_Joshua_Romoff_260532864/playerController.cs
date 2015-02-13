using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {
	public float movementSpeed= 5.0f;
	public float mouseSense = 2.5f;
	
	public float verticalRotation = 0;
	public float upDownRange = 60.0f;
	
	float verticalVelocity = 0;
	CharacterController characterController;
	EndGameElevator elev; 
	public Texture2D crosshairTexture;
	public float crosshairScale = 1;
	// Use this for initialization
	void Start () {
		Screen.lockCursor= true;
		characterController = GetComponent<CharacterController> ();
		elev = GetComponent<EndGameElevator>();
	}
	
	// Update is called once per frame
	void Update () {
		float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSense;
		transform.Rotate (0,rotLeftRight,0);
		
		verticalRotation -= Input.GetAxis ("Mouse Y") * mouseSense;
		verticalRotation = Mathf.Clamp (verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation,0,0);

		float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;
		if (!elev.getAscent ()) {
						verticalVelocity += Physics.gravity.y * Time.deltaTime; 
				} else
						verticalVelocity = 0;
		//jumping
		//if (characterController.isGrounded && Input.GetButtonDown ("Jump")) {
		//	verticalVelocity = jumpSpeed;		
		//}

		Vector3 speed = new Vector3 (sideSpeed, verticalVelocity, forwardSpeed);
		
		speed = transform.rotation * speed;
		
		
		
		characterController.Move (speed * Time.deltaTime);
	}

	void OnGUI(){
		if(crosshairTexture!=null)
			GUI.DrawTexture(new Rect((Screen.width-crosshairTexture.width*crosshairScale)/2 ,(Screen.height-crosshairTexture.height*crosshairScale)/2, crosshairTexture.width*crosshairScale, crosshairTexture.height*crosshairScale),crosshairTexture);
		else
			Debug.Log("No crosshair texture set in the Inspector");
		
	}
}
