using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Element : MonoBehaviour {
    public State State;
    private SpriteRenderer sprite;
//    public Sprite SplashSprite;
    bool isMoving, isDestroying;
    Vector3 targetPos, startPos;
    float startTime;
    float jorneyLength;

    [HideInInspector]
    public Sprite Sprite;

    private Sprite newSprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(Sprite sprite)
    {
        this.sprite.sprite = sprite;
//        if (this.sprite.sprite == null)
//            this.sprite.sprite = sprite;

//        newSprite = sprite;
//        isDestroying = true;
        renderer.material.color = new Color(255,255,255,0);
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

    void Start () {
	
	}


    public void MoveToPosition(Vector3 pos)
    {
        transform.position = pos;
        return;
        startPos = transform.position;
        targetPos = pos;
        startTime = Time.time;
        jorneyLength = Vector3.Distance(startPos, targetPos);
        isMoving = true;
    }
	
	void Update () {
        if (isDestroying)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);
            if (transform.localScale.x <= 0.5f)
                isDestroying = false;
           
        }

        if (isMoving)
        {
//            print("startPos = " + startPos + " targetPos = " + targetPos + " jorneyLength = " + jorneyLength);
            transform.position = Vector3.Lerp(transform.position, targetPos, (Time.time - startTime) / jorneyLength);
            if (transform.position == targetPos) isMoving = false;
        }
	
	}
}
public enum State
{
    s1=1, s2, s3, s4, s5, s6
}