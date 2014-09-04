using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class CellScript : MonoBehaviour {
    public string ElementState;
    public Model.Cell Cell;
  //  public CellScript Left, Right, Up, Down;

    public int RowIndex, ColIndex;

    public Vector2 CellPosition;
    public ElementScript Element;

    // Use this for initialization
    void Start()
    {
//        Cell.Element.CellChanged += (c) => print("cell changed");
    }

    public void DestroyElement()
    {
        Element.gameObject.renderer.material.color = Color.red;
    }

    public void InitElement()
    {
        Element.gameObject.renderer.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (Element == null) return;
        if (Element.Element == null) return;
        ElementState = Cell.Element.State.ToString();
//        Element.Element.State = Cell.Element.State;
//        Element.Element.Effect = Cell.Element.Effect;
    }
}
