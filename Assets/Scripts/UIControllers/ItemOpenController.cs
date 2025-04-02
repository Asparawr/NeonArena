using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemOpenController : MonoBehaviour
{
    public GameObject openedItem;
    public GameObject lockObject;
    public GameObject dustObject;
    public ParticleSystem OpenParticle1;
    public ParticleSystem OpenParticle2;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI dustAmount;
    public Image dustIcon;
    public Image itemIcon;
    private Color color;
    public float timerLockMax;
    private float timerLock;
    public float timerItemMax;
    private float timerItem;
    public float timerDustMax;
    private float timerDust;

    public float alphaLock = 0.6f;
    private float alphaItem;
    private float alphaDust = 1;
    private bool startDustTimer = false;
    public bool skipAnim = false;
    private void FixedUpdate()
    {
        if (timerLock > 0)
        {
            timerLock -= Time.deltaTime;
            Color colorLock = lockObject.GetComponent<Image>().color;
            colorLock.a = timerLock / timerLockMax * alphaLock;
            lockObject.GetComponent<Image>().color = colorLock;
        }
        if (timerItem > 0)
        {
            timerItem -= Time.deltaTime;
            color.a = alphaItem - timerItem / timerItemMax * alphaItem;
            itemDescription.color = color;
            itemIcon.color = color;
        }
        if (startDustTimer && timerLock <= 0 && timerItem <= 0)
        {
            timerDust = timerDustMax;
            Color colorDust = dustAmount.color;
            colorDust.a = 0;
            dustAmount.color = colorDust;
            colorDust = dustAmount.color;
            colorDust.a = 0;
            dustAmount.color = colorDust;
            dustObject.SetActive(true);
            startDustTimer = false;
        }
        if (timerDust > 0)
        {
            timerDust -= Time.deltaTime;
            color.a = timerDust / timerDustMax * alphaItem;
            itemDescription.color = color;
            itemIcon.color = color;

            Color colorDust = dustAmount.color;
            colorDust.a = alphaDust - timerDust / timerDustMax * alphaDust;
            dustAmount.color = colorDust;
            colorDust = dustIcon.color;
            colorDust.a = alphaDust - timerDust / timerDustMax * alphaDust;
            dustIcon.color = colorDust;
        }
    }
    public void Reset()
    {
        lockObject.SetActive(true);
        openedItem.SetActive(false);
        dustObject.SetActive(false);
        Color colorReset = lockObject.GetComponent<Image>().color;
        colorReset.a = alphaLock;
        lockObject.GetComponent<Image>().color = colorReset;
        timerLock = 0;
        timerItem = 0;
        timerDust = 0;
        startDustTimer = false;
    }
    public void Open(ShopItem item, ChestsController chestsController, bool skipAnim, int dust = 0)
    {
        this.skipAnim = skipAnim;
        lockObject.SetActive(false);
        openedItem.SetActive(true);
        dustObject.SetActive(false);
        color = chestsController.menuController.helper.colors[Enum.GetName(typeof(Rarity), item.rarity)];

        if (!skipAnim)
        {
            //particles
            var main = OpenParticle1.GetComponent<ParticleSystem>().main;
            main.startColor = color;
            main = OpenParticle2.GetComponent<ParticleSystem>().main;
            main.startColor = color;
            OpenParticle1.Play();
            OpenParticle2.Play();
        }

        //desc
        itemIcon.sprite = item.iconSprite;
        itemDescription.text = item.GetEffects();
        itemDescription.color = color;
        itemIcon.color = color;
        alphaItem = color.a;
        if (!skipAnim)
            color.a = 0;
        itemDescription.color = color;
        itemIcon.color = color;

        //timers
        if (!skipAnim)
        {
            timerLock = timerLockMax;
            timerItem = timerItemMax;
        }
        else
        {
            timerLock = 0;
            timerItem = 0;
        }
        if (dust > 0)
        {
            dustAmount.text = dust.ToString();
            startDustTimer = true;
        }
        else
            startDustTimer = false;
        timerDust = 0;
    }
}
