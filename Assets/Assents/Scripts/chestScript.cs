using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestScript : MonoBehaviour
{
	private GameControler gameControler;
	private SpriteRenderer spriteRenderer;
	public Sprite[] imagens;// variável que guarda os sprites do baú fechado e aberto
	private bool teste; //variavel usada para saber se o baú está aberto ou fechado
	public GameObject loots;//loot do baú
	//private bool gerouLoot = true;// para verificar se o loot foi gerado uma vez

    // Start is called before the first frame update
    void Start()
    {
		gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;

		spriteRenderer = GetComponent<SpriteRenderer>();


    }


	private void Interacao()
	{

		spriteRenderer.sprite = imagens[1];
			
		StartCoroutine("Loot");

		this.gameObject.GetComponent<Collider2D>().enabled = false;


	}

	/*private void Interacao() {

		teste = !teste;

		if (teste)//se estiver fechado, abre.
		{
			spriteRenderer.sprite = imagens[1];
			gameControler.teste += 1;

			if (gerouLoot)//se estiver aberto, fecha.
			{
				StartCoroutine("Loot");
			}


		}
		else {
			spriteRenderer.sprite = imagens[0];
		}

	}*/

	//co-rotina que gera o loot
	IEnumerator Loot()
	{
		//gerouLoot = false;
		int qtdLoot = Random.Range(1, 10);
		for (int i = 0; i < qtdLoot; i++)
		{
			GameObject loot = Instantiate(loots, transform.position, transform.localRotation);
			loot.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-30, 30), 100));
			yield return new WaitForSeconds(0.1f);

		}

	}

	
}
