using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    GameStateManager GM;
    public GameStates gs;


    // Start is called before the first frame update
    void Start()
    {
        GM = GameStateManager.Instance;
		GM.OnStateChange += StateChangeHandler;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown(){
        Debug.Log("Clicked on: " +  this.gameObject.name);
        this.gameObject.GetComponent<Renderer>().material.color = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );
        GM.SetGameState(gs);
    }

    public void StateChangeHandler () {
        Debug.Log("Changing state: " + GM.gameState);
    }
}
