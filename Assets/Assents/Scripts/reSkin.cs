using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class reSkin : MonoBehaviour
{
	private GameControler gameControler;
	private SpriteRenderer sRenderer;

	public Sprite[] sprites;//sprites em uso
	public string spriteSheetName;//nome do sprite obtido
	public string loadedSpritSheetName;//nome do sprite atual

	private Dictionary<string, Sprite> spriteSheet;//sprites disponiveis 

	public bool isPlayer;//para verificar se o script pertence ao personagem para carregar os sprites apenas no personagem

	

    // Start is called before the first frame update
    void Start()
    {
		gameControler = FindObjectOfType<GameControler>();
		sRenderer = GetComponent<SpriteRenderer>();

		if (isPlayer)
		{
			spriteSheetName = gameControler.spritsheetName[gameControler.idPersonagem].name;//obtem o nome dos sprites corretos do personagem ao trocar de cena
		}

	}

	// Update is called once per frame
	void LateUpdate()
    {
		if (isPlayer)
		{
			if(gameControler.idPesonagemAtual != gameControler.idPersonagem)
			{
				spriteSheetName = gameControler.spritsheetName[gameControler.idPersonagem].name;//obtem o nome dos sprites corretos do personagem ao trocar de cena
				gameControler.idPesonagemAtual = gameControler.idPersonagem;
				gameControler.ValidarArma();
			}
		}

        if(loadedSpritSheetName != spriteSheetName)
		{
			LoadSpriteSheet();
			
		}

		sRenderer.sprite = spriteSheet[sRenderer.sprite.name];
    }

	//carrega o sprite de acordo com o nome do spriteSeet informado
	private void LoadSpriteSheet()
	{

		sprites = Resources.LoadAll<Sprite>(spriteSheetName);
		spriteSheet = sprites.ToDictionary(x => x.name, x => x);
		loadedSpritSheetName = spriteSheetName;
	}


}
