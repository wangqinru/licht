using UnityEngine;
using System.Collections;

public class ItemBase {

	public int id { get; private set;}
	public string name { get; private set;}
	public string description { get; private set;}
	public Texture image { get; private set;}
	public int number { get; set;}

	public ItemBase (ItemData i, int n)
	{
		id = i.id;
		name = i.name;
		description = i.work;
		image = i.image;
		number = n;
	}

	public virtual void ItemEvent (ShinBattleController s)
	{
	}

	public virtual void ItemEvent (ShinBattleController[] ss)
	{

	}
}
