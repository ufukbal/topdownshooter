using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public Enemy enemy;

	LivingEntity playerEntity;
	Transform playerT;

	Wave currentWave;
	int currentWaveNumber;

	int enemiesToSpawn; 
	int enemiesAlive; 
	float nextSpawnTime;

	MapGenerator map;

	float campThreshold = 1.5f;
	float timeBetweenCampingCheck = 2;
	float nextCampCheck;
	Vector3 campPos;
	bool isCamping;

	bool isDisabled;

	public event System.Action<int> OnNewWave;

	void Start(){
		playerEntity = FindObjectOfType<Player> ();
		playerT = playerEntity.transform;
		playerEntity.OnDeath += onPlayerDeath;

		nextCampCheck = timeBetweenCampingCheck + Time.time;
		campPos = playerT.position;

		map = FindObjectOfType<MapGenerator> ();
		NextWave (); 
	}

	void Update(){
		if (!isDisabled) {
			if (Time.time > nextCampCheck) {
				nextCampCheck = timeBetweenCampingCheck + Time.time;

				isCamping = (Vector3.Distance (playerT.position, campPos) < campThreshold);
				campPos = playerT.position;
			}

			if (enemiesToSpawn > 0 && Time.time > nextSpawnTime) {
				enemiesToSpawn--;
				nextSpawnTime = Time.time + currentWave.timeBetweenSpawns; 

				StartCoroutine ("SpawnEnemy");
			}
		}
	}

	IEnumerator SpawnEnemy(){

		float spawnDelay = 1;
		float tileFlashSpeed = 4;

		Transform spawnTile = map.GetRandomOpenTile ();
		if (isCamping) {
			spawnTile = map.GetTileFromPosition (playerT.position);
		}
		Material tileMat = spawnTile.GetComponent<Renderer> ().material;
		Color initColor = tileMat.color;
		Color flashColor = Color.red;
		float spawnTimer = 0;

		while (spawnTimer < spawnDelay) {

			tileMat.color = Color.Lerp (initColor, flashColor, Mathf.PingPong (spawnTimer * tileFlashSpeed, 1));
				
			spawnTimer += Time.deltaTime;
			yield return null;
		}


		Enemy spawnedEnemy = Instantiate (enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
		spawnedEnemy.OnDeath += OnEnemyDeath;
	}

	void onPlayerDeath(){
		isDisabled = true;
	}

	void OnEnemyDeath(){
		enemiesAlive--;
		if (enemiesAlive == 0) {
			NextWave ();
		}
	}

	void ResetPlayer(){
		playerT.position = map.GetTileFromPosition (Vector3.zero).position + Vector3.up*3;
	}

	void NextWave(){

		currentWaveNumber++;
		if (currentWaveNumber - 1 < waves.Length) {

			currentWave = waves [currentWaveNumber - 1];
			enemiesToSpawn = currentWave.enemyCount; 
			enemiesAlive = enemiesToSpawn; 

			if (OnNewWave != null) {
				OnNewWave (currentWaveNumber);
			}
			ResetPlayer ();
		}
	}

	[System.Serializable]
	public class Wave{
		
		public int enemyCount;
		public float timeBetweenSpawns;
		 
	}


}
