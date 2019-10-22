using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class trocarScena : MonoBehaviour
{
	private GameControler gameControler;
	public string destino;
	private fade fade;

    // Start is called before the first frame update
    void Start()
    {
		gameControler = FindObjectOfType<GameControler>();
		fade = FindObjectOfType<fade>();
    }


	//faz a troca da scena atual para a scena de destino
	public void Interacao()
	{
		StartCoroutine("trocaScena");

	}


	IEnumerator trocaScena()
	{
		fade.fadeIn();
		yield return new WaitWhile(() => fade.fume.color.a < 1f);

		if(destino == "titulo")
		{
			Destroy(gameControler.gameObject);
		}

		SceneManager.LoadScene(destino);

		
		fade.fadeOut();


	}
}
