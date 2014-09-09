using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {
    public UILabel fire, water, nature, light, darkness;
    private PlayerScript player;
    private MonsterScript monster;

    private UILabel label;
    bool gameover;
    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
        player = GetComponentInChildren<PlayerScript>();
        monster = GetComponentInChildren<MonsterScript>();
    }
	void Start () {
        NewGame();
	}
	
	// Update is called once per frame
	void Update () {
        var f = player.Player.Storage.FireCnt;
        fire.GetComponentInParent<UIButton>().isEnabled = f > 9;
        fire.text = f.ToString();

        var w = player.Player.Storage.WaterCnt;
        water.GetComponentInParent<UIButton>().isEnabled = w > 9;
        water.text = w.ToString();

        var n = player.Player.Storage.NatureCnt;
        nature.GetComponentInParent<UIButton>().isEnabled = n > 9;
        nature.text = n.ToString();

        var l = player.Player.Storage.LightCnt;
        light.GetComponentInParent<UIButton>().isEnabled = l > 9;
        light.text = l.ToString();

        var d = player.Player.Storage.DarknessCnt;
        darkness.GetComponentInParent<UIButton>().isEnabled = d > 9;
        darkness.text = d.ToString();

        if (gameover)
        {
            if (Input.GetMouseButton(0))
                NewGame();
        }
	}

    private void Attack(Model.State state)
    {
        print("attack with " + state);
        var elements = new Model.Element[10];
        for (var i = 0; i < elements.Length; ++i)
        {
            elements[i] = new Model.Element() { State = state };
//            player.Player.Storage.Remove(fireElements[i]);
        }
        player.Player.CollectElements(elements);
        player.Player.AttackMonsterWithSpecialElement();//.AttackEnemy();
    }

    public void AttackWithFire()
    {
        Attack(Model.State.s1);
        player.Player.Storage.FireCnt -= 20;
    }

    public void AttackWithWater()
    {
        Attack(Model.State.s2);

    }
    public void AttackWithNature()
    {
        Attack(Model.State.s3);
    }
    public void AttackWithLight()
    {
        Attack(Model.State.s4);
    }
    public void AttackWithDarkness()
    {
        Attack(Model.State.s5);
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
