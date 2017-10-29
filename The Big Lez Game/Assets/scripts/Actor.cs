using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	[Header("Base stats")]
	public float moveSpeed, MaxHP;
	public float footstepAmp;
	float HP;
	bool Invulnerable = false;

	[Header("Base references")]
	public Rigidbody2D m_rigidbody;
	public SpriteRenderer[] sr;
	public Color[] standardCol;
	public Animator anim;
	public Vector3 _movement;

	public virtual void Start ()
	{
		HP = MaxHP;
		standardCol = new Color[sr.Length];
		for (int i = 0; i < sr.Length; i++) {
			standardCol [i] = sr [i].color;
			standardCol [i] = Color.white;
		}
	}

	void Update ()
	{

	}

	public void Move (float x, float y)
	{
		m_rigidbody.AddForce(Vector2.right * x);
		m_rigidbody.AddForce(Vector2.up * y);

		//_movement.x += x;
		//_movement.y += y;
	}

	public virtual void LateUpdate ()
	{
		Vector3 pos = transform.position;
		Vector3 clamped_position = new Vector2 ((int)pos.x, (int)pos.y);
		transform.position = clamped_position;


		// Clamp the current movement
		Vector3 clamped_movement = new Vector3 ((int)_movement.x, (int)_movement.y, 0);
		// Check if a movement is needed (more than 1px move)
		if (clamped_movement.magnitude >= 1.0f) {
			// Update velocity, removing the actual movement
			_movement = _movement - clamped_movement;
			if (clamped_movement != Vector3.zero) {
				// Move to the new position
				transform.position = (transform.position) + clamped_movement;
			}
		}
	}

	public virtual IEnumerator TakeDamage (GameObject _affectingObject, float damage)
	{
		if (!Invulnerable)
		{
			Knockback(_affectingObject, damage);
			HP -= damage;
			if (HP <= 0)
				Death();
			foreach (SpriteRenderer s in sr)
			{
				s.color = Color.red;
			}
		}
		else
		{
			foreach (SpriteRenderer s in sr)
			{
				s.color = Color.yellow;
			}
		}
		yield return new WaitForSeconds(0.1f);

		int i = 0;
		foreach (SpriteRenderer s in sr)
		{
			s.color = standardCol[i];
			i++;
		}
	}

	public virtual void Knockback (GameObject _affectingObject, float _amount)
	{
		m_rigidbody.AddForce(Vector3.Normalize(transform.position - _affectingObject.transform.position) * _amount * 2);
	}

	public virtual void Death ()
	{
		gameObject.SetActive(false);
	}

	public virtual void Movement ()
	{
		if (anim) {
			anim.SetBool("walking",(m_rigidbody.velocity.magnitude / 10) > 0.1f ? true : false);
			anim.SetFloat("speed",1 + m_rigidbody.velocity.magnitude / 10);
		}
	}

	[SerializeField]
	AudioClip[] footstepSounds;

	public void Footstep ()
	{
		SoundManager.instance.playSound(footstepSounds [Random.Range(0,footstepSounds.Length)],0.25f * footstepAmp);
	}
}
