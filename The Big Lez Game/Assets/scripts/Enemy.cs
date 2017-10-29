using UnityEngine;
using System.Collections;

public class Enemy : Actor, IPoolable<Enemy>
{
	public static int total;

	public PoolData<Enemy> poolData { get; set; }

	[SerializeField]
	GameObject corpse;
	[SerializeField]
	AudioClip[] deathSound;

	void Update ()
	{
		Movement();
	}

	public void OnPooled (Vector3 startPos)
	{
		total++;
		//set everything up
		m_rigidbody.velocity = Vector2.zero;
		transform.position = startPos;

		base.Start();
		int i = 0;
		foreach (SpriteRenderer s in sr) {
			s.color = standardCol [i];
			i++;
		}

		velocity = new Vector2 (Random.Range(0.05f,0.15f), Random.Range(0.05f,0.15f));

		gameObject.SetActive(true);
	}

	public override void Knockback(GameObject _affectingObject, float _amount)
	{
		if (_affectingObject.GetComponent<Projectile>())
			m_rigidbody.AddForce(_affectingObject.GetComponent<Projectile>().transform.right * _amount);
		else
			base.Knockback(_affectingObject, _amount);
	}

	public override void Death ()
	{
		total--;
		SoundManager.instance.playSound(deathSound [Random.Range(0,deathSound.Length)],1,Random.Range(0.9f,1.1f));
		BloodParticles.instance.Blood(transform.position,Random.Range(16,22));
		Instantiate(corpse,transform.position,transform.rotation,transform.parent);
		//frameHolder.instance.holdFrame();
		ReturnPool();
		base.Death();
	}

	public void ReturnPool ()
	{
		if (poolData != null)
			poolData.ReturnPool(this);
		gameObject.SetActive(false);
	}

	Vector2 velocity;
	Vector2 goalPos;
	Vector2 currentForce;

	public override void LateUpdate ()
	{
	}

	public override void Movement ()
	{
		//base.Movement();

		Vector3 target = Player.instance.transform.position;
		#region boid movement

		velocity = m_rigidbody.velocity;

		if (EnemyManager.instance.allCunts.Count > 0) {
		}
		else {
			m_rigidbody.AddForce((target - transform.position).normalized * moveSpeed);
		}

		//transform.position = Vector3.Lerp(transform.position,target,Time.deltaTime);

		#endregion

		/*if (target.x > transform.position.x)
        {
            if (rigBod.velocity.x < moveSpeed)
            rigBod.AddForce(Vector2.right * 5);
        }
        if (target.x < transform.position.x)
        {
            if (rigBod.velocity.x > -moveSpeed)
                rigBod.AddForce(Vector2.left * 5);
        }

            transform.localScale = new Vector3(target.x > transform.position.x ? 1 : -1, 1, 1);
        if (target.y > transform.position.y)
        {
            if (rigBod.velocity.y < moveSpeed)
                rigBod.AddForce(Vector2.up * 5);
        }
        if (target.y < transform.position.y)
        {
            if (rigBod.velocity.y > -moveSpeed)
                rigBod.AddForce(Vector2.down * 5);
        }*/

	}
}
