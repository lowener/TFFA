﻿using UnityEngine;
using System.Collections;

public class DeplacementsMulti : MonoBehaviour {
	//Variables publiques
	public float gravity;
	public float speed;
	public float speedRun;
	public float speedJump;
	public float sensiverticale = (float)150.0;
	public CharacterController controller;
	public GameObject player;
	
	
	//Variables privées

	private Vector3 moveDirection;
	private float deltaTime;
	private Transform characterContent;
	
	private Vector3 to;
	private Vector3 mouseDirection;
	private float x = (float)0.0;
	private float y = (float)0.0;
	private Vector3 angle;
	public float lastSynchronizationTime = 0f;
	public float syncDelay = 0f;
	public float syncTime = 0f;
	public Vector3 syncStartPosition = Vector3.zero;
	public Vector3 syncEndPosition = Vector3.zero;
	
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		moveDirection = new Vector3(0f, 0f, 0f);
		characterContent = transform.Find("Perso Principal FInal 1");
		
		//On initialise l'angle
		angle = transform.eulerAngles;
		x = angle.y;
		y = angle.x;
		
	}
	
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = player.GetComponent<Rigidbody>().position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			player.GetComponent<Rigidbody>().position = syncStartPosition;
			syncEndPosition = syncPosition;
		}
	}
	private void SyncedMovement(){
		syncTime += Time.deltaTime;
		player.GetComponent<Rigidbody> ().position = Vector3.Lerp (syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}
	
	// Update is called once per frame
	void Update () {

						deplacement ();
		if (!GetComponent<NetworkView> ().isMine)
						SyncedMovement ();
	}
	
	void deplacement()
	{
		if (GetComponent<NetworkView> ().isMine)
		if (characterContent != null) 
		{

			deltaTime = Time.deltaTime; //Ca evite de prendre de la mémoire pour rien.
			//Le personnage va tourner sur lui-meme en fonction du mouvement de la souris.
			x += Input.GetAxis ("Mouse X") * sensiverticale * (float)0.02;
			Quaternion rotation = Quaternion.Euler (y, x, 0);
			transform.rotation = rotation;
			if (controller.isGrounded) {
				//On prend les différentes valeurs des axes horizontaux et verticaux et on les met dans le moveDirection (vecteur du déplacement du perso)
				moveDirection.Set (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
				moveDirection *= speed;
				moveDirection = transform.TransformDirection (moveDirection); //transforme les axes globaux en axes locaux et vice versa.
				if (Input.GetKey ("left shift")) { //Si le perso court
					moveDirection *= speedRun;
					characterContent.GetComponent<Animation> ().CrossFade ("Anim - Courrir", 0.2f);
					WaitForAnimation ();
				}
				if (Input.GetButton ("Jump")) { //Si le perso saute. On le met avant le courrir car si on saute, on ne peut ni marcher, ni courrir.
					characterContent.GetComponent<Animation> ().CrossFade ("Animation - Saut", 0.2f);
					WaitForAnimation ();
					moveDirection.y += speedJump;
				} 
				else if (Input.GetButton ("Horizontal") || Input.GetButton ("Vertical")) {
					characterContent.GetComponent<Animation> ().CrossFade ("Anim - Marche"); //Si il ne court ni ne saute et qu'il bouge, alors il marche
				}
				if (!Input.anyKey)
					characterContent.GetComponent<Animation> ().CrossFade ("Anim - Idle", 0.2f);
			}
			//Gravity et on bouge le character controller
			moveDirection.y -= gravity * deltaTime; 
			controller.Move (moveDirection * deltaTime);
		}
	}

	//Fonction qui sert à attendre une seconde, soit le temps d'appliquer l'animation. On pourra la changer plus tard avec en paramètres le nombre de secondes
	//si on veut réaliser une animation plus longue.
	private IEnumerator WaitForAnimation()
	{
		yield return new WaitForSeconds(1);
	}
}
