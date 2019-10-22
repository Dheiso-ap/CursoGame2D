using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
	public GameControler gameControler;
	public int id;//identificador do item


    // Start is called before the first frame update
    void Start()
    {
		gameControler = FindObjectOfType(typeof(GameControler)) as GameControler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void UsarItem()
	{
		gameControler.OpenItemInfo();
		//gameControler.UsarItemArma(id);
	}
}
