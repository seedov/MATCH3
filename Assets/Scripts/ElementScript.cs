using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using Model;

public class ElementScript : MonoBehaviour {
    public GridScript grid;
    public string ElementState;
    public string ElementEffect;
    private SpriteRenderer sprite;
    private Model.Element element = new Model.Element();
    private Sprite[] sprites;
    private TextMesh text;
    private Vector3 newPosition, startPosition;
    float startTime, jorneyLength;

    private TweenScale tweenScale;
    private TweenPosition textTweenPosition;
    private TweenAlpha textTweenAlpha;

    public Texture2D SourceTexture;



    public Model.Element Element
    {
        get { return element; }
        set
        {
            element = value;
        }
    }

    bool isJustDestroyed;

	// Use this for initialization
	void Start () {
        newPosition = transform.position;
        Element.CellChanged += (c) =>
        {
            element = c.Element;
            var cell = grid.cells[c.ColIndex, c.RowIndex];
            cell.Element = this;
            startTime = Time.time;
            startPosition = transform.position;
            newPosition = cell.transform.position;
            jorneyLength = Vector3.Distance(startPosition, newPosition);

            //transform.localPosition = cell.transform.position;
        };

        Element.Destroyed += () =>
        {
			//switch(Element.Effect){
			//case Effects.radius1:
			text.text = "Destroyed";
				//	break;
			//}
			textTweenPosition.ResetToBeginning();
			textTweenPosition.PlayForward();
            print("destroyed");
            isJustDestroyed = true;
            renderer.material.color = new Color(255, 255, 255, 0);
            StartCoroutine(WaitAndSetVisible());
        };

	}
	
