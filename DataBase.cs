using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "InventorySystem/Database")]
public class DataBase : ScriptableObject 
{
	public List <Item> items = new List<Item>(); //creo una nueva lista del tipo items

	public Item FindItemDatabase(int id) //funcion para rastrear items por id
	{
		foreach(Item item in items)
		{
			if (item.id == id)
			{
				return item; //si encuentra
			}
		}
		return null; //si no encuentra
	}
}

[System.Serializable] //es un objeto serializable
public class Item
{
	public int id;
	public string name;
	[TextArea(5,5)]
	public string descripcion;
	public int cost;
	public int sellCost;
	public int usaLevel;
	public Stats stats;

	[System.Serializable]
	public struct Stats
	{
		public int danio;
		public int defensa;
	}
}




