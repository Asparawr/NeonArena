using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffParticles : MonoBehaviour
{
    public GameObject slowParticleObject;
    public GameObject burnParticleObject;
    public GameObject poisonParticleObject;
    public GameObject stunParticleObject;

    GameObject slowParticle;
    GameObject burnParticle;
    GameObject poisonParticle;
    GameObject stunParticle;
    public void SetParticle(string particleName)
    {
        switch (particleName)
        {
            case "Slow":
                if (slowParticle == null)
                {
                    slowParticle = Instantiate(slowParticleObject, transform);
                    slowParticle.transform.localScale = transform.localScale;
                }
                else slowParticle.SetActive(true);
                break;
            case "Burn":
                if (burnParticle == null)
                {
                    burnParticle = Instantiate(burnParticleObject, transform);
                    burnParticle.transform.localScale = transform.localScale;
                }
                else burnParticle.SetActive(true);
                break;
            case "Poison":
                if (poisonParticle == null)
                {
                    poisonParticle = Instantiate(poisonParticleObject, transform);
                    poisonParticle.transform.localScale = transform.localScale;
                }
                else poisonParticle.SetActive(true);
                break;
            case "Stun":
                if (stunParticle == null)
                {
                    stunParticle = Instantiate(stunParticleObject, transform);
                    stunParticle.transform.localScale = transform.localScale;
                }
                else stunParticle.SetActive(true);
                break;
        }
    }
    public void DisableParticle(string particleName)
    {
        switch (particleName)
        {
            case "Slow":
                if (slowParticle != null)
                {
                    slowParticle.SetActive(false);
                }
                break;
            case "Burn":
                if (burnParticle != null)
                {
                    burnParticle.SetActive(false);
                }
                break;
            case "Poison":
                if (poisonParticle != null)
                {
                    poisonParticle.SetActive(false);
                }
                break;
            case "Stun":
                if (stunParticle != null)
                {
                    stunParticle.SetActive(false);
                }
                break;
        }
    }
}
