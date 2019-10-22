using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class porta : MonoBehaviour
{
	public fade fade;//objeto utilizado para realizar efeito de fade
	public Transform destino;//para onde o player deve ir
	private playerScript scriptPlayer;

	public bool luz;//se o local de destino é sensivel a luz ou não
	public Material luzPadrao, LuzDiffusa;//material sensivel normal e material sensivel a luz

    // Start is called before the first frame update
    void Start()
    {
		scriptPlayer = FindObjectOfType<playerScript>();
		fade = FindObjectOfType(typeof(fade)) as fade;
        
    }


	//quando entrar em um porta o efeito de fade in e chamado, realiza a locomoção do personagem e realiza fade out
	private void Interacao()
	{
		StartCoroutine("acionarPorta");

	}

	IEnumerator acionarPorta()
	{
		fade.fadeIn();
		yield return new WaitWhile(() => fade.fume.color.a < 1f);

		if (luz)
		{
			scriptPlayer.AlteraMaterial(luzPadrao);
		}
		else
		{
			scriptPlayer.AlteraMaterial(LuzDiffusa);

		}

		scriptPlayer.transform.position = destino.position;
		
		fade.fadeOut();

	}

}
