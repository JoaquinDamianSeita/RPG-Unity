using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejoTextos : MonoBehaviour {

	public Text textoVida;
	public Text textoMana;
	public Text textoExp;
	// Use this for initialization

	public void SetVidaText(int hp, int hpmax){
		string Hp = hp.ToString();
		string Hpmax = hpmax.ToString();
		textoVida.text = Hp + "/" + Hpmax;
	}
	public void SetManaText(int mana, int manamax){
		string Mana = mana.ToString();
		string Manamax = manamax.ToString();
		textoMana.text = Mana + "/" + Manamax;
	}
	public void SetExpText(int exp){
		string Exp = exp.ToString();
		textoExp.text = Exp;
	}
}
