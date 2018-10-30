using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoTextScript : MonoBehaviour
{
    public PlayerController player;
    private Text ammoText;
    public Gun gun;

    public HPSliderScript slider;

    void Start ()
    {
        player = transform.root.GetComponent<PlayerController>();
        gun = transform.root.GetComponent<GunController>().equippedGun;
        ammoText = gameObject.GetComponent<Text>();
    }

    void Update ()
    {
        gun = transform.root.GetComponent<GunController>().equippedGun;

        ammoText.text = (gun.roundsLeft + " | " + gun.ammoLeft);
	}
}
