using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooterGun : Gun
{
    void Start()
    {
        isAuto = true;

        damage = 20f;
        knockback = 10f;
        range = 50f;

        maxAmmo = 300;
        ammoLeft = 300;

        clipSize = 50;
        roundsLeft = 50;

        fireRate = 800f;

        reloadTime = 2.334f;
        switchTime = .5f;
        equipTime = .5f;

        maxRecoil = 4f;
        minRecoil = 3f;

        recoil.x = 1f;
        recoil.y = 1.5f;

        zoomFOV = 60f;
        unZoomFOV = 80f;

        convertedFireRate = 1f / (fireRate / 60f);

    }

}
