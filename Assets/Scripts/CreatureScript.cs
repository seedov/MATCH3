using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CreatureScript : MonoBehaviour {
    public float Health;
    private UISlider HealthBar;
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
        log.textLabel.supportEncoding = true;

        lastHelath = Health;
		log.Clear ();
    //    log.Add(String.Format("[{0}]{1}[-]{2}", "0000FF", new string('0', 3), "5"));
    }
    protected virtual void Update()
    {
        if (Health != lastHelath)
        {
			var deltaHP=Health - lastHelath;
            string col = deltaHP<0?"FFFFFF":"00FF00";
            deltaHpLabel.text = lastHelath == 0 ? "" : "" + deltaHP;
			deltaHpLabel.color = deltaHP>0?Color.green:Color.red;
            hpLabel.text = Health.ToString();
            HealthBar.value = Health / 100;
            log.Add(String.Format("[{0}]{1}", col, Health.ToString()));
            lastHelath = Health;
//            tween.gameObject.SetActive(true);
            tween.from = 1;
            tween.to = 0;
            tween.ResetToBeginning();
            tween.PlayForward();
            var tweencolor = HealthBar.GetComponentInChildren<TweenColor>();
            tweencolor.from = deltaHP > 0 ? Color.green : Color.red;
            tweencolor.ResetToBeginning();
            tweencolor.PlayForward();

           
        }
    }

    public void OnHealthBarValueChanged()
    {
    }

}
