using UnityEngine;
using System.Collections;

[RequireComponent(typeof (NavMeshAgent))]
public class Enemy : LivingEntity  {

	public enum State{
		Idle,
		Chasing,
		Attacking 
	}

	State currentState;

	NavMeshAgent pathfinder;
	Transform target;
	LivingEntity targetEntity;

	Material skinMaterial;
	Color originalClr; 

	float attackDistance=.5f; 
	float timeBetweenAttacks=1;
	float nextAttackTime;
	float damage = 1 ; 

	float myCollisionRadius;
	float targetCollisionRadius;

	bool hasTarget;

	protected override  void Start () {
		
		base.Start ();
		pathfinder = GetComponent<NavMeshAgent> ();

		skinMaterial = GetComponent<Renderer> ().material;
		originalClr = skinMaterial.color;

		if (GameObject.FindGameObjectWithTag ("Player") != null) { 
			
			currentState = State.Chasing;

			hasTarget = true; 
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();
			targetEntity.OnDeath += OnTargetDeath;

			myCollisionRadius = GetComponent<CapsuleCollider> ().radius; 
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius; 

			StartCoroutine (UpdatePath ());
		}



	}

	void OnTargetDeath(){

		hasTarget = false; 
		currentState = State.Idle;
	}

	void Update () {

		if ( hasTarget) {
			if (Time.time > nextAttackTime) {
				float sqrDistance = (target.position - transform.position).sqrMagnitude; 
				if (sqrDistance < Mathf.Pow (attackDistance + myCollisionRadius + targetCollisionRadius, 2)) {
					nextAttackTime = Time.time + timeBetweenAttacks;
					StartCoroutine (Attack ());

				}
			}
		}

	}
	IEnumerator Attack(){

		currentState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originPos = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPos = target.position - dirToTarget * (myCollisionRadius );  

		float attackSpeed = 3; 
		float percent = 0;
		skinMaterial.color = Color.red; 
		bool hasAppliedDmg = false ; 

		while (percent < 1) {

			if (percent >= 0.5f && !hasAppliedDmg) {
				hasAppliedDmg = true;
				targetEntity.TakeDamage (damage); 
			}
			percent += Time.deltaTime * attackSpeed; 
			float interpolation = (-Mathf.Pow(percent, 2)  + percent)*4;
			transform.position = Vector3.Lerp (originPos, attackPos, interpolation); 

			yield return null; 
		}
		skinMaterial.color = originalClr; 
		currentState = State.Chasing; 
		pathfinder.enabled = true;
	}


	IEnumerator UpdatePath(){
		float refreshRate = 0.25f;
		 
		while (hasTarget) {
			if (currentState == State.Chasing) {
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPos = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistance/2);
			
				if (!dead) {
					pathfinder.SetDestination (targetPos);
				}
			}
			yield return new WaitForSeconds (refreshRate);

		}
	}
}
