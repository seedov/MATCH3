using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    public int Width, Height;
    public Element ElementPrefab;
    public Sprite[] Sprites;

    List<GameObject> S1Elements = new List<GameObject>();
    Cell[,] cells;
    List<Element> elements = new List<Element>();


	// Use this for initialization
	void Start () {
        cells = new Cell[Width, Height];
        for (var i = 0; i < Width; ++i)
        {
            for (var j = 0; j < Height; ++j)
            {
                var c = new GameObject();
                c.name = "Cell_" + i + "_" + j;
                c.transform.parent = transform;
                c.transform.localScale = Vector3.one ;
                c.transform.localPosition = new Vector2(i , j);
                c.AddComponent<Cell>();

                var cell = c.GetComponent<Cell>();
				cell.ColIndex = i;
				cell.RowIndex = j;
                cells[i, j] = cell;

                var e = Instantiate(ElementPrefab) as Element;
				var rand = Random.Range(0,6);
                e.Init(Sprites[rand]);
//				e.State = (State)rand;
                e.transform.parent = transform;
                e.transform.localPosition = new Vector2(i, j);
                //e.State = State.s1;
                elements.Add(e);
                cell.Element = e;
            }
        }	

        for (var i = 0; i < Width; ++i)
            for (var j = 0; j < Height; ++j)
            {
                if (i > 0) cells[i, j].Left = cells[i - 1, j];
                if (i < Width-1) cells[i, j].Right = cells[i + 1, j];
                if (j > 0) cells[i, j].Down = cells[i, j-1];
                if (j < Height-1) cells[i, j].Up = cells[i, j+1];
            }


//        CheckRows();
//        CheckColumns();
	}

    void OnGUI()
    {

    }

	private void MoveColumn(int columnIndex, float value){
		var elements = new List<Element> ();

		for(var j=0; j<Height; ++j)
			elements.Add(cells[columnIndex, j].Element);

        foreach (var el in elements)
        {
            el.transform.position += Vector3.up * value / 100;
            if (el.transform.position.y > cells[0, Height - 1].transform.position.y + .5f)
                el.transform.position -= Vector3.up * 6;
            else if (el.transform.position.y < cells[0, 0].transform.position.y - .5f)
                el.transform.position += Vector3.up * 6;


        }
	}

	private void MoveRow(int rowIndex, float value){
		var elements = new List<Element> ();
		
		for(var j=0; j<Height; ++j)
			elements.Add(cells[j, rowIndex].Element);

        foreach (var el in elements)
        {
            el.transform.position += Vector3.right * value / 100;
            if (el.transform.position.x > cells[Width - 1, 0].transform.position.x + .5f)
                el.transform.position -= Vector3.right * 6;
            else if (el.transform.position.x < cells[0, 0].transform.position.x - .5f)
                el.transform.position += Vector3.right * 6;
        }
	}

	private void Normalize(){
        foreach (var el in elements)
        {
            var nc = FindNearestCell(el);
            if (nc != null)
            {
                nc.Element = el;
                el.transform.position = nc.transform.position;
            }
        }
	}

    Cell FindNearestCell(Element element)
    {
        foreach (var cell in cells)
        {
            if (Vector3.Distance(cell.transform.position, element.transform.position) < .5f)
                return cell;
        }
        return null;
    }

    private void CheckColumns()
    {
        bool check = false;
        //do
        {
            check = false;
            for (var j = 0; j < Width; ++j)
            {
                var col = new Cell[Height];
                for (var i = 0; i < Height - 1; ++i)
                    col[i] = cells[j, i];

                var sequence = new List<Cell>();
                sequence.Add(col[0]);
                for (var i = 0; i < Height - 1; ++i)
                {
                    if (col[i].Element.State == col[i].Up.Element.State)
                    {
                        sequence.Add(col[i].Up);
                    }
                    else
                    {
                        if (sequence.Count < 3)
                        {
                            sequence.Clear();
                            sequence.Add(col[i].Up);
                        }
                    }
                }

                if (sequence.Count > 2)
                {
                    print(sequence.Count);
                    DestroyColumn(sequence);
                    check = true;
                        //c.Element.renderer.material.color = Color.red;
                        //Destroy(c.Element.gameObject);
                }
                //    else
                //        return;
            }
        } 
        //while (check);
    }

    private void CheckRows()
    {
        bool check = false;
      //  do
        {
            check = false;
            for (var j = 0; j < Height; ++j)
            {
                var row = new Cell[Width];
                for (var i = 0; i < Width - 1; ++i)
                    row[i] = cells[i, j];

                var sequence = new List<Cell>();
                sequence.Add(row[0]);
                for (var i = 0; i < Width - 1; ++i)
                {
                    if (row[i].Element.State == row[i].Right.Element.State)
                    {
                        sequence.Add(row[i].Right);
                    }
                    else
                    {
                        if (sequence.Count < 3)
                        {
                            sequence.Clear();
                            sequence.Add(row[i].Right);
                        }
                    }

                }
                if (sequence.Count > 2)
                {
                    print(sequence.Count);
                    DestroyRow(sequence);
                    check = true;
                        //c.Element.renderer.material.color = Color.red;
                        //Destroy(c.Element.gameObject);
                }
            //    else
            //        return;
            }
        }
  //      while(check);
    }

    //Уничтожить вертикальную секвенцию и переместить все верхние ячейки на их место
    public void DestroyColumn(List<Cell> sequence)
    {
		var destroyableElements = new List<Element> ();
		foreach (var c in sequence)
			destroyableElements.Add (c.Element);


		var aboveCell = sequence [sequence.Count - 1].Up;
		var currCell = sequence [0];
		while(aboveCell!=null) {
			currCell.Element = aboveCell.Element;
			currCell.Element.MoveToPosition(currCell.transform.position);
			currCell = currCell.Up;
			aboveCell = aboveCell.Up;
		}

		foreach(var e in destroyableElements){
			currCell.Element = e;
			e.Init(Sprites[Random.Range(0, 6)]);
			currCell.Element.transform.position = currCell.transform.position;
			currCell = currCell.Up;

		}
    }


    //Уничтожить горизонтальную секвенцию и переместить все верхние ячейки на их место
    public void DestroyRow(List<Cell> sequence)
    {
        foreach (var c in sequence)
        {
            var destroyableElement = c.Element;

            var cell = c;
            while (cell.Up != null)
            {
                cell.Element = cell.Up.Element;
                cell.Element.MoveToPosition(cell.transform.position);
                cell = cell.Up;
            }
            destroyableElement.Init(Sprites[Random.Range(0, 6)]);
            cell.Element = destroyableElement;
            cell.Element.transform.position = cell.transform.position;
        }
    }

	Cell selectedCell;
	Vector3 deltaMousePos;
	Vector3 prevMousePos, initMousePos;
	bool movingRow, movingCol;
	bool isDown;

    // Update is called once per frame
    void Update()
    {
        if (!isDown)
        {
            foreach(var cell in cells)
                cell.Element.transform.position = cell.transform.position;
        }

		if (Input.GetMouseButtonDown (0)) {
			initMousePos = Input.mousePosition;
			prevMousePos = Input.mousePosition;
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				var cell = hit.collider.GetComponent<Cell>();
				if(cell !=null){
//					cell.Element.renderer.material.color = Color.green;
					selectedCell = cell;
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) {
            Normalize();
            print("up");
            if (movingCol)
            {
                CheckRows();
                CheckColumns();
            }
            else if (movingRow)
            {
                CheckColumns();
                CheckRows();
            }
            isDown = false;
            movingCol = false;
            movingRow = false;
            
		}
		if (!Input.GetMouseButton (0)) {
						return;
		} 


		deltaMousePos = Input.mousePosition - prevMousePos;
		prevMousePos = Input.mousePosition;

		//if (deltaMousePos.x > 1 || deltaMousePos.y>1) 
        {
			if (Mathf.Abs (Input.mousePosition.x - initMousePos.x) > Mathf.Abs (Input.mousePosition.y - initMousePos.y)) {
				if(!isDown){
					isDown = true;
				movingRow = true;
				movingCol = false;
				}
			} else if (Mathf.Abs (Input.mousePosition.x -initMousePos.x) < Mathf.Abs (Input.mousePosition.y - initMousePos.y)) {
				if(!isDown){
					isDown = true;
				movingCol = true;
				movingRow = false;
				}
			}
		}

		if(movingRow)
			MoveRow(selectedCell.RowIndex, deltaMousePos.x);
		else if (movingCol)
			MoveColumn(selectedCell.ColIndex, deltaMousePos.y);


      //  for (var i = 1; i < Width-1; ++i)
        {
            for (var j = 0; j < Height-1; ++j)
            {
          //      CheckRows(j);
            }
        }
	}
}
