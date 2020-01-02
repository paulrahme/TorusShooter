using System.Collections.Generic;
using UnityEngine;

public class RecycleStack
{
	Stack<GameObject> Pool = new Stack<GameObject>();

	public void Recycle(GameObject gameObj)
	{
		gameObj.SetActive(false);
		Pool.Push(gameObj);
	}

	public GameObject RetrieveOrCreate(GameObject prefab, bool setActive = true)
	{
		GameObject gameObj  = (Pool.Count > 0) ? Pool.Pop() : GameObject.Instantiate(prefab);

		if (setActive)
			gameObj.SetActive(true);

		return gameObj;
	}

	public void Empty()
	{
		while (Pool.Count > 0)
			GameObject.Destroy(Pool.Pop());
	}

	public void Clear()
	{
		Pool.Clear();
	}
}