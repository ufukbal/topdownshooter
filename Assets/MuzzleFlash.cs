using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {

	public GameObject flashHolder;
	public float flashTime;

	public Sprite[] flashSprites;
	public SpriteRenderer[] spriteRenderers;


	public void Start(){
		Deactivate();
	}

	public void Activate(){
		flashHolder.SetActive (true);

		int flashIndex = Random.Range (0, flashSprites.Length);
		for (int i = 0; i < spriteRenderers.Length; i++) {
			spriteRenderers [i].sprite = flashSprites [flashIndex];
		}

		Invoke ("Deactivate", flashTime);
	}

	public void Deactivate(){
		flashHolder.SetActive (false);
	}
}
