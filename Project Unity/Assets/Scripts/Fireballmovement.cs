﻿using UnityEngine;
using System.Collections;

public class Fireballmovement : MonoBehaviour
{
		public Rigidbody RB;
		public Rigidbody fumee;
		//SphereCollider sc;
		public float Force;
		public float rayon;
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
			
		}

		void OnTriggerEnter (Collider collider)
		{
				if (b) {
						Debug.Log ("tag: " + collider.tag);
						if (collider.tag == "Character") {
						}

						RB.AddRelativeForce (Vector3.forward * Force * 0.2f);
						Destroy (gameObject, 0.3f);
						UnityEngine.Object smoke = Instantiate (fumee, collider.transform.position + Vector3.up, Quaternion.identity);
						Destroy (smoke, 4f);
						foreach (GameObject go in ennemies) {
								if (go != null) {
										float my_x = go.transform.position.x - RB.transform.position.x;
										float my_y = go.transform.position.y - RB.transform.position.y;
										float my_z = go.transform.position.z - RB.transform.position.z;
										Debug.Log ("tag rayon: " + go.tag + " " + (go.name == "Perso(Clone)") + go.name);
										if (Mathf.Abs (my_x) <= rayon && Mathf.Abs (my_y) <= rayon && Mathf.Abs (my_z) <= rayon) {
												Debug.Log ("oui");
												if (go.name == "Perso(Clone)") {
														Debug.Log ("go");
														GameObject objet = GameObject.FindGameObjectWithTag ("Joueur");
														objet.SendMessage ("degats", 15, SendMessageOptions.DontRequireReceiver);
												} else {
														Collider objet = go.GetComponent<Collider> ();
														objet.SendMessage ("degats", 10, SendMessageOptions.DontRequireReceiver);
												}
												
												
										}
								}
						}
						b = false;
				}

		}
}