	// Update is called once per frame
	void Update () {
        if (isJustDestroyed)
        {
            transform.position = newPosition;
            isJustDestroyed = false;
        }
        if (transform.position != newPosition)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, (Time.time - startTime));
        }
        if (Element == null) return;
        ElementState = Element.State.ToString();
        ElementEffect = Element.Effect.ToString();

  //      text.text = "";
        if(textTweenPosition.value == textTweenPosition.from)
        switch (Element.Effect)
        {
            case Model.Effects.radius1: print("Ss");
                text.text = "Charged";
                textTweenPosition.ResetToBeginning();
                textTweenPosition.PlayForward();
                break;
            case Model.Effects.cross:
                text.text = "Cross";
                textTweenPosition.ResetToBeginning();
                textTweenPosition.PlayForward();
                break;
            case Model.Effects.star:
                //                            tweenScale.PlayForward();
                text.text = "Star";
                textTweenPosition.ResetToBeginning();
                textTweenPosition.PlayForward();
                break;

            case Model.Effects.no:
            default:
                //                            tweenScale.ResetToBeginning();
                sprite.sprite = Array.Find(sprites, s => s.name == "fire_normal");
                text.text = "";
                break;
        }


        if (Element.IsUniversal)
        {
            sprite.sprite = Array.Find(sprites, s => s.name == "universal_normal");
        }
        else
        {
            switch (Element.State)
            {
                case Model.State.s1:
                    switch (Element.Effect)
                    {
                        case Model.Effects.radius1:
                            sprite.sprite = Array.Find(sprites, s => s.name == "fire_charged");
                            break;
                        case Model.Effects.cross:
                            sprite.sprite = Array.Find(sprites, s => s.name == "fire_cross");
                            break;
                        case Model.Effects.star:
                            sprite.sprite = Array.Find(sprites, s => s.name == "fire_star");
//                            tweenScale.PlayForward();
                            break;

                        case Model.Effects.no:
                        default:
//                            tweenScale.ResetToBeginning();
                            sprite.sprite = Array.Find(sprites, s => s.name == "fire_normal");
                            break;
                    }
                    break;
                case Model.State.s2:
                    switch (Element.Effect)
                    {
                        case Model.Effects.radius1:
                            sprite.sprite = Array.Find(sprites, s => s.name == "water_charged");
                            break;
                        case Model.Effects.cross:
                            sprite.sprite = Array.Find(sprites, s => s.name == "water_cross");
                            break;
                        case Model.Effects.star:
                            sprite.sprite = Array.Find(sprites, s => s.name == "water_star");
                            //                            tweenScale.PlayForward();
                            break;

                        case Model.Effects.no:
                        default:
                            //                            tweenScale.ResetToBeginning();
                            sprite.sprite = Array.Find(sprites, s => s.name == "water_normal");
                            break;
                    }
                    break;
                case Model.State.s3:
                    switch (Element.Effect)
                    {
                        case Model.Effects.radius1:
                            sprite.sprite = Array.Find(sprites, s => s.name == "nature_charged");
                            break;
                        case Model.Effects.cross:
                            sprite.sprite = Array.Find(sprites, s => s.name == "nature_cross");
                            break;
                        case Model.Effects.star:
                            sprite.sprite = Array.Find(sprites, s => s.name == "nature_star");
                            //                            tweenScale.PlayForward();
                            break;

                        case Model.Effects.no:
                        default:
                            //                            tweenScale.ResetToBeginning();
                            sprite.sprite = Array.Find(sprites, s => s.name == "nature_normal");
                            break;
                    }
                    break;
                case Model.State.s4:
                    switch (Element.Effect)
                    {
                        case Model.Effects.radius1:
                            sprite.sprite = Array.Find(sprites, s => s.name == "light_charged");
                            break;
                        case Model.Effects.cross:
                            sprite.sprite = Array.Find(sprites, s => s.name == "light_cross");
                            break;
                        case Model.Effects.star:
                            sprite.sprite = Array.Find(sprites, s => s.name == "light_star");
                            //                            tweenScale.PlayForward();
                            break;

                        case Model.Effects.no:
                        default:
                            //                            tweenScale.ResetToBeginning();
                            sprite.sprite = Array.Find(sprites, s => s.name == "light_normal");
                            break;
                    }
                    break;
                case Model.State.s5:
                    switch (Element.Effect)
                    {
                        case Model.Effects.radius1:
                            sprite.sprite = Array.Find(sprites, s => s.name == "poison_charged");
                            break;
                        case Model.Effects.cross:
                            sprite.sprite = Array.Find(sprites, s => s.name == "poison_cross");
                            break;
                        case Model.Effects.star:
                            sprite.sprite = Array.Find(sprites, s => s.name == "poison_star");
                            //                            tweenScale.PlayForward();
                            break;

                        case Model.Effects.no:
                        default:
                            //                            tweenScale.ResetToBeginning();
                            sprite.sprite = Array.Find(sprites, s => s.name == "poison_normal");
                            break;
                    }
                    break;

            }
//            sprite.sprite = Array.Find(sprites, s => s.name == Element.State.ToString()[1].ToString());
        }
       // print(sprite.sprite);
	}
    void Awake()
    {
        tweenScale = GetComponent<TweenScale>();
        text = GetComponentInChildren<TextMesh>();
        textTweenAlpha = text.GetComponent<TweenAlpha>();
        textTweenPosition = text.GetComponent<TweenPosition>();
        sprite = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("");
    }

    public void TextTweenPositionComplete()
    {
        text.text = "";
    }

    public void Destroy()
    {
  //      gameObject.renderer.material.color = Color.green;
//        renderer.material.color = Color.green;
//        StartCoroutine(WaitAndSetVisible());
    }

    public void Init()
    {
        gameObject.renderer.material.color = Color.white;
    }

    public void Init(Sprite sprite)
    {
        this.sprite.sprite = sprite;
        //        if (this.sprite.sprite == null)
        //            this.sprite.sprite = sprite;

        //        newSprite = sprite;
        //        isDestroying = true;
        renderer.material.color = new Color(255, 255, 255, 0);
        //Destroy();
        StartCoroutine(WaitAndSetVisible());
    }
    private IEnumerator WaitAndSetVisible()
    {

        yield return new WaitForSeconds(1.5f);
        //        this.sprite.sprite = newSprite;
        //        State = (State)int.Parse(sprite.name);
        transform.localScale = Vector3.one*.7f;
        renderer.material.color = Color.white;
    }
}
