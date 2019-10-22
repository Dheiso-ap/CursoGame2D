using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
	private GameControler gameControler;
	public int valor;


	private void Start()
	{
		gameControler = FindObjectOfType<GameControler>();
	}

	private void Interacao()
	{
		gameControler.gold += valor;
		Destroy(this.gameObject);

	}

}
