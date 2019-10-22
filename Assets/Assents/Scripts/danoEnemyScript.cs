using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class danoEnemyScript : MonoBehaviour
{

	private GameControler gameControler;
	private Rigidbody2D enemyRB;
	private SpriteRenderer sRender;
	private Animator animator;


	[Header("configuração de vida")]
	public int vidaMaxima;
	public int vidaAtual;
	public GameObject barrasHp;
	public Transform hpAtual;
	public Color[] corPersonagem;
	private float percVida;
	private bool died;

	[Header("configuração de dano")]
	public float[] ajusteDano; //guarda as informações de resistencia ou fraqueza do personagem
	private bool getHit; // indica se o personagem tomou um hit
	public GameObject danoTXT;

	[Header("Configuração de chão")]
	public Transform groudcheck;
	public LayerMask whatIsGroud;
	public bool grouded;

	[Header("Configuração de loot")]
	public GameObject loots;



	/*knockback, metodo da aula
	public GameObject knocBackPrefab;// força de knockback
	public Transform knocBackPosition;// posição onde ocorre o knocback
	*/

	// Start is called before the first frame update
	void Start()
	{
		enemyRB = GetComponent<Rigidbody2D>();
		sRender = GetComponent<SpriteRenderer>();
		gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
		animator = GetComponent<Animator>();

		sRender.color = corPersonagem[0];

		vidaAtual = vidaMaxima;
		barrasHp.SetActive(false);


	}

	private void Update()
	{
		animator.SetBool("grounded",true);
	}


	private void FixedUpdate()
	{
		grouded = Physics2D.OverlapCircle(groudcheck.position, 0.01f, whatIsGroud);
	}


	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!died)//se não estiver morto, sofre hit
		{

			if (collision.gameObject.tag == "arma")//se for colidido por uma arma
			{

				if (getHit == false)//se não estiver invulnerável
				{
					animator.SetTrigger("hit");
					getHit = true;

					barrasHp.SetActive(true);

					armaInfo infoArma = collision.gameObject.GetComponent<armaInfo>();//obtem o script de informação da arma
					Transform PosicaoArma = collision.GetComponent<Transform>();//obtem a posição do colisor da arma
					int dano = Random.Range(infoArma.danoMin, infoArma.danoMax);

					AplicarDano(dano, infoArma.tipoDano);

					//GameObject fxDanoTemp = Instantiate(gameControler.fxDano[infoArma.tipoDano], PosicaoArma.position, PosicaoArma.rotation);
					//Destroy(fxDanoTemp, 1f);

					//print("Tomei " + infoArma.qtdDano + " de dano do tipo " + gameControler.tipoDano[infoArma.tipoDano]);

					//solução da aula para knocback, achei desnecessario
					//GameObject knockTemp = Instantiate(knocBackPrefab,knocBackPosition.position,knocBackPosition.rotation);
					//Destroy(knockTemp,0.03f);


					//determina a direção do knocback e para onde o inimigo olha quando sofre dano
					if (collision.gameObject.transform.position.x < transform.position.x)//verifica de que lado veio o golpe
					{
						if (transform.localScale.x > 0)//verifica se o inimigo já está virado para o personagem que desferiu o golpe
						{
							float x = transform.localScale.x;
							x *= -1;
							transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
							barrasHp.transform.localScale = new Vector3(x, barrasHp.transform.localScale.y, barrasHp.transform.localScale.z);
						}

						enemyRB.AddForce(new Vector2(50, 50));
					}
					else
					{
						if (transform.localScale.x < 0)
						{
							float x = transform.localScale.x;
							x *= -1;
							transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
							barrasHp.transform.localScale = new Vector3(x, barrasHp.transform.localScale.y, barrasHp.transform.localScale.z);

						}
						enemyRB.AddForce(new Vector2(-50, 50));

					}
					StartCoroutine(Invulneravel());
				}
			}
		}
	}

	//função que calcula o dano sofrido e atualiza a vida atual, também decrementa a barra de vida do inimigo
	private void AplicarDano(int qtdDano, int tipoDano) {

		float dano = qtdDano - (qtdDano * (ajusteDano[tipoDano] / 100));//ajusta o dano de acordo com fraqueza ou resistencia
		vidaAtual -= Mathf.RoundToInt(dano);

		percVida = (float)vidaAtual / (float)vidaMaxima;

		if (percVida < 0) { percVida = 0; }

		hpAtual.localScale = new Vector3(percVida,1,1);

		//instâcia um gameObject que representa a animação de dano ao atingir o inimigo
		//GameObject fxDanoTemp = Instantiate(gameControler.fxDano[tipoDano],transform.position,transform.rotation);
		//Destroy(fxDanoTemp,1f);

		//Instancia o game objeto que representa o valor relativo ao dano tomado
		GameObject tempDanoTxt = Instantiate(danoTXT,transform.localPosition,transform.localRotation);
		tempDanoTxt.GetComponent<MeshRenderer>().sortingLayerName = "HUD";
		tempDanoTxt.GetComponent<TextMesh>().text = Mathf.RoundToInt(dano).ToString();
		tempDanoTxt.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,250));
		Destroy(tempDanoTxt, 0.7f);

		if(vidaAtual <= 0)
		{
			this.gameObject.layer = 16;
			
			animator.SetInteger("IdAnimation",3);
			died = true;
			StartCoroutine("Loot");
		}

	}

	//Co-rotina que gera o loot do inimigo
	IEnumerator Loot()
	{
		yield return new WaitForSeconds(1f);
		GameObject fxMorte = Instantiate(gameControler.fxDeath, groudcheck.position, transform.localRotation);
		yield return new WaitForSeconds(0.1f);		
		sRender.enabled = false;

		int qtdLoot = Random.Range(1,10);
		for (int i = 0; i < qtdLoot; i++)
		{
			GameObject loot = Instantiate(loots, transform.position, transform.localRotation);
			loot.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-30, 30), 100));
			yield return new WaitForSeconds(0.1f);

		}

		yield return new WaitForSeconds(0.7f);
		Destroy(fxMorte);
		Destroy(this.gameObject, 1f);

	}

	//co-rotina que deixa o inimigo invulneravel por alguns segundos após sofrer um hit
	IEnumerator Invulneravel() {

		sRender.color = corPersonagem[1];
		yield return new WaitForSeconds(0.2f);
		sRender.color = corPersonagem[0];
		yield return new WaitForSeconds(0.2f);
		sRender.color = corPersonagem[1];
		yield return new WaitForSeconds(0.2f);
		sRender.color = corPersonagem[0];
		yield return new WaitForSeconds(0.2f);
		getHit = false;
		barrasHp.SetActive(false);
	}




}
