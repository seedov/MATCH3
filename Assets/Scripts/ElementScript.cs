using UnityEngine;
using System.Collections;
using System.Linq;
using System;

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
        switch (Element.Effect)
        {
            case Model.Effects.radius1:
            case Model.Effects.cross:
            case Model.Effects.radius2:
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
        transform.localScale = Vector3.one;
        renderer.material.color = Color.white;
    }
}
