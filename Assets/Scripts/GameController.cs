using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] hazards;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Text scoreText;
	private int score;

	public Text restartText;
	public GameObject restartButton;
	private bool restart;

	public Text gameOverText;
	private bool gameOver;

	void Start () {
		gameOver = false;
		restart = false;
		gameOverText.text = "";
		#if UNITY_IPHONE || UNITY_ANDROID
			restartButton.SetActive(false);
		#else
			restartText.text = "";
		#endif
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
	}

	void Update (){
		if (restart) {
			if (Input.GetKeyDown(KeyCode.R)) {
				RestartGame();
			}
		}
	}

	IEnumerator SpawnWaves () {
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazards[Random.Range(0, hazards.Length)], spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);

			if(gameOver){
				#if UNITY_IPHONE || UNITY_ANDROID
					restartButton.SetActive(true);
				#else
					restartText.text = "Appuyez sur 'R' pour rejouer !";
				#endif
				restart = true;
				break;
			}
		}
	}

	public void AddScore(int newScore){
		score += newScore;
		UpdateScore ();
	}

	void UpdateScore () {
		scoreText.text = "Score : " + score;
	}

	public void GameOver(){
		gameOverText.text = "Game Over !";
		gameOver = true;
	}

	public void RestartGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}
