using UnityEngine;
using System.Collections;
using System;

public class CreatureScript : MonoBehaviour {
    [NonSerialized]
    public float Health;
    [NonSerialized]
    UILabel label;
    [NonSerialized]
    private UISlider HealthBar;
    [NonSerialized]
    protected float lastHelath;

    private TweenAlpha tween;

    void Awake()
    {
        label = GetComponentInChildren<UILabel>();
        label.text = "";
        tween = label.GetComponent<TweenAlpha>();
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
            label.text = lastHelath==0?"": "-" + (lastHelath - Health);
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
