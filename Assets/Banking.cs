using UnityEngine;
using System.Collections;

public class Banking : MonoBehaviour {

	public float stiffness; // Hit distance smaller than rest length
	public float damping;   // Hit distance greater than rest length
	public float restLength;   // Hit distance greater than rest length
	
	public float bankingStiffness; // Hit distance smaller than rest length
	public float bankingDamping;   // Hit distance greater than rest length

	public Transform leftStab;
	public Transform rightStab;
	private Rigidbody rb;
	
	private float prev=0;
	private float prevL=0;
	private float prevR=0;
	
	private float susp;
	private float suspL;
	private float suspR;

	private float weightL;
	private float weightR;
	
	public float speed;
	public float jumpSpeed;

	private Vector3 origPos;
	private Quaternion origRot;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		origPos = transform.position;
		origRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y < 50) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			prev=0;
			prevL=0;
			prevR=0;
			transform.rotation = origRot;
			transform.position = origPos;
		}

		float horizontal = Input.GetAxis ("Horizontal");
		weightR = 1f;
		if (0 < horizontal) {
			weightR = 0.3f;
			rb.AddForce(transform.right*0.4f /Time.deltaTime);
		}
		weightL = 1f;
		if (0 > horizontal) {
			weightL = 0.3f;	
			rb.AddForce(-transform.right*0.4f /Time.deltaTime);
		}

		// Left balancer
		RaycastHit hitL;
		Physics.Raycast (leftStab.position, -leftStab.up, out hitL, 2*restLength);
		if( 0<hitL.distance ) {
			suspL = suspension(weightL*restLength, hitL.distance, prevL, bankingStiffness, bankingDamping, Time.deltaTime);
			rb.AddForceAtPosition(transform.up * suspL, leftStab.position);
			prevL = hitL.distance;
		}

		// Right balancer
		RaycastHit hitR;
		Physics.Raycast (rightStab.position, -rightStab.up, out hitR, 2*restLength);
		if( 0<hitR.distance ) {
			suspR = suspension(weightR*restLength, hitR.distance, prevR, bankingStiffness, bankingDamping, Time.deltaTime);
			rb.AddForceAtPosition(transform.up * suspR, rightStab.position);
			prevR = hitR.distance;
		}

		// Main Thrust
		RaycastHit hit;
		Physics.Raycast (transform.position, -transform.up, out hit, 20*restLength);
		if( 0<hit.distance && hit.distance <= 20*restLength) {
			susp = suspension(restLength, hit.distance, prev, stiffness, damping, Time.deltaTime);
			rb.AddForce(transform.up * susp);
			prev = hit.distance;
		}

		// Jump
		if (Input.GetKey("space")) {
			rb.AddForce (transform.forward * speed * Time.deltaTime);
		}
	}

	float suspension(float desiredLength, float currentLength, float previousLength, float stiff, float damp, float deltaTime){
		return stiff * (desiredLength - currentLength) + damp * (previousLength - currentLength) / deltaTime;
	}
}
