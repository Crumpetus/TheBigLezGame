using UnityEngine;
using System.Collections;

public class Enemy : Actor, IPoolable<Enemy>
{
    public static int total;

    public PoolData<Enemy> poolData { get; set; }

    public EnemyManager m_manager;

    [SerializeField]
    GameObject corpse;
    [SerializeField]
    AudioClip[] deathSound;

    float minVel = 7,
        maxVel = 50;

    void Update()
    {
        Movement();
    }

    public void OnPooled(Vector3 startPos)
    {
        total++;
        //set everything up
        m_rigidbody.velocity = Vector2.zero;
        transform.position = startPos;

        base.Start();
        int i = 0;
        foreach (SpriteRenderer s in sr)
        {
            s.color = standardCol[i];
            i++;
        }

        gameObject.SetActive(true);
    }

    public override void Knockback(GameObject _object, float _amount)
    {
        if (_object.GetComponent<Projectile>())
            m_rigidbody.AddForce(_object.transform.right * _amount);
        else
            base.Knockback(_object, _amount);
    }

    public override void Death()
    {
        total--;
        SoundManager.instance.playSound(deathSound[Random.Range(0, deathSound.Length)], 1, Random.Range(0.9f, 1.1f));
        BloodParticles.instance.Blood(transform.position, Random.Range(16, 22));
        Instantiate(corpse, transform.position, transform.rotation, transform.parent);
        frameHolder.instance.holdFrame();
        ReturnPool();
        base.Death();
    }

    public void ReturnPool()
    {
        m_manager.allCunts.Remove(this);
        if (poolData != null)
            poolData.ReturnPool(this);
        gameObject.SetActive(false);
    }

    public override void LateUpdate()
    {
    }

    float timeSinceLastMove = 0;

    public override void Movement()
    {
        //base.Movement();
        #region boid movement
        timeSinceLastMove += Time.deltaTime;

        if (timeSinceLastMove > 0.15f)
        {
            timeSinceLastMove = 0;
            m_rigidbody.velocity = (m_rigidbody.velocity * .15f) + Calc() * Time.deltaTime * m_manager.amp;

            if (m_rigidbody.velocity.magnitude > maxVel)
            {
                m_rigidbody.velocity = m_rigidbody.velocity.normalized * maxVel;
            }
            else if (m_rigidbody.velocity.magnitude < minVel)
            {
                m_rigidbody.velocity = m_rigidbody.velocity.normalized * minVel;
            }
        }
        #endregion
    }

    public float rando;

    Vector2 Calc()
    {
        float coheesion = Random.Range(.05f, .25f);
            
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 target = new Vector2(Player.instance.transform.position.x, Player.instance.transform.position.y);

        Vector2 groupCentre = m_manager.objectCentre - pos;
        Vector2 groupVelocity = m_manager.objectVelocity - m_rigidbody.velocity;

        float r = 1;

        groupCentre *= coheesion;
        groupVelocity *= coheesion;

        Vector2 randomVector = new Vector2(Random.Range(-r, r), Random.Range(-r, r));
        rando = r;
        sr[0].flipX = pos.x > target.x;

        Vector2 returnMe = groupCentre + groupVelocity +
                           (((target - pos) + randomVector) * 2);

        if (float.IsNaN(returnMe.x) || float.IsNaN(returnMe.y))
        {
            Debug.Log("NaN!");
            return Vector2.zero;    
        }
        return returnMe;
    }
}
