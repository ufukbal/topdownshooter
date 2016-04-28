using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour {

	public Rigidbody myRigidbody;
	public float minForce;
	public float maxForce;

	float lifeTime = 4;
	float fadeTime = 2;

	// Use this for initialization
	void Start () {
		float force = Random.Range (minForce, maxForce);
		myRigidbody.AddForce (transform.right * force);
		myRigidbody.AddTorque (Random.insideUnitSphere * force);

		StartCoroutine (Fade ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Fade(){
		yield return new WaitForSeconds (lifeTime);

		float percent = 0;
		float fadeSpeed = 1 / fadeTime;

		Material mat = GetComponent<Renderer> ().material;
		Color initColor = mat.color;

		while (percent < 1) {
			percent += Time.deltaTime * fadeSpeed;
			mat.color = Color.Lerp (initColor, Color.clear, percent);
			yield return null;
		}

		Destroy (gameObject);
	}

}
