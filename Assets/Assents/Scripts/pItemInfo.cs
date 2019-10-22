using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class pItemInfo : MonoBehaviour
{
	private GameControler gameControle;

	public int idSlots;
	public GameObject objetoSlot;

	[Header("Configuração hud")]
	public Image imageItem;
	public Text nomeItem;
	public Text danoArma;

	public GameObject[] aprimoramentos;

	[Header("Botões")]
	public Button btnEquipar;
	public Button btnAprimorar;
	public Button btnExcluir;



    // Start is called before the first frame update
    void Start()
    {

		gameControle = FindObjectOfType(typeof(GameControler)) as GameControler;
        
    }

  

	public void CarregarItemInfo()
	{

		item itemInfo = objetoSlot.GetComponent<item>();
		int idArma = itemInfo.id;

		//atualiza o nome da arma no menu do item 
		nomeItem.text = gameControle.nomeArma[idArma];

		//atualiza a imagem do item no menu item
		imageItem.sprite = gameControle.iconesArma[idArma];

		//atualiza a informação sobre o dano da arma no menu item
		string tipoDano = gameControle.tiposDano[gameControle.tipoDanoArma[idArma]];
		int danoMin = gameControle.danoMin[idArma];
		int danoMax = gameControle.danoMax[idArma];
		danoArma.text = "Dano: " + danoMin.ToString() + " - " + danoMax.ToString() + " do tipo " + tipoDano;

		int aprimoramento = gameControle.aprimoramentoArma[idArma];
		
		foreach(GameObject a in aprimoramentos)
		{
			a.SetActive(false);
		}

		for(int i = 0; i <= aprimoramento; i++)
		{
			aprimoramentos[i].SetActive(true);
		}

	}
}
