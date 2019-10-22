using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum GameState
{
	PAUSE,
	GAMEPLAY,
	ITENS
}


public class GameControler : MonoBehaviour
{

	public GameState currentGameState;

	public inventario inventario;

	public playerScript playerScript;

	//variáveis usadas para controlar efeitos que aparecem por determinadas ações
	[Header("Controle de efeitos")]
	public GameObject[] fxDano;//lista de objetos que gera um efeito de acordo com o tipo de dano
	public GameObject fxDeath;//lista que guarda efeitos de morte



	//variáveis usadas para controlar o gold do personagem
	[Header("Controle de gold")]
	public int gold;
	public TextMeshProUGUI goldTxt;

	[Header("Paineis")]
	public GameObject painelPause;
	public GameObject painelItens;
	public GameObject painelItensInfo;

	[Header("Primeiro elemento de cada painel")]
	public Button fisrtBtnPause;
	public Button fisrtBtnItens;
	public Button fisrtBtnItensInfo;

	//dados onde é guardado as informações do personagem
	[Header("Informações player")]
	public int idPersonagem;
	public int idPesonagemAtual;
	public int vidaMaxima;
	public int idArma;//identificador da arma obtida
	public int idArmaAtual;//identificador da arma sendo usada

	//informações para manipulação de skins dos personagens
	[Header("Banco de personagens")]
	public string[] nomePersonagem;
	public Texture2D[] spritsheetName;
	public int[] idClasse;
	

	//dados onde são guardados as informações das armas
	//########################################################################################
	[Header("Banco de dados armas")]
	public string[] nomeArma;
	public int[] custoArma;
	public int[] idClasseArma; //0- machado, martelo, espadas 1- arco 2- staffs
	public int[] armaInicial;
	public Sprite[] iconesArma;

	public int[] aprimoramentoArma;

	public Sprite[] spriteArmas1;
	public Sprite[] spriteArmas2;
	public Sprite[] spriteArmas3;
	public Sprite[] spriteArmas4;
	public int[] danoMin;
	public int[] danoMax;
	public int[] tipoDanoArma;
	public string[] tiposDano;
	//########################################################################################

	// Start is called before the first frame update
	void Start()
    {
		inventario = FindObjectOfType<inventario>();
		DontDestroyOnLoad(this.gameObject);
		idPersonagem = PlayerPrefs.GetInt("idPersonagem");
		painelPause.SetActive(false);
		painelItens.SetActive(false);
		painelItensInfo.SetActive(false);
		//painelItens.SetActive(false);
		ValidarArma();

		playerScript = FindObjectOfType(typeof(playerScript)) as playerScript;

    }

	private void Update()
	{
		//define o gold do personagem e mostra no HUD
		string s = gold.ToString("N0");
		goldTxt.text = s.Replace(",",".");

		//caso esc seja pressionado e o estado do game não seja o painel item, o pause é ativado
		if (Input.GetButtonDown("Cancel") && currentGameState != GameState.ITENS)
		{
			pauseGame();
		}

	}

	//função utilizada para validar a arma do personagem, onde alguns personagens usam armas especificas
	public void ValidarArma()
	{
		if(idClasseArma[idArma] != idClasse[idPesonagemAtual])//verifica se o identificar do personagem é compativel com a identificação da classe da arma
		{
			idArma = armaInicial[idPesonagemAtual];//caso a arma seja imcompátivel com o personagem, muda a arma para uma apropriada definida como inicial para a classe do personagem

		}
	}

	//função para pausar o jogo
	public void pauseGame()
	{

		if (!painelPause.activeSelf)//verifica o estado do painel de pause, se estiver ativo então o pause será desativado e vise versa
		{
			Time.timeScale = 0;//para o tempo no jogo
			changeState(GameState.PAUSE);//muda a variável que representa o estado do jogo para pausado
		}
		else
		{
			Time.timeScale = 1;//retorna o tempo do jogo ao normal
			changeState(GameState.GAMEPLAY);//muda a variável relativa ao estado do jogo para jogando
		}

		painelPause.SetActive(!painelPause.activeSelf);//verifica o estado do painel de pause, se estiver ativo, estão desative, e vise versa
		fisrtBtnPause.Select();//seleciona o botão inicial ao ativar o painel de pause
	}

	//função que muda avariável que indica o estado do jogo
	public void changeState(GameState newState)
	{
		currentGameState = newState;
	}

	//função utilizado quando o botão de item no menu pause é pressionado
	public void btnItensDown()
	{
		inventario.CarregarInventario();
		painelPause.SetActive(false);//desativa o painel pause
		painelItens.SetActive(true);//ativa o painel item
		fisrtBtnItens.Select();//selecionao primeiro botão do painel item
		changeState(GameState.ITENS);//muda o estado do jogo para painel item

	}

	//função utilizada para fechar o painel item
	public void btnFecharItens()
	{
		inventario.LimparInventario();
		painelItens.SetActive(false);//desativa o painel item
		painelPause.SetActive(true);//ativa o painel pause
		fisrtBtnPause.Select();//seleciona o primeiro botão do painel pause
		changeState(GameState.PAUSE);//muda a vairável que representa o estado do jogo para indicar que esta no painel pause
	}

	public void OpenItemInfo()
	{
		painelItensInfo.SetActive(true);
		fisrtBtnItensInfo.Select();//seleciona o primeiro botão do panel ItensInfo
	}

	public void fecharItensInfo()
	{
		painelItensInfo.SetActive(false);
		fisrtBtnItens.Select();
	}


	public void UsarItemArma(int idArma)
	{
		playerScript.TrocarArma(idArma);
	}

}
