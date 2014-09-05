using UnityEngine;
using System.Collections;
using Battle;
using Model;


public class MonsterScript : MonoBehaviour {
    public float Health;
    private UISlider HealthBar;

    public PlayerScript Enemy;
    public Monster Monster = new Monster();
	// Use this for initialization
    void Awake()
    {
        HealthBar = GetComponentInChildren<UISlider>();
    }
	void Start () {
        Monster.Died += () =>
        {
            //HealthBar.gameObject.SetActive(false);
            SendMessageUpwards("PrintWin");
        };
	
	}
	
	// Update is called once per frame
	void Update () {
        Health = Monster.Health;
        HealthBar.value = Health / 100;
	}
}
