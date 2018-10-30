using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    void Start()
    {
        isAuto = false;

        damage = 34f;
        knockback = 10f;
        range = 100f;

        maxAmmo = 90;
        ammoLeft = 90;

        clipSize = 18;
        roundsLeft = 18;
        fireRate = 200f;

        reloadTime = 1.84f;
        switchTime = .25f;
        equipTime = .25f;

        maxRecoil = 3f;
        minRecoil = 1.5f;

        recoil.x = 1.5f;
        recoil.y = 0.2f;

        zoomFOV = 50f;
        unZoomFOV = 80f;

        convertedFireRate = 1f / (fireRate / 60f);
    }
}
