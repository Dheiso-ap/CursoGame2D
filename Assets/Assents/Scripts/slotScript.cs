using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slotScript : MonoBehaviour
{

	private GameControler gameControle;
	private pItemInfo pItemInfo;

	public int idslot;

	public GameObject objetoSlot;

    // Start is called before the first frame update
    void Start()
    {

		gameControle = FindObjectOfType(typeof(GameControler)) as GameControler;
		pItemInfo = FindObjectOfType(typeof(pItemInfo)) as pItemInfo;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//função chamada quando se clica em um item do inventário
	public void UsarItem()
	{
		if (objetoSlot != null)
		{

			//objetoSlot.SendMessage("UsarItem", SendMessageOptions.DontRequireReceiver);
			pItemInfo.objetoSlot = objetoSlot;
			pItemInfo.idSlots = idslot;
			pItemInfo.CarregarItemInfo();
			gameControle.OpenItemInfo();
		}
	}
}
