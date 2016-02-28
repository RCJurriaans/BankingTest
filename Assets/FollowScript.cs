using UnityEngine;
using System.Collections;

public class FollowScript : MonoBehaviour {

	public Transform target;
	public float distance;
	public float verticalOffset;
	public float damping;
	public float stiffness;
	public float rotationalDamping;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 wantedPosition = target.TransformPoint(0, verticalOffset, -distance);
		transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);
	}
}
