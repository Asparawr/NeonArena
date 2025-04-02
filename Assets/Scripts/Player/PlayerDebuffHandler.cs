using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuffHandler : MonoBehaviour
{
    PlayerController playerController;
    float slowTimer;

    float burnTimer;
    float burnTickTimer;
    float burnTickMax = 0.4f;
    float burnDamage = 0;

    float poisonTimer;
    float poisonTimerMax;
    float poisonTickTimer;
    float poisonTickMax = 0.8f;
    float poisonDamage = 0;

    public GameObject debuffParticles;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (slowTimer < 0)
        {
            playerController.slowSpeedMod = 1;
            DisableParticle("Slow");
        }
        else
            slowTimer -= Time.deltaTime;

        if (burnTimer < 0)
        {
            DisableParticle("Burn");
        }
        else
        {
            burnTimer -= Time.deltaTime;
            if (burnTickTimer < 0)
            {
                burnTickTimer = burnTickMax;
                playerController.UpdateHealth(-burnDamage);
                var popup = Instantiate(playerController.damagePopupPrefab, transform.position, Quaternion.identity);
                popup.GetComponent<PopupController>().SetText(Mathf.Ceil(burnDamage).ToString());
            }
            else
                burnTickTimer -= Time.deltaTime;
        }
        if (poisonTimer < 0)
        {
            DisableParticle("Poison");
        }
        else
        {
            poisonTimer -= Time.deltaTime;
            if (poisonTickTimer < 0)
            {
                poisonTickTimer = poisonTickMax;
                playerController.UpdateHealth(-poisonDamage);
                var popup = Instantiate(playerController.damagePopupPrefab, transform.position, Quaternion.identity);
                popup.GetComponent<PopupController>().SetText(Mathf.Ceil(poisonDamage).ToString());
            }
            else
                poisonTickTimer -= Time.deltaTime;
        }
    }
    public void ResetDebuffTimers()
    {
        slowTimer = 0;
        burnTimer = 0;
        poisonTimer = 0;
    }
    public void SetSlow(float slow, float slowDuration)
    {
        if (slowTimer < 0)
        {
            SetParticle("Slow");
        }
        playerController.slowSpeedMod = slow;
        slowTimer = slowDuration * playerController.playerStats.slowDebuff;
    }
    public void SetBurn(float burnDamage, float burnTimerMax, float burnTickMax)
    {
        SetParticle("Burn");
        this.burnDamage = burnDamage;
        this.burnTickMax = burnTickMax;
        burnTimer = burnTimerMax * playerController.playerStats.burnDebuff;
    }
    public void SetPoison(float poisonDamage, float poisonTimerMax, float poisonTickMax)
    {
        SetParticle("Poison");
        this.poisonDamage = poisonDamage;
        this.poisonTickMax = poisonTickMax;
        poisonTimer = poisonTimerMax * playerController.playerStats.poisonDebuff;
    }

    public void SetParticle(string name)
    {
        debuffParticles.GetComponent<DebuffParticles>().SetParticle(name);
    }

    public void DisableParticle(string name)
    {
        debuffParticles.GetComponent<DebuffParticles>().DisableParticle(name);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyAura"))
        {
            if (other.gameObject.tag == "SlowAura")
            {
                var slowAura = other.gameObject.GetComponent<SlowAura>();
                SetSlow(slowAura.slow, slowAura.slowDuration);
            }
            if (other.gameObject.tag == "BurnAura")
            {
                var burnAura = other.gameObject.GetComponent<BurnAura>();
                SetBurn(burnAura.burnDamage, burnAura.burnDuration, burnAura.burnTickRate);
            }
            if (other.gameObject.tag == "PoisonAura")
            {
                var poisonAura = other.gameObject.GetComponent<PoisonAura>();
                SetPoison(poisonAura.poisonDamage, poisonAura.poisonDuration, poisonAura.poisonTickRate);
            }
        }
    }
}
