using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {
    private UILabel label;
    bool gameover;
    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
    }
	void Start () {
        NewGame();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameover)
        {
            if (Input.GetMouseButton(0))
                NewGame();
        }
	}
    public void NewGame()
    {
        gameover = false;
        label.text = "";
    }
    public void PrintLoose()
    {
        gameover = true;
        label.text = "Defeat";
    }
    public void PrintWin()
    {
        gameover = true;
        label.text = "Win";
    }

}
