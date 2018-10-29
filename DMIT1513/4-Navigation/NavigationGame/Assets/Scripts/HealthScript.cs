using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public float health = 100, maxHealth = 100;
    public bool canDestory = false, useSliderHp = false, bTestDmg = false;
    [SerializeField]
    Slider healthDisplay;
    [SerializeField]
    GameObject greenHealth, redHealth, lookAtTarget, lookAtFrom = null;
    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (bTestDmg && Input.GetKey(KeyCode.T))
        {
            Damage();
        }
        if (lookAtTarget != null)
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
    }
    public void Damage(float dmg = 1, bool useUI = true)
    {
        health -= dmg;
        if (health < 0) health = 0;
        if (canDestory && health <= 0)
        {
            if (healthDisplay != null) Destroy(healthDisplay);
            Destroy(gameObject);
        }
        UpdateHealth(useUI);
    }
    void UpdateHealth(bool useUI = true)
    {
        if (useUI)
        {
            if (healthDisplay == null) return;
            healthDisplay.value = health;
        }
        else
        {
            if (greenHealth == null || redHealth == null) return;
            Vector3 greenPos = greenHealth.transform.localPosition, greenScale = greenHealth.transform.localScale;
            greenScale.x = health / 100;
            greenPos.x = ((1 - greenScale.x) / 2) * -1;
            greenHealth.transform.localScale = greenScale;
            greenHealth.transform.localPosition = greenPos;
        }
    }
    public void SetHealth(float newHealth)
    {
        health = newHealth;
        if (health < 0) health = 0;
        if (health > maxHealth) health = maxHealth;
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
}
