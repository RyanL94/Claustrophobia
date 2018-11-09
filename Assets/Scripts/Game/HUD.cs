using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {

	public Slider healthBar;
	public Slider ammoBar;
	public float updateSpeed;

	private GameController game;
	private GameObject player;
	private Damageable playerHealth;
	private PlayerController playerController;

    public GameObject pauseMenu;
    private bool paused = false;

    void Awake() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        pauseMenu.SetActive(false);
    }

	void Start() {
		player = game.player;
		playerHealth = player.GetComponent<Damageable>();
		playerController = player.GetComponent<PlayerController>();
		healthBar.maxValue = playerHealth.health;
		healthBar.value = playerHealth.health;
		ammoBar.maxValue = playerController.ammo;
		ammoBar.value = playerController.ammo;
	}
	
	void Update() {
        //if (Input.GetKeyDown(KeyCode.Escape)) {
        //    pauseGame();
        //}

        var progression = Time.deltaTime * updateSpeed;
		if (playerHealth.health != healthBar.value) {
			healthBar.value = Mathf.Lerp(healthBar.value, playerHealth.health, progression);
		}
		if (playerController.ammo != ammoBar.value) {
			ammoBar.value = Mathf.Lerp(ammoBar.value, playerController.ammo, progression);
		}
	}

    // Pause game and display menu
    private void pauseGame() {
        if (paused) {
            Resume();
        }
        else {
            Pause();
        }
    }

    public void Resume() {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause() {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart() {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void Quit() {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
