using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    //UI for win/lose
    [SerializeField] private GameObject loseUI;
    public AudioSource hitSound;

    [SerializeField] private ParticleSystem hitParticleFX;
    [SerializeField] private GameObject boat;
    public KayakDrift kayakDrift;
    public int maxHealth = 100;
    public int healAmount = 10;
    public int currentHealth;
    public bool isDead;
    public Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
 
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Set the slider's max value to the kayak's max health
            healthSlider.value = currentHealth; // Set the initial value of the slider
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        // Cancel auto-healing if it is running
        CancelInvoke("AutoHeal");

        currentHealth -= damage;


        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Check if health has reached 0 and handle death
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            //Auto Heal
            InvokeRepeating("AutoHeal", 2f, 5f);
        }
    }

    public void Heal(int amount)
    {
        if (!isDead) return;

     
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Update the health slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

    }

    public void Die() {
        StartCoroutine(LoseScreenLoader());
        CancelInvoke("AutoHeal");
        hitParticleFX.Stop();
        kayakDrift.moveSpeed = 0f;
        DOTween.Play("DeadFlip");
        Debug.Log("Dead, Game over!");
    }

    void AutoHeal()
    {
        if (!isDead)
        {
            Heal(healAmount); // Heal the specified amount
        }
    }

    public void ResetHealth()
    {
        isDead = false;
        currentHealth = maxHealth;
        // Update the health slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        Debug.Log(gameObject.name + " health reset! Current health: " + currentHealth);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {
            hitSound.Play();
            TakeDamage(50);
            if (!isDead)
            {
                hitParticleFX.Play();
            }
            DOTween.Restart("Hit");
        }
    }

    IEnumerator LoseScreenLoader() {

        yield return new WaitForSeconds(0.5f);
        loseUI.SetActive(true);

    }

    void OnDestroy()
    {
        CancelInvoke("AutoHeal");
    }
}
