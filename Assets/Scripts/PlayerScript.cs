using UnityEngine;
using System.Collections;
using Battle;
using Model;

public class PlayerScript : MonoBehaviour {
    public float Health;
    private UISlider HealthBar;
    public MonsterScript Enemy;
    public Player Player = new Player();

    void Awake()
    {
        HealthBar = GetComponentInChildren<UISlider>();
    }

	void Start () {
        Player.Died += () =>
        {
            //HealthBar.gameObject.SetActive(false);
            SendMessageUpwards("PrintLoose");

        };
	}
	
	// Update is called once per frame
	void Update () {
        Health = Player.Health;
        HealthBar.value = Health / 100;

	}
}
