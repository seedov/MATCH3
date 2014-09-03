using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class ElementScript : MonoBehaviour {
    public string ElementState;
    public string ElementEffect;
    private SpriteRenderer sprite;
    private Model.Element element = new Model.Element();
    private Sprite[] sprites;
    private TextMesh text;

    public Model.Element Element
    {
        get { return element; }
        set
        {
            element = value;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Element == null) return;
        ElementState = Element.State.ToString();
        ElementEffect = Element.Effect.ToString();
        switch (Element.Effect)
        {
            case Model.Effects.radius:
            case Model.Effects.cross:
            case Model.Effects.all:
                text.text = Element.Effect.ToString();
                break;

            case Model.Effects.no:
            default:
                text.text = "";
                break;
        }
        if (Element.State == Model.State.uni)
        {
            sprite.sprite = Array.Find(sprites, s => s.name == "skype");
        }
        else
        {
            sprite.sprite = Array.Find(sprites, s => s.name == Element.State.ToString()[1].ToString());
        }
       // print(sprite.sprite);
	}
    void Awake()
    {
        text = GetComponentInChildren<TextMesh>();
        sprite = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("");
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

        yield return new WaitForSeconds(.2f);
        //        this.sprite.sprite = newSprite;
        //        State = (State)int.Parse(sprite.name);
        transform.localScale = Vector3.one;
        renderer.material.color = Color.white;
    }
}
