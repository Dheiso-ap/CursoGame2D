using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyProject : MonoBehaviour
{
	public LayerMask colisionEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

		//Cria um raycast responsável por identificar se o projétil atingil um inimigo
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.01f, colisionEnemy);

		if(hit == true)
		{
			Destroy(this.gameObject);
		}
        
    }
}
