using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade : MonoBehaviour
{

	public GameObject painelFume;//gameObject que representa um painel negro utilizado para realizar um efeit de fade
	public Image fume;//imagem negra 
	public Color[] corTransicao;//usa duas cores, um preto normal e um preto sem alpha para realizr o efeito fade
	public float step;//valor utilizado para determinar o tempo de transição entra as duas cores da imagem
	private bool transicao;

    // Start is called before the first frame update
    void Start()
    {

		StartCoroutine("fadeO");
        
    }

	//efeito de fade in
	public void fadeIn()
	{
		if (!transicao)
		{
			transicao = true;
			painelFume.SetActive(true);
			StartCoroutine("fadeI");
		}
	}

	//efeito de fade out
	public void fadeOut()
	{
		StartCoroutine("fadeO");
	}


	IEnumerator fadeI()
	{
		/*for (float i = 0; i < 1f; i += step)
		{
			
			
			fume.color = Color.Lerp(corTransicao[1], corTransicao[0], i);
			yield return new WaitForEndOfFrame();
			
		}*/
		float i = 0;
		while(i < 1f)
		{
			i += step;
			if (i > 1f)
			{
				i = 1f;
			}
			fume.color = Color.Lerp(corTransicao[0], corTransicao[1], i);
			yield return new WaitForEndOfFrame();

		}

		
	}

	IEnumerator fadeO()
	{
		/*for (float i = 0; i <= 1; i += step)
		{

			fume.color = Color.Lerp(corTransicao[0], corTransicao[1], i);
			yield return new WaitForEndOfFrame();
		}*/
		yield return new WaitForSeconds(0.5f);
		float i = 0;
		while (i < 1f)
		{
			i += step;
			if (i > 1f)
			{
				i = 1f;
			}
			fume.color = Color.Lerp(corTransicao[1], corTransicao[0], i);
			yield return new WaitForEndOfFrame();
			
		}
		transicao = false;

	}

}
