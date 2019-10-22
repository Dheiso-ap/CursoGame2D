using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventario : MonoBehaviour
{
	public GameControler gameControler;

	public Button[] slots;//lista de botões do menu item
	public Image[] iconItens;//icones dos botões 

	public int qtdVida, qtdMana, qtdFlecha1, qtdFlecha2, qtdFlecha3;//valores relativos as quantidade de pote de vida e mana e também as flechas
	public Text vida, mana, flecha1, flecha2, flecha3;// texto a ser exibido no menu de itens, relativos a potes de vida e mana, e também as flechas

	public List<GameObject> itensSlot;//lista de itens presentes no menu
	public List<GameObject> itensCarregas;//lista de itens carregados

    // Start is called before the first frame update
    void Start()
    {
		gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;

	}


    // Update is called once per frame
    void Update()
    {
        
    }

	public void CarregarInventario()
	{
		foreach (GameObject c in itensCarregas)
		{
			Destroy(c);

		}

		itensCarregas.Clear();

		foreach (Button b in slots)//inicia os slots com interação falsa, quando não há itens não são utilizados os slots
		{
			b.interactable = false;
		}

		foreach (Image i in iconItens)//inicia os icones dos slots com a imagem desativada, se não há item não deve exibir imagem
		{
			i.sprite = null;
			i.gameObject.SetActive(false);
		}


		//inicia os textos dos potes de vida mana e também as flechas como 0
		vida.text = "x 0";
		mana.text = "x 0";
		flecha1.text = "x 0";
		flecha2.text = "x 0";
		flecha3.text = "x 0";

		int j = 0;//interador para atualizar a posição do vetor slots dentro do próximo foreach

		foreach (GameObject g in itensSlot)
		{
			GameObject temp = Instantiate(g);
			itensCarregas.Add(temp);
			slots[j].GetComponent<slotScript>().objetoSlot = temp;
			slots[j].interactable = true;

			item itemInfo = temp.GetComponent<item>();
			iconItens[j].sprite = gameControler.iconesArma[itemInfo.id];
			iconItens[j].gameObject.SetActive(true);

			j++;
		}

	}

	public void LimparInventario()
	{
		foreach (GameObject c in itensCarregas)
		{
			Destroy(c);

		}

		itensCarregas.Clear();

	}
}
