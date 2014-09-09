using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CreatureScript : MonoBehaviour {
    [NonSerialized]
    public float Health;
    [NonSerialized]
    private UISlider HealthBar;
    [NonSerialized]
    protected float lastHelath;

    public UILabel hpLabel, deltaHpLabel;
	UITextList log;
	Queue<string> logQueue = new Queue<string>();

    private TweenAlpha tween;

    void Awake()
    {
		log = GetComponentInChildren<UITextList> ();
        deltaHpLabel = GetComponentInChildren<UILabel>();
        deltaHpLabel.text = "";
        tween = deltaHpLabel.GetComponent<TweenAlpha>();
        HealthBar = GetComponentInChildren<UISlider>();
    }

	// Use this for initialization
	public virtual void Start () {
        lastHelath = Health;
		log.Clear ();
	}
    protected virtual void Update()
    {
        if (Health != lastHelath)
        {
			log.Add(Health.ToString());
			var deltaHP=Health - lastHelath;
            deltaHpLabel.text = lastHelath == 0 ? "" : "" + deltaHP;
			deltaHpLabel.color = deltaHP>0?Color.green:Color.red;
            hpLabel.text = Health.ToString();
            HealthBar.value = Health / 100;
            lastHelath = Health;
//            tween.gameObject.SetActive(true);
            tween.from = 1;
            tween.to = 0;
            tween.ResetToBeginning();
            tween.PlayForward();
        }
    }

}
