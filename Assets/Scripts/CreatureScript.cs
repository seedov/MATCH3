using UnityEngine;
using System.Collections;
using System;

public class CreatureScript : MonoBehaviour {
    [NonSerialized]
    public float Health;
    [NonSerialized]
    private UISlider HealthBar;
    [NonSerialized]
    protected float lastHelath;

    public UILabel hpLabel, deltaHpLabel;

    private TweenAlpha tween;

    void Awake()
    {
        deltaHpLabel = GetComponentInChildren<UILabel>();
        deltaHpLabel.text = "";
        tween = deltaHpLabel.GetComponent<TweenAlpha>();
        HealthBar = GetComponentInChildren<UISlider>();
    }

	// Use this for initialization
	protected virtual void Start () {
        lastHelath = Health;
	}
    protected virtual void Update()
    {
        if (Health != lastHelath)
        {
            deltaHpLabel.text = lastHelath == 0 ? "" : "" + (Health - lastHelath);
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
