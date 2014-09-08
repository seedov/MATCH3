using UnityEngine;
using System.Collections;
using Battle;
using Model;


public class MonsterScript : CreatureScript {

    public PlayerScript Enemy;
    public Monster Monster = new Monster();



    protected override void Start()
    {
        Monster.Died += () =>
        {
            //HealthBar.gameObject.SetActive(false);
            SendMessageUpwards("PrintLoose");

        };
        base.Start();

    }
	// Update is called once per frame
	protected override void Update () {
        Health = Monster.Health;
        base.Update();
	}
}
