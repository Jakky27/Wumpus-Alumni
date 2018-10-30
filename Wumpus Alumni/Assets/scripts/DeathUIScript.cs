using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUIScript : MonoBehaviour
{
    private bool isDead = false;

    public PlayerController player;
    private Animator anim;

    private Text deathText;
    private Text promptText;

	void Start ()
    {
        deathText = GetComponentsInChildren<Text>()[0];
        promptText = GetComponentsInChildren<Text>()[1];

        anim = gameObject.GetComponent<Animator>();
        player = transform.root.GetComponent<PlayerController>();
    }

    void Update ()
    {
		if(!isDead && player.hp<=0)
        {
            StartCoroutine(playDeathScreen());
        }
    }

    IEnumerator playDeathScreen()
    {
        isDead = true;
        anim.SetTrigger("death");
        deathText.text = ("YOU ARE DEAD\n\nYOU SURVIVED " + GameController.waveNum + " ROUNDS");
        yield return new WaitForSeconds(1.5f);

        anim.SetBool("isPrompting", true);
        yield return new WaitForSeconds(2f);
    }
}
