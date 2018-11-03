using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public float health = 100, maxHealth = 100;
    public bool canDestory = false, canDeactivate = false, useSliderHp = false, bTestDmg = false;
    [SerializeField]
    Slider healthDisplay;
    [SerializeField]
    GameObject greenHealth, redHealth, lookAtTarget, lookAtFrom = null;
    [SerializeField]
    bool lockPos = false, lockRot = false, lookAt = false;
    [SerializeField]
    float healthbarOffset = 3;
    [SerializeField]
    ParticleSystem deathParticle;
    [SerializeField]
    AudioSource deathSound;

    Quaternion startRot;
    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        UpdateHealth();
        startRot = greenHealth.transform.parent.parent.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (bTestDmg && Input.GetKey(KeyCode.T))
        {
            Damage();
        }
        if (lookAtTarget != null && lookAt)
        {
            if (lookAtFrom != null)
            {
                HealthLookAt(lookAtTarget.transform, lookAtFrom.transform);
            }
            else
            {
                HealthLookAt(lookAtTarget.transform);
            }
        }
        HealthbarLock();
    }
    #region Change Health
    public void Damage(float dmg = 1, bool useUI = true)
    {
        health -= dmg;
        if (health < 0) health = 0;
        if (health <= 0)
        {
            Die();
        }
        UpdateHealth();
    }
    void Die()
    {
        if (deathParticle != null)
        {
            ParticleSystem dieParticle = Instantiate(deathParticle, transform.position, transform.rotation, null);
            dieParticle.Play();
            Destroy(dieParticle, 3f);
        }
        if (deathSound != null)
        {
            AudioSource dieSound = Instantiate(deathSound, transform.position, transform.rotation, null);
            dieSound.Play();
            Destroy(dieSound, 3f);
        }

        if (canDestory)
        {
            if (healthDisplay != null) Destroy(healthDisplay);
            Destroy(gameObject);
        }
        if (canDeactivate)
        {
            if (healthDisplay != null) healthDisplay.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    void UpdateHealth()
    {
        if (healthDisplay != null)
        {
            healthDisplay.value = health;
        }
        if (greenHealth != null || redHealth != null)
        {
            Vector3 greenPos = greenHealth.transform.localPosition, greenScale = greenHealth.transform.localScale;
            greenScale.x = health / 100;
            greenPos.x = ((1 - greenScale.x) / 2) * -1;
            greenHealth.transform.localScale = greenScale;
            greenHealth.transform.localPosition = greenPos;
        }
    }
    void SetHealth(float newHealth)
    {
        health = newHealth;
        if (health < 0) health = 0;
        if (health > maxHealth) health = maxHealth;
        UpdateHealth();
    }
    public void AddHealth(float moreHealth = 1, bool useUI = true)
    {
        health += moreHealth;
        if (health < maxHealth) health = maxHealth;
        UpdateHealth();
    }
    public void DefaultHealth()
    {
        health = maxHealth;
        if (healthDisplay != null)
        {
            healthDisplay.minValue = 0;
            healthDisplay.maxValue = maxHealth;
        }
        else
        {
            if (greenHealth == null || redHealth == null) return;
        }
        UpdateHealth();
    }
    public void FullHeal()
    {
        SetHealth(maxHealth);
    }
    #endregion
    void HealthLookAt(Transform target, Transform lookAtFrom = null)
    {
        if (greenHealth == null || redHealth == null) return;

        if (lookAtFrom != null)
        {
            lookAtFrom.LookAt(target);
        }
        else
        {
            greenHealth.transform.parent.parent.LookAt(target);
            //redHealth.transform.LookAt(target);
        }
    }
    void HealthbarLock()
    {
        if (lockPos)
        {
            Vector3 lockPos = greenHealth.transform.root.position;
            lockPos.z += healthbarOffset;
            greenHealth.transform.parent.parent.position = lockPos;
        }
        if (lockRot)
        {
            startRot.w = greenHealth.transform.parent.parent.rotation.w;
            greenHealth.transform.parent.parent.rotation = startRot;
        }
    }
}
