using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	Transform follow; //esto es el objeto que sigue la camara


	public float smoothTime = 1f;
	float tLX,tLY,bRX,bRY; //t=top b=bottom L=Left R=Right X=eje Y=eje

	Vector2 velocity;

	void Awake(){
		follow = GameObject.FindGameObjectWithTag ("Player").transform; 
	}


	void Update(){

		float posX = Mathf.Round (
			Mathf.SmoothDamp (
				transform.position.x,follow.transform.position.x,ref velocity.x,smoothTime
			) * 100) / 100; //redondeado
		float posY = Mathf.Round (
			Mathf.SmoothDamp (
				transform.position.y,follow.transform.position.y,ref velocity.y,smoothTime
			) * 100) / 100; //redondeado

		transform.position = new Vector3 (
			Mathf.Clamp (posX, tLX, bRX),
			Mathf.Clamp (posY, bRY, tLY),
			transform.position.z
		);
	}
	public void SetBound(GameObject map){ //map va a ser el mapa que rescatamos desde el fichero tiled to unity
		Tiled2Unity.TiledMap config = map.GetComponent <Tiled2Unity.TiledMap> (); //el nombre esta aparte de la clase por eso lo llamo con TiledMap despues cargo la config, de ese mapa rescato el componente script
		float cameraSize = Camera.main.orthographicSize; //almacena el tamaño de la camara

		if (map.tag == "Interior") {
			cameraSize = cameraSize - 5;
		} else {
			cameraSize = 10;
		}
		tLX = map.transform.position.x + cameraSize; //aca limitamos con la ubicacion de los extremos del mapa y el tamaño de la camara
		tLY = map.transform.position.y - cameraSize;
		bRX = map.transform.position.x + config.NumTilesWide - cameraSize; //el config nos da el tamaño de tiles para saber donde esta el extremo inferior del mapa
		bRY = map.transform.position.y - config.NumTilesHigh + cameraSize;

		FastMove ();
	}

	public void FastMove(){
		transform.position = new Vector3 (follow.position.x, follow.position.y, transform.position.z);
	}
}
