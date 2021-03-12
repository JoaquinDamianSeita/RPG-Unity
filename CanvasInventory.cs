using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInventory : MonoBehaviour {

	public GameObject inventario;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)){ //alternador entre activado y desactivado
			inventario.SetActive(!inventario.activeInHierarchy);
		}
	}
}
