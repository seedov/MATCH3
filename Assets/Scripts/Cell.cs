using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public class Cell : MonoBehaviour {

    public Cell Left, Right, Up, Down;

	public int RowIndex, ColIndex;

    public Vector2 CellPosition;
    public Element Element;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}


