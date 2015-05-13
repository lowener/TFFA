﻿using UnityEngine;
using System.Collections;

public class Fireballmouvementmulti : MonoBehaviour {

	public Rigidbody RB;
	public Rigidbody fumee;
	//SphereCollider sc;
	public float Force;
	public float rayon = 3f;
	GameObject[] ennemies;
	bool b;
	// Use this for initialization
	void Start ()
	{
		//sc = GetComponent<SphereCollider> ();
		RB.AddRelativeForce (Vector3.forward * -Force);
		ennemies = GameObject.FindGameObjectsWithTag ("Character");
		b = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		ennemies = GameObject.FindGameObjectsWithTag ("Character");
	}
	
	void OnTriggerEnter (Collider collider)
	{
		if (b) {
			Debug.Log ("tag: " + collider.tag);
			if (collider.tag == "Character") {
			}
			RB.AddRelativeForce (Vector3.forward * Force * 0.2f);
			UnityEngine.GameObject smoke = Network.Instantiate (fumee, collider.transform.position + Vector3.up, Quaternion.identity,0) as GameObject;
			foreach (GameObject go in ennemies) {
				if (go != null) {
					float my_x = go.transform.position.x - RB.transform.position.x;
					float my_y = go.transform.position.y - RB.transform.position.y;
					float my_z = go.transform.position.z - RB.transform.position.z;
					if ( Mathf.Abs(my_x) <= rayon && Mathf.Abs(my_y) <= rayon && Mathf.Abs(my_z) <= rayon)
					{
						Debug.Log ("tag rayon: " + go.tag + " " + (go.name == "Perso(Clone)") + go.name);
						Debug.Log ("oui");
						GameObject objet = GameObject.FindGameObjectWithTag("Joueur");
						objet.SendMessage("degats", 15, SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			b = false;
			Wait(0.3f);
			Network.Destroy (gameObject);
			Wait(4f);
			Network.Destroy(smoke);
		}
		
	}
	private IEnumerator Wait(float f)
	{
		yield return new WaitForSeconds(f);
	}

}
