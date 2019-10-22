using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
	
	private GameControler gameControler;

	[Header("Controle do personagem")]
	private	Animator playerAnimator;// determina a interação entre as animações
	public SpriteRenderer playerSpriteRender;//altera opções de renderização do player
	private Rigidbody2D playerRB;// determina as ações fisicas do personagem
	public BoxCollider2D standing,crouching;//colisor em pé e colisor agachado
	public Transform groudedCheck; // objeto responsavel por detectar se o personagem está pisando no chão
	public LayerMask whatIsGroud;//variavel usada para definir oq é chão
	public bool Grouded; // indica se o personagem está pisando no chão
	public float speed;// velocidade do personagem
	public float jumpForce;// indica força do pulo do personagem
	public bool atacking;// indica se o personagem está atacando
	public int idAnimation; // indica qual ação o personagem deve realizar
	private bool lookLeft;//define o lado que o personagem está olhando
	private float h, v;//variaveis que definem as direções que o personagem se movimenta


	//interação com itens e objetos
	[Header("Controle de Interação")]
	private Vector3 dir = new Vector3(1,0,0);//determina a direção do raycast do personagem
	public LayerMask handTest;//determina com quais objetos a mão do personagem que possui um raycast deve interagir
	public Transform hand;//objeto que representa a mão do personagem
	public GameObject objetoInteracao;//recebe o objeto que o personagem está interagindo
	public GameObject alertaInteracao;

	//sistema de armas
	[Header("Informações de Arma")]
	public GameObject[] armas, arcos, flechaArco, staffs;//recebe os objetos que representam o estado da arma
	public GameObject prefabFlecha, prefabMagia;
	public Transform spawFlecha, spawMagia;
	//public int idArma;//identificador da arma obtida
	//public int idArmaAtual;//identificador da arma sendo usada

	//sistema de vida 
	[Header("Informações de vida")]
	public int vidaMaxima;
	public int vidaAtual;


    // Start is called before the first frame update
    void Start()
    {

		gameControler = FindObjectOfType<GameControler>();//encontra o objeto especificado

		//inicia gameObjects
		playerAnimator = GetComponent<Animator>();
		playerRB = GetComponent<Rigidbody2D>();
		playerSpriteRender = GetComponent<SpriteRenderer>();

		//inicia arma, desativada
		foreach (GameObject o in armas) {
			o.SetActive(false);
			
		}

		foreach (GameObject o in arcos)
		{
			o.SetActive(false);

		}

		foreach (GameObject o in staffs)
		{
			o.SetActive(false);

		}

		//TrocarArma();


		//inicia informações do GameControler ao trocar de cena
		vidaMaxima = gameControler.vidaMaxima;
		vidaAtual = vidaMaxima;
		//idArma = gameControler.idArma;

	}

	//funções relacionadas a física, preferencialmente colocar aqui
	private void FixedUpdate()
	{
		if(gameControler.currentGameState == GameState.PAUSE)
		{
			return;
		}

		Grouded = Physics2D.OverlapCircle(groudedCheck.position,0.01f,whatIsGroud);
		
		// está linha determina a movimentação do personagem
		playerRB.velocity = new Vector2(h * speed, playerRB.velocity.y);
	}


	// Update is called once per frame
	void Update()
    {

		if (gameControler.currentGameState == GameState.PAUSE || gameControler.currentGameState == GameState.ITENS)
		{
			return;
		}

		h = Input.GetAxisRaw("Horizontal");
		 v = Input.GetAxisRaw("Vertical");


		//este bloco de código define o lado para qual o personagem olha
		if (h > 0 && lookLeft == true && !atacking)
		{
			flip();
		}
		else {
			if (h < 0 && lookLeft == false && !atacking) {
				flip();
			}
		}

		//este bloco de código define se o personagem está abaixado ou se movimentando
		if (v < 0 )
		{
			idAnimation = 2;

			if (Grouded == true)
			{
				h = 0;
			}
		
		}
		else {



			if (h != 0)
					{
						idAnimation = 1;
					}
					else {
						idAnimation = 0;
					}
		}

		if (atacking && Grouded) {
			h = 0;
		}


		if (v < 0 && Grouded)
		{

			standing.enabled = false;
			crouching.enabled = true;
		}
		else {
			standing.enabled = true;
			crouching.enabled = false;

		}
		



		//este bloco de código define o ataque do personagem
		if (Input.GetButtonDown("Fire1") && !atacking && objetoInteracao == null)
		{
			playerAnimator.SetTrigger("atack");
		}


		//caso o personagem esteja interagindo com um objeto, envia uma mensagem para esse objeto, uma mensagem significa executar uma função
		if (Input.GetButtonDown("Fire1") && !atacking && objetoInteracao != null)
		{
			objetoInteracao.SendMessage("Interacao",SendMessageOptions.DontRequireReceiver);
		}


		//este bloco de código define o pulo do personagem
		if (Input.GetButtonDown("Jump") && Grouded == true && !atacking)
		{
			playerRB.AddForce(new Vector2(0, jumpForce));
		}

		if(Input.GetKey(KeyCode.Alpha0) && atacking == false)
		{
			gameControler.idArma = 0;
		}

		if (Input.GetKey(KeyCode.Alpha1) && atacking == false)
		{
			gameControler.idArma = 1;
		}

		if (Input.GetKey(KeyCode.Alpha2) && atacking == false)
		{
			gameControler.idArma = 2;
		}

		if (Input.GetKey(KeyCode.Alpha5) && atacking == false)
		{
			gameControler.idArma = 5;
		}


		//Seta valores relacionados a animação do personagem
		playerAnimator.SetBool("grounded", Grouded);
		playerAnimator.SetInteger("IdAnimation", idAnimation);
		playerAnimator.SetFloat("speedY", playerRB.velocity.y);
		playerAnimator.SetFloat("idClasseArma", gameControler.idClasseArma[gameControler.idArmaAtual]);


		interage();
    }

	private void LateUpdate()
	{
		if(gameControler.idArmaAtual != gameControler.idArma)
		{
			gameControler.ValidarArma();
			TrocarArma(gameControler.idArma);
		}
	}

	//função que muda para que lado o personagem está olhando
	void flip() {

		lookLeft = !lookLeft;
		float x = transform.localScale.x;
		x *= -1;
		transform.localScale = new Vector3(x,transform.localScale.y, transform.localScale.z);
		dir.x = dir.x * -1;
	}


	//função que detecta se o personagem está atacando, nesse caso com uma arma da classe 0
	public void atack(int atack) {
		if (atack == 1)
		{
			atacking = true;
			
		}
		else if(atack == 0){
			atacking = false;
			armas[2].SetActive(false);
		}
	
	}

	//função que detecta se o personagem está atacando, nesse caso com uma arma da classe 1
	public void atackArco(int atack)
	{
		if (atack == 1)
		{
			atacking = true;
			
		}
		else if (atack == 0)
		{
			atacking = false;
			arcos[2].SetActive(false);//desativa o último objeto que representa a arma, nesse caso o arco
		}

	}

	//função que detecta se o personagem está atacando, nesse caso com uma arma da classe 2
	public void atackStaff(int atack)
	{
		if (atack == 1)
		{
			atacking = true;
			
		}
		else if (atack == 0)
		{
			atacking = false;
			staffs[3].SetActive(false);//desativa o ultimo objeto que representa a arma, nesse caso o cajado 
		}

	}

	void controleArmas(int id)//função de controle da arma de corpo a corpo, ativa e desativa os gameObjects corretamente de acordo com os sprites que precisam ser mostrados
	{

		foreach (GameObject o in armas)
		{
			o.SetActive(false);

		}

		armas[id].SetActive(true);
	}

	void controleArcos(int id)//função de controle do arco, ativa e desativa os gameObjects corretamente de acordo com os sprites que precisam ser mostrados, também dispara o projétil de flecha
	{

		foreach (GameObject o in arcos)
		{
			o.SetActive(false);

		}

		if(id == 2)
		{
			GameObject tempFlecha = Instantiate(prefabFlecha,spawFlecha.position,spawFlecha.rotation);
			tempFlecha.transform.localScale = new Vector3(tempFlecha.transform.localScale.x * dir.x, tempFlecha.transform.localScale.y, tempFlecha.transform.localScale.z);
			tempFlecha.GetComponent<Rigidbody2D>().velocity = new Vector2(5*dir.x, 0);
			Destroy(tempFlecha, 1.5f);
		}

		arcos[id].SetActive(true);
	}

	void controleStaffs(int id)//função de controle do cajado do mago, ativa e desativa os gameObjects corretamente de acordo com os sprites que precisam ser mostrados, também dispara o projétil de magia
	{

		foreach (GameObject o in staffs)
		{
			o.SetActive(false);

		}

		if(id == 2)
		{
			GameObject tempMagia = Instantiate(prefabMagia, spawMagia.position, spawMagia.rotation);
			tempMagia.GetComponent<Rigidbody2D>().velocity = new Vector2(3 * dir.x, 0);
			Destroy(tempMagia, 1f);
		}

		staffs[id].SetActive(true);
	}



	/*
	//funções para tratar colisores
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "coletavel")
		{

			Destroy(collision.gameObject);
		}
		
	}

		/*
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "caixa")
		{

			print("saiu " + collision.gameObject);
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "caixa")
		{

			print("esta " + collision.gameObject);
		}
	}

	*/
	//funções para tratar triggers 
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "coletavel")
		{

			collision.SendMessage("Interacao", SendMessageOptions.DontRequireReceiver);
		}
	}
	/*
	private void OnTriggerExit2D(Collider2D collision)
	{
		
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		
	}
	*/

	//exemplo de uso do raycast
	//######################################################################################
	private void interage() {

		RaycastHit2D hit = Physics2D.Raycast(hand.position,dir,0.2f,handTest);
		//Debug.DrawRay(hand.position,dir * 0.2f,Color.cyan);

		if (hit == true)
		{
			objetoInteracao = hit.collider.gameObject;
			alertaInteracao.SetActive(true);
		}
		else {
			objetoInteracao = null;
			alertaInteracao.SetActive(false);
		}
	}

	//função responsavel por alterar o tipo de material do personagem, e das armas que ele pode utilizar
	public void AlteraMaterial(Material tipoLuz)
	{
		playerSpriteRender.material = tipoLuz;


		foreach (GameObject o in armas)
		{
			o.GetComponent<SpriteRenderer>().material = tipoLuz;

		}

		foreach (GameObject o in arcos)
		{
			o.GetComponent<SpriteRenderer>().material = tipoLuz;

		}

		foreach (GameObject o in flechaArco)
		{
			o.GetComponent<SpriteRenderer>().material = tipoLuz;

		}

		foreach (GameObject o in staffs)
		{
			o.GetComponent<SpriteRenderer>().material = tipoLuz;

		}

	}

	//função responsável para trocar a arma do personagem e todos os atributos relacionados a arma
	public void TrocarArma(int id)
	{

		switch (gameControler.idClasseArma[id])
		{
			case 0://armas da classe 0, de acordo com o banco de armas no game controler
				armaInfo infoArma;
				armas[0].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas1[id];

				infoArma = armas[0].GetComponent<armaInfo>();
				infoArma.danoMin = gameControler.danoMin[id];
				infoArma.danoMax = gameControler.danoMax[id];
				infoArma.tipoDano = gameControler.tipoDanoArma[id];

				armas[1].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas2[id];

				infoArma = armas[1].GetComponent<armaInfo>();
				infoArma.danoMin = gameControler.danoMin[id];
				infoArma.danoMax = gameControler.danoMax[id];
				infoArma.tipoDano = gameControler.tipoDanoArma[id];

				armas[2].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas3[id];

				infoArma = armas[2].GetComponent<armaInfo>();
				infoArma.danoMin = gameControler.danoMin[id];
				infoArma.danoMax = gameControler.danoMax[id];
				infoArma.tipoDano = gameControler.tipoDanoArma[id];

				break;

			case 1://armas da classe 1, de acordo com o banco de armas no game controler

				arcos[0].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas1[id];
				arcos[1].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas2[id];
				arcos[2].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas3[id];

				break;

			case 2://armas da classe 2, de acordo com o banco de armas no game controler

				staffs[0].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas1[id];
				staffs[1].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas2[id];
				staffs[2].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas3[id];
				staffs[3].GetComponent<SpriteRenderer>().sprite = gameControler.spriteArmas4[id];

				break;

		}


		gameControler.idArmaAtual = gameControler.idArma;
	}


}
