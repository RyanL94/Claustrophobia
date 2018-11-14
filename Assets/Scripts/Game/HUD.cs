using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {

    public PopUp popUp;
    public CanvasGroup itemBanner;
    public Text itemName;
    public Text itemDescription;
    public float itemBannerDuration;
    public Text money;
    public Text healthText;
    public Text ammoText;
	public Slider healthBar;
	public Slider ammoBar;
    public Slider bossHealthBar;
	public float updateSpeed;

	private GameController game;
	private PlayerController player;
	private Damageable playerHealth;

    public GameObject pauseMenu;
    private Animator itemBannerAnimator;
    private Animator bossHealthAnimator;
    private Damageable bossHealth;
    private bool paused = false;

	void Start() {
		game = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		player = game.player;
		playerHealth = player.gameObject.GetComponent<Damageable>();
		healthBar.maxValue = playerHealth.health;
		healthBar.value = playerHealth.health;
		ammoBar.maxValue = player.ammo;
		ammoBar.value = player.ammo;
        pauseMenu.SetActive(false);
        itemBannerAnimator = itemBanner.GetComponent<Animator>();
        bossHealthAnimator = bossHealthBar.GetComponent<Animator>();
        bossHealthAnimator.CrossFadeInFixedTime("Transparent", 0);
	}
	
	void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseGame();
        }

        var progression = Time.deltaTime * updateSpeed;
		if (playerHealth.health != healthBar.value) {
			healthBar.value = Mathf.Lerp(healthBar.value, playerHealth.health, progression);
            healthText.text = string.Format("{0} / {1}", playerHealth.health, healthBar.maxValue);
		}
		if (player.ammo != ammoBar.value) {
			ammoBar.value = Mathf.Lerp(ammoBar.value, player.ammo, progression);
            ammoText.text = string.Format("{0} / {1}", Mathf.Floor(player.ammo), ammoBar.maxValue);
		}
        if (game.boss != null && bossHealth.health != bossHealthBar.value) {
            bossHealthBar.value = Mathf.Lerp(bossHealthBar.value, bossHealth.health, progression);
        }
        money.text = player.money.ToString() + " G";
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
        ResetPauseMenuButtons();
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

    public void DisplayItem(string name, string description) {
        itemName.text = string.Format("NEW POWER UP! {0}", name);
        itemDescription.text = description;
        StartCoroutine(DisplayItemHelper());
    }

    public void DisplayBossHealth() {
        bossHealth = game.boss.GetComponent<Damageable>();
        bossHealthBar.maxValue = bossHealth.health;
        bossHealthBar.value = bossHealth.health;
        bossHealthAnimator.CrossFadeInFixedTime("Opaque", 0.5f);
    }

    public void HideBossHealth() {
        bossHealthAnimator.CrossFadeInFixedTime("Transparent", 0.5f);
    }

    private IEnumerator DisplayItemHelper() {
        itemBannerAnimator.CrossFadeInFixedTime("Opaque", 0.10f);
        yield return new WaitForSeconds(itemBannerDuration);
        itemBannerAnimator.CrossFadeInFixedTime("Transparent", 0.25f);
    }

    private void ResetPauseMenuButtons() {
        foreach (Transform child in pauseMenu.transform) {
            child.localScale = Vector3.one;
        }
    }
}
