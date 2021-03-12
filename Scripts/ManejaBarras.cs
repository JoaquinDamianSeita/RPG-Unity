using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejaBarras : MonoBehaviour {

	
	public RawImage vida;
	public RawImage mana;
	public RawImage exp;

	public void ActualizaBarraVida(float hp,float hpmax){
		vida.transform.localScale = new Vector2 (hp/hpmax,1);
	}
	public void ActualizaBarraMana(float Mana,float Manamax){
		mana.transform.localScale = new Vector2 (Mana/Manamax,1);
	}
}
