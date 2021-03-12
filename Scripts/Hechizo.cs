using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hechizo : MonoBehaviour {

	[Tooltip ("Espera unos segundos antes de destruir el objeto")]
	public float waitBeforeDestroy;

	[HideInInspector] //lo oculta en el inspector
	public Vector2 mov;

	public float speed;

	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (mov.x, mov.y, 0) * speed * Time.deltaTime; //mueve el hechizo de forma automatica
	}
	IEnumerator OnTriggerEnter2D (Collider2D col){
		if (col.tag == "Enemy") {
			yield return new WaitForSeconds (waitBeforeDestroy);
			col.SendMessage("Atacado",30);
			Destroy (gameObject);
		}else if (col.tag != "Player" && col.tag != "Ataque") {
			Destroy (gameObject);
		}
	}
}
