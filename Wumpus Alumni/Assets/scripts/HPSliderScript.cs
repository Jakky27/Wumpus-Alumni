using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSliderScript : MonoBehaviour
{
    private Slider hpSlider;

    public PlayerController player;

	void Start()
    {
        player = transform.root.GetComponent<PlayerController>();
        hpSlider = gameObject.GetComponent<Slider>();
    }
	
	void Update ()
    {
        if(player.hp > 0)
        {
            hpSlider.value = (player.hp / player.maxHp) * 100f;

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
