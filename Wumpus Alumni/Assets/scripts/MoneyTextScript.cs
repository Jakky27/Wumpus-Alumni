using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTextScript : MonoBehaviour
{
    private Text text;
    public PlayerController player;

	void Start ()
    {
        player = transform.root.GetComponent<PlayerController>();
        text = gameObject.GetComponent<Text>();
	}
	
	void Update ()
    {
        text.text = "$" + player.money.ToString();
		
	}
}
