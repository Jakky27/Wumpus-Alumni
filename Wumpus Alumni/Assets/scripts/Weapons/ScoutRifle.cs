using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutRifle : Gun
{
    void Start()
    {
        isAuto = false;

        damage = 51f;
        knockback = 50f;
        range = 100f;

        maxAmmo = 60;
        ammoLeft = 60;

        clipSize = 10;
        roundsLeft = 10;
        fireRate = 90f;

        reloadTime = 3f;
        switchTime = 1f;
        equipTime = 1f;

        maxRecoil = 4f;
        minRecoil = 3f;

        recoil.x = 3f; //vert
        recoil.y = 0.5f; //horz

        zoomFOV = 50f;
        unZoomFOV = 80f;

        convertedFireRate = 1f / (fireRate / 60f);
    }

}
