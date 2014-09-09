using UnityEngine;
using System.Collections;
using Battle;
using Model;


public class MonsterScript : CreatureScript {

    public PlayerScript Enemy;
    public Monster Monster = new Monster();



    public override void Start()
    {
		Monster.Died += MonsterDied;
        base.Start();

    }

	private void MonsterDied(){
		Monster.Died -= MonsterDied;
		print ("monster died");
		SendMessageUpwards ("OnMonsterDied");
	}
	// Update is called once per frame
	protected override void Update () {
        Health = Monster.Health;
        base.Update();
	}
}
