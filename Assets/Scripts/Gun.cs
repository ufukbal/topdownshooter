﻿using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public Transform shell;
	public Transform ejectionPoint;

	public Transform muzzle;
	public Projectile projectile;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;
	MuzzleFlash muzzleflash;
	float nextShotTime;

	public void Start(){
		muzzleflash = GetComponent<MuzzleFlash> ();
	}

	public void Shoot() {

		if (Time.time > nextShotTime) {
			nextShotTime = Time.time + msBetweenShots / 1000;
			Projectile newProjectile = Instantiate (projectile, muzzle.position, muzzle.rotation) as Projectile;
			newProjectile.SetSpeed (muzzleVelocity);

			Instantiate (shell, ejectionPoint.position, ejectionPoint.rotation);
			muzzleflash.Activate ();
		}
	}
}