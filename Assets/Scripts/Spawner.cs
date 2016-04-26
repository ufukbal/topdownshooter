using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public Enemy enemy;

	Wave currentWave;
	int currentWaveNumber;

	int enemiesToSpawn; 
	int enemiesAlive; 
	float nextSpawnTime;

	void Start(){
		NextWave (); 
	}

	void Update(){
		if (enemiesToSpawn > 0 && Time.time > nextSpawnTime ) {
			enemiesToSpawn--;
			nextSpawnTime = Time.time + currentWave.timeBetweenSpawns; 

			Enemy spawnedEnemy = Instantiate (enemy, Vector3.zero, Quaternion.identity) as Enemy;
			spawnedEnemy.OnDeath += OnEnemyDeath;  
		}
	}

	void OnEnemyDeath(){
		enemiesAlive--;
		if (enemiesAlive == 0) {
			NextWave ();
		}
	}


	void NextWave(){

		currentWaveNumber++;
		Debug.Log("Wave: " + currentWaveNumber) ;
		if (currentWaveNumber - 1 < waves.Length) {

			currentWave = waves [currentWaveNumber - 1];
			enemiesToSpawn = currentWave.enemyCount; 
			enemiesAlive = enemiesToSpawn; 
		}
	}

	[System.Serializable]
	public class Wave{
		
		public int enemyCount;
		public float timeBetweenSpawns;
		 
	}


}
