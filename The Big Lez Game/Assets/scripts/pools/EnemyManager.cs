using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager instance;
	public List<Enemy> allCunts = new List<Enemy> ();

	private ObjectPool<Enemy> objectPool = null;
	[SerializeField]
	private Enemy CuntPrefab = null;

	void Start ()
	{
		instance = this;
		objectPool = new ObjectPool<Enemy> (CuntPrefab, 15, transform);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Random.value > .95f && Enemy.total < 90)
			PoolEnemy(transform.position + (Random.insideUnitSphere * 3));
	}

	public Enemy PoolEnemy (Vector3 startPos)
	{
		Enemy choomah = objectPool.GetPooledObject(transform);
		choomah.transform.position = new Vector3 (choomah.transform.position.x, 0, choomah.transform.position.z);
		choomah.OnPooled(startPos);
		allCunts.Add(choomah);
		return choomah;
	}
}
