using UnityEngine;
using System.Collections;
using Battle;
using Model;

public class PlayerScript : CreatureScript {
    public MonsterScript Enemy;
    public Player Player = new Player();


    public override void Start()
    {
		Player.Died += PlayerDied;
        base.Start();
        
	}

	private void PlayerDied(){
		Player.Died -= PlayerDied;
		SendMessageUpwards ("OnPlayerDied");

	}

	// Update is called once per frame
    
	protected override void Update () {
        Health = Player.Health;
        base.Update();
	}
}
