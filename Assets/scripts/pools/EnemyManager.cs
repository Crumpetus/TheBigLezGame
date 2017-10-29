using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public  float amp = 1;

    public static EnemyManager instance;
    public List<Enemy> allCunts = new List<Enemy>();

    public Vector2 objectCentre, objectVelocity;

    private ObjectPool<Enemy> objectPool = null;
    [SerializeField]
    private Enemy CuntPrefab = null;

    void Start()
    {
        amp = Random.Range(3, 10);
        instance = this;
        objectPool = new ObjectPool<Enemy>(CuntPrefab, 15, transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value > .95f && allCunts.Count < 50)
            PoolEnemy(transform.position + (Random.insideUnitSphere * 3));

        if (allCunts.Count > 0)
        {
            objectCentre = Vector2.zero;
            objectVelocity = Vector2.zero;

            foreach (Enemy e in objectPool.objects)
            {
                objectCentre += new Vector2(e.transform.localPosition.x, e.transform.localPosition.y);
                objectVelocity += e.m_rigidbody.velocity;
            }
            objectCentre /= allCunts.Count;
            objectVelocity /= allCunts.Count;
        }

    }

    public Enemy PoolEnemy(Vector3 startPos)
    {
        Enemy choomah = objectPool.GetPooledObject(transform);
        choomah.transform.position = new Vector3(choomah.transform.position.x, 0, choomah.transform.position.z);
        choomah.OnPooled(startPos);
        choomah.m_manager = this;
        allCunts.Add(choomah);
        return choomah;
    }
}
