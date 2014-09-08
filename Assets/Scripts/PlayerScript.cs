using UnityEngine;
using System.Collections;
using Battle;
using Model;

public class PlayerScript : CreatureScript {
    public MonsterScript Enemy;
    public Player Player = new Player();


    protected override void Start()
    {
        Player.Died += () =>
        {
            //HealthBar.gameObject.SetActive(false);
            SendMessageUpwards("PrintLoose");

        };
        base.Start();
        
	}
	
	// Update is called once per frame
    
	protected override void Update () {
        Health = Player.Health;
        base.Update();
	}
}
