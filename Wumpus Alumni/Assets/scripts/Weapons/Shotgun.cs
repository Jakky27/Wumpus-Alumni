using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
	void Start ()
    {
        isAuto = false;
        isShotugn = true;

        damage = 20f;
        knockback = 10f;
        range = 100f;

        maxAmmo = 60;
        ammoLeft = 60; 

        clipSize = 6;
        roundsLeft = 6;
        fireRate = 39f; // Trial and error to find right number for animation

        reloadTime = .5f; // Irrelavent I believe - JM
        switchTime = .84f;
        equipTime = 1f;

        maxRecoil = 5f;
        minRecoil = 3f;

        recoil.x = 7f;
        recoil.y = 7f;

        zoomFOV = 50f;
        unZoomFOV = 80f;

        convertedFireRate = 1f / (fireRate / 60f);
    }
}
