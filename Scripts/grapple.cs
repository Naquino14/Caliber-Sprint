using System.Collections;
using System.Collections.Generic;
using UnityEditor;

// WARNING!!!! UNTESTED!!! MADE ON IOS!!!

public class yeah : monobehavior
{
	public Rigidbody rb;
	private bool isEquip;
	private int amtJump;
	private bool isGrounded;
	private bool isGrapple;
	private bool isGrappleReady;
	public float grappleSpeed;
	
	public float maxGrappleDistance;
	private Vector3 direction;
	private Vector3 origin;
	public layerMask layerMask;
	
	
	void Start(){
		rb.GetComponent<Rigidbody>();
	}
	
	void Update(){
		isGrounded = MovementController.isGrounded;
		amtJump = MovementController.amtJump;
		origin = controller.transform.position;
		direction = controller.transform.transformDirection(Vector3.forward);
		
		if(input.getKeyDown(keyCode.G) && isGrounded && isEquip && isGrappleReady){
			RaycastHit hit;
			if(Physics.Raycast(origin, direction, out hit, maxGrappleDistance, layerMask)){
				controller.enabled(false);
				rb.enabled(true);
				isGrapple = true;
				Vector3 anchor = hit.transform.transformDirection(Vector3.forward);
				rb.addForce(anchor * grappleSpeed, ForceMode.Force);
				
			}
		}
	}
	
	
}
