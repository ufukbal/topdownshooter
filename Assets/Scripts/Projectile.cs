 using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	float speed = 10;
	float damage =1;
	public LayerMask collisionMask;

	float lifeTime = 3;
	float skinWidht = 5.1f;

	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
	} 

	void Start(){
		Destroy (gameObject, lifeTime);

		Collider[] initialCollisions = Physics.OverlapSphere (transform.position, .1f, collisionMask);
		if (initialCollisions.Length > 0) {
			OnHitObject(initialCollisions[0]);
		}
	}

	void Update () {
		float moveDistance = speed * Time.deltaTime;
		transform.Translate (Vector3.forward * moveDistance);
		CheckCollisions (moveDistance);
	}

	public void CheckCollisions(float moveDistance){

		Ray ray = new Ray (transform.position, transform.forward); 
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, moveDistance+skinWidht, collisionMask, QueryTriggerInteraction.Collide)) { //query for istrigger on object  
			OnHitObject (hit);
		}
	}
	
	void OnHitObject(RaycastHit hit ){
		IDamageable damageableObject = hit.collider.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeHit (damage, hit); 
		}

		print (hit.collider.gameObject.name); 
		GameObject.Destroy (gameObject); //do pooling

	}
	void OnHitObject(Collider c) {
		IDamageable damageableObject = c.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeDamage(damage);
		}
		GameObject.Destroy (gameObject);
	}
}