﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CKortinToiminta : MonoBehaviour
{

    public CatalystKortti card;
    public Text hinta;
    public Text nameText;
    public Text kuvaus;
    public Image artwork;
    public Text korttinumero;

    public Monster monster1;

    public int addhealth;
    public int addattack;
    public int adddefence;
    public float addmaxhealth;

    public float oldhealth;
    public float healthscale;

    // Use this for initialization
    void Start()
    {
        hinta.text = card.cost;
        nameText.text = card.name;
        kuvaus.text = card.description;
        artwork.sprite = card.kuva;
        korttinumero.text = card.cardnumber;

        addhealth = card.addhealth;
        addattack = card.addattack;
        adddefence = card.adddefence;
        addmaxhealth = card.addmaxhealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        monster1.attack += addattack;
        monster1.defence += adddefence;
        monster1.health += addhealth;

        if (addmaxhealth > 0)
            StartCoroutine(OldHealth());

        //Destroy(this.gameObject, 5);
    }

    IEnumerator OldHealth()
    {
        oldhealth = monster1.maxhealth;
        yield return new WaitForSeconds(0.0001F);
        monster1.maxhealth += addmaxhealth;
        yield return new WaitForSeconds(0.0001F);
        healthscale = monster1.maxhealth / oldhealth;
        monster1.health = monster1.health * healthscale;
    }
}
