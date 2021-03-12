using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : MonoBehaviour {

	public float speed = 4f;
	
	public int hp;
	public int hpmax;
	public int mana;
	public int manamax;
	public int exp;
	public int oro;
	public int nivel;


	Animator anim;
	Rigidbody2D rb2d;
	Vector2 mov; //vectror 2 para movimiento del rigidbody
	Aura aura;


	Vector3 areaAtaqueSizeX = new Vector3 (2.4f, 0.4f, 0f);
	Vector3 areaAtaqueSizeY = new Vector3 (0.4f, 2.4f, 0f);

	public GameObject initialMap; //que mapa es el primero que aparezco
	public GameObject hechizoPrefab; 

	BoxCollider2D areaAtaque;

	GameObject usuarioBarras;

	bool movePrevent;

	void Awake(){
		Assert.IsNotNull (initialMap);
		Assert.IsNotNull (hechizoPrefab);
	}

	// Use this for initialization
	void Start () {

		hp = hpmax;
		anim = GetComponent <Animator> ();
		rb2d = GetComponent <Rigidbody2D> ();

		areaAtaque = transform.GetChild (0).GetComponent <BoxCollider2D> (); //obtengo el box collider de ataque
		areaAtaque.enabled = false; //desactivo el box desde el comienzo luego activo cuando ataco

		Camera.main.GetComponent <CameraFollow> ().SetBound (initialMap); //aca le indico a la camara sobre que mapa situarse

		aura = transform.GetChild (1).GetComponent <Aura> (); //rescato el script del aura

		usuarioBarras = GameObject.FindGameObjectWithTag("usuariobarras");
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Movimiento ();

		AnimacionesMov ();

		AtaqueCuerpo ();

		AtaqueHechizo ();

		PreventMovement ();

		ActualizaLasBarras();

	}
	void FixedUpdate(){ //maneja las fisicas del player
		rb2d.MovePosition (rb2d.position + mov * speed * Time.deltaTime); //un vector maneja el movimiento ahora mov es el vector que indica dirrecion por la velocidad con deltatime
	}
	void Movimiento(){
		mov = new Vector2 (Input.GetAxisRaw ("Horizontal"),Input.GetAxisRaw ("Vertical")); //rescato los axis de las flechas y las almaceno ene l vector 2
	}
	void AnimacionesMov(){
		if (mov != Vector2.zero) { //si el mov es disinto de nulo actualizo la posicion si no queda en la ultima direccion que miro el player
			anim.SetFloat ("movX", mov.x);
			anim.SetFloat ("movY", mov.y);//estos son los parametros del animator para manejar el cambio de animaciones los setea con el vector antes definido
			anim.SetBool ("walking", true);//indica que tiene que caminar cuando hay velocidad
		} else {
			anim.SetBool ("walking", false);//si el vector es cero no esta caminando
		}
	}

	void AtaqueCuerpo(){
		//buscamos el estado actual del jugador con la informacion del animador
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		bool ataque = stateInfo.IsName ("Player_Attack"); //comprueba si esta esta animacion


		//detectamos el ataque va abajo de todo por la prioridad
		if (Input.GetKeyDown (KeyCode.LeftControl) && !ataque) { //comprueba si apreto la tecla y si no estoy atacando ya
			anim.SetTrigger ("Ataque"); //seteo el trigger a true
		}

		ActualizaElBoxCol ();

		if (ataque) {
			float playbackTime = stateInfo.normalizedTime; //aca almaceno el tiempo que dura la animacion
			if (playbackTime > 0.5 && playbackTime < 0.9) { //cuando la espada este completa
				areaAtaque.enabled = true;
			} else {
				areaAtaque.enabled = false; //si no desactuvo
			}
		}
	}

    void AtaqueHechizo (){
		//buscamos el estado actual del jugador con la informacion del animador
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);
		bool loading = stateInfo.IsName ("Player_Hechizo"); //comprueba si esta esta animacion


		//detectamos el ataque va abajo de todo por la prioridad
		if (Input.GetKeyDown (KeyCode.LeftShift)) { //comprueba si apreto la tecla y si no estoy atacando ya
			anim.SetTrigger ("Loading"); //seteo el trigger a true
			aura.AuraStart (); //pone la animacion del aura a funcionar
		} else if (Input.GetKeyUp (KeyCode.LeftShift)) { 
			anim.SetTrigger ("Idle");
			if (aura.IsLoaded ()) { //si esta cargada el aura crea la instancia del hechizo
				//conseguir rotacion de un vector
				float angle = Mathf.Atan2 (anim.GetFloat ("movY"), anim.GetFloat ("movX")) * Mathf.Rad2Deg; //siempre eje Y primero y la ultima funcion pasa de radianes a grados

				//instanciamos el hechizo pasamos prefab la pos del jugador el angulo o direccion vector forward siemore es hacia adelante
				GameObject hechizoObj = Instantiate (hechizoPrefab, transform.position, Quaternion.AngleAxis (angle, Vector3.forward));

				//le damos movimiento llamando a su funcion de hechizo.cs le seteamos al vector 2 los valores almacenados en el blend tree de walking
				Hechizo hechizo = hechizoObj.GetComponent <Hechizo> ();
				hechizo.mov.x = anim.GetFloat ("movX");
				hechizo.mov.y = anim.GetFloat ("movY");
			}
			aura.AuraStop (); //detiene la animacion del aura

			//esperamos un poco antes de darle movimiento otra vez
			StartCoroutine (EnableMovAfter (0.4f));
		}




			//privamos el movimiento al estar cargando
			if (loading) {
				movePrevent = true;
			}
		}

	void ActualizaElBoxCol(){
		if (mov != Vector2.zero) { //actualiza el box collider segun la posicion hacia la que este mirando para saber a donde va a atacar
			if (mov == Vector2.up) {
				areaAtaque.offset = new Vector2 (0f, 1f); // modifico los tamaños y offset del collider
				areaAtaque.size = areaAtaqueSizeY;
			} else if (mov == Vector2.down) {
				areaAtaque.offset = new Vector2 (0f, -1f);
				areaAtaque.size = areaAtaqueSizeY;
			} else if (mov == Vector2.left) {
				areaAtaque.offset = new Vector2 (-0.9f, 0f);
				areaAtaque.size = areaAtaqueSizeX;
			} else if (mov == Vector2.right) {
				areaAtaque.offset = new Vector2 (0.9f, 0f);
				areaAtaque.size = areaAtaqueSizeX;
			}
		}
	}

	void ActualizaLasBarras(){
		usuarioBarras.GetComponent<ManejoTextos>().SetVidaText(hp,hpmax);
		usuarioBarras.GetComponent<ManejoTextos>().SetManaText(mana,manamax);
		usuarioBarras.GetComponent<ManejoTextos>().SetExpText(exp);

		usuarioBarras.GetComponent<ManejaBarras>().ActualizaBarraVida(hp,hpmax);
		usuarioBarras.GetComponent<ManejaBarras>().ActualizaBarraMana(mana,manamax); //rescato del objeto el componente y la funcion en la misma linea
	}

	void PreventMovement(){ //para cuando carga el hechizo
		if (movePrevent) {
			mov = Vector2.zero;
		}
	}
	IEnumerator EnableMovAfter(float seconds){
		yield return new WaitForSeconds (seconds);
		movePrevent = false;
	}

	public void Atacado(int danio){ //cuando le saco vida llamo a este metodo
		hp = hp-danio;
		if (hp <=0){
			print("Moriste");
		}
	}

}
