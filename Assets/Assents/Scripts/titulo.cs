using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titulo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SelecionarPersonagem(int idPersonagem)
	{

		PlayerPrefs.SetInt("idPersonagem", idPersonagem);
		SceneManager.LoadScene("scena1");

	}
}
