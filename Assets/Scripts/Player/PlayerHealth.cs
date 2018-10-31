using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int playerHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Text healthText;
    public Image redFlash;
    public Color flashColor = new Color(1.0f, 0.0f, 0.0f, 0.25f);
    public float flashSpeed = .1f;

    float time = 0.0f;
    float damageCooldown = 5f;
    bool hit = false;
    bool dead = false;


    // Use this for initialization
    void Start ()
    {
        currentHealth = playerHealth;
        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
        hit = false;

        time += Time.deltaTime;

        if (time > damageCooldown && currentHealth > 0)
        {
            loseHealth(10);
            time = 0.0f;
        }

        if (!hit)
        {
            redFlash.color = Color.Lerp(redFlash.color, Color.clear, flashSpeed * Time.deltaTime);
            hit = false;
        }
	}

    public void loseHealth (int damage)
    {
        hit = true;
        currentHealth -= damage;
        redFlash.color = flashColor;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerDeath();
        }

        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString();
    }

    public void playerDeath()
    {
        dead = true;
    }
}
