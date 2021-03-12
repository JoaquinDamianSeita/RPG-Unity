using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions; //para los errores

public class Warp : MonoBehaviour {

	public GameObject target; //a donde quiero ir
	public GameObject targetMap; //el mapa de destino 

	void Awake(){ //mientras carga el escenario se desactiva el renderer de los sprites para el warp y el hijo exit

		Assert.IsNotNull (target);
		Assert.IsNotNull (targetMap); //si no asignamos esto tira error

		GetComponent <SpriteRenderer>().enabled = false;
		transform.GetChild (0).GetComponent <SpriteRenderer>().enabled = false;
	}

	void OnTriggerEnter2D(Collider2D other){ // enumerador uso un trigger en el collider del box y other es el jugador, si choca contra el cambia la posicion a donde esta el hijo del tp osea la exit
		if (other.tag == "Player"){
			other.transform.position = target.transform.GetChild (0).transform.position; //actualizo la posicion
			Camera.main.GetComponent <CameraFollow> ().SetBound (targetMap); //aca le digo a la camara que cambie de mapa
		}

	}
}


