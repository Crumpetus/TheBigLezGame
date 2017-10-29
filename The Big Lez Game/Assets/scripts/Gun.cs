using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {


	[Header("Gun stats")]
	public string m_gunName;
	public float m_fireRate;
	public float m_initialBulletSpeed;
	public float m_bulletAcceleration;
	public float m_bulletSpread;
	public float m_timeToReload;
	public int m_barrels;
	public int m_bulletsPerBarrel;
	public int m_bulletDamage;
	public int m_enemiesPierced;
	public int m_clipSize;
	public bool m_explosiveBullets;

	[Header("Current Gun Values")]
	private bool reloading = false;
	private bool recentlyFired = false;
	private float reloadTimer = 0.0f;
	private float firingTimer = 0.0f;
	private float firingAngle;
	private int currentBulletsInClip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (reloading)
		{
			reloadTimer += Time.deltaTime;
			if (reloadTimer >= m_timeToReload)
			{
				//finish reloading
				currentBulletsInClip = m_clipSize;
				reloading = false;
			}
		}
		if (recentlyFired)
		{
			firingTimer += Time.deltaTime;
			if (firingTimer >= m_fireRate)
			{
				recentlyFired = false;
			}
		}
	}

	public void InitialiseGun()
	{
		float totalAngle = m_bulletSpread * m_bulletsPerBarrel * m_barrels * m_bulletsPerBarrel;
	}

	public void Shoot(bool _reverse)
	{
		if (CanShoot())
		{
			if (reloading)
				reloading = false;

			//fire bullet(s)
			for (int i = 0; i < m_barrels; i++)
			{
				Vector3 AimingPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				float SpreadAngle = Mathf.Atan2(AimingPosition.y - transform.position.y, AimingPosition.x - transform.position.x) * Mathf.Rad2Deg;
				if (_reverse)
					SpreadAngle += 180;
				Vector3 BulletDirection;
				//if (m_barrels)
			}

			firingTimer = 0.0f;
			recentlyFired = true;
			if (currentBulletsInClip <= 0)
				BeginReload();
		}
	}

	public virtual bool CanShoot()
	{
		if (currentBulletsInClip > 0 && !recentlyFired)
			return true;
		return false;
	}

	public virtual void BeginReload()
	{
		if (!reloading)
		{
			reloadTimer = 0.0f;
			reloading = true;
		}
	}
}
