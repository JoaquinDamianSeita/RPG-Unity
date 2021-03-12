using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
//variables que gestionan el radio de vision ataque y la vel del enemigo
	public float visionRadio;
	public float ataqueRadio;
	public float speed;

	//var para guardar al jugador
	GameObject player;
	//variable para guardar la pos inicial del enemigo
	Vector3 initialPos;

	//animador y cuerpo cinematico von la rotacion en z congelada
	Animator anim;
	Rigidbody2D rb2d;

	//vida enemigo
	public int maxHp = 50;
	public int hp; 

	// Use this for initialization
	void Start () {
		//inicia full vida
		hp = maxHp;

		//Encontramos al jugador por el tag
		player = GameObject.FindGameObjectWithTag("Player");
		//guardamos la posinicial del enemigo
		initialPos = transform.position;

		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		//por defecto el target sera siempre la pos inicial
		Vector3 target = initialPos;

		//comprobamos un raycast de enemigo hasta el jugador
	
	
		RaycastHit2D hit = Physics2D.Raycast(
			transform.position,
			player.transform.position-transform.position,
			visionRadio,
			1 <<LayerMask.NameToLayer("Default")
				//poner el poner el enemigo y el ataque en capas distintas la raycast
				//si no los detectara como entorno y genera bugs
		);

		//debugear el raycast
		Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
		Debug.DrawRay(transform.position,forward,Color.blue);
		

		//si esta el jugador en el radio de vision cagaste papu (es target)
	 	if (hit.collider != null) {
  			 if (hit.collider.tag == "Player") {
    			target = player.transform.position;
			   }
		 }

		//calcular la distancia y direccion hasta el target la dir es hacia donde debe mirar para moverse
		float distancia = Vector3.Distance(target,transform.position);
		Vector3 dir = (target - transform.position).normalized; //devuelve un vector en 1
		
		//si el enemigo esta en rango de ataque entonces se para y ataca
		if (target != initialPos && distancia < ataqueRadio){
			//ahora atacaria
			anim.SetFloat("movX",dir.x);
			anim.SetFloat("movY",dir.y);
			anim.Play("Enemy_Walking",0,0); //congela la animacion de andar

		}else {
			//si no nos movemos hasta el player
			rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);

			//al movernos seteamos la animacion de caminar
			anim.speed = 1; //importante
			anim.SetFloat("movX",dir.x);
			anim.SetFloat("movY",dir.y);
			anim.SetBool("Walking",true);
		}

		//una ultima cosa para evitar los bugs
		if (target == initialPos && distancia<0.02f){
			transform.position = initialPos;
			//seteamos la anim a idle
			anim.SetBool("Walking",false);
		}

		//debug de lina desd el enemigo al target
		Debug.DrawLine(transform.position,target,Color.green);
	}
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position,visionRadio);
		Gizmos.DrawWireSphere(transform.position,ataqueRadio);
	}
	public void Atacado(int danio){ //cuando le saco vida llamo a este metodo
		hp = hp-danio;
		if (hp <=0) Destroy(gameObject);
		print(hp);
	}
	void OnTriggerEnter2D (Collider2D col){
		//le pasamos el daño que queremos hacer con este ataque
		if (col.tag == "Player"){
			col.SendMessage("Atacado",10);
		}
	}
	void OnGUI(){
		//guardamos la posiicon del enemigo respecto de la camara
		Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

		//dibujamos el cuadrado debajo del enemigo con el texto
		GUI.Box(
			new Rect(
				pos.x-25,					//pos x de la barra
				Screen.height - pos.y +20,	//pos y de la barra
				50,							//anchura de la barra
				20							//altura de la barra
			),hp + "/" + maxHp				//texto de la barra
		);
	}
}

