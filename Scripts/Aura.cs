using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour {

	//tiempo de precarga
	public float waitBeforePlay;

	Animator anim;
	Coroutine manager;
	bool loaded;


	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator> ();	
	}

	//comprobamos si cargamos lo suficiente
	public IEnumerator Manager(){
		yield return new WaitForSeconds (waitBeforePlay);
		anim.Play ("Aura_Play");
		loaded = true;
	}

	public void AuraStart(){
		manager = StartCoroutine (Manager ());
		anim.Play ("Aura_Play");
	}

	public void AuraStop(){
		StopCoroutine (manager);
		anim.Play ("Aura_Idle");
		loaded = false;
	}



	public bool IsLoaded(){
		return loaded;
	}
}
