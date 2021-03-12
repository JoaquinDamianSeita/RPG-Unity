using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col){
		//le pasamos el daño que queremos hacer con este ataque
		if (col.tag == "Enemy"){
			col.SendMessage("Atacado",15);
		}
	}


}
