using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun
{

    void Start()
    {
        isAuto = true;

        damage = 34f;
        knockback = 20f;
        range = 100f;

        maxAmmo = 120;
        ammoLeft = 120;

        clipSize = 30;
        roundsLeft = 30;
        fireRate = 500;

        reloadTime = 3f;
        switchTime = 1f;
        equipTime = 1f;

        maxRecoil = 2.7f;
        minRecoil = 1.3f;

        recoil.x = 1f; //vert
        recoil.y = 0.8f; //horz

        zoomFOV = 50f;
        unZoomFOV = 80f;

        convertedFireRate = 1f / (fireRate / 60f);
    }
}
