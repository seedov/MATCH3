using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Model;
using System.Linq;

public class GridScript : MonoBehaviour {
    public int Width, Height;
    public ElementScript ElementPrefab;
    public Sprite[] Sprites;
    Model.Grid grid;
    public CellScript[,] cells;
    CellScript selectedCell;
    Vector3 deltaMousePos;
    Vector3 prevMousePos, initMousePos;
    bool movingRow, movingCol;
    bool isDown;



    public PlayerScript Player;
    public MonsterScript Monster;

    public Regimes Regime;

	void Awake(){


	}

	// Use this for initialization
	void Start () {
        Player.Player.Enemy = Monster.Monster;
        Monster.Monster.Enemy = Player.Player;

        grid = new Model.Grid(Width, Height);
        grid.SequenceDestroyed += (seq) =>
        {
            foreach (var c in seq)
                cells[c.ColIndex, c.RowIndex].Element.Destroy();
        };
        cells = new CellScript[Width, Height];
        for (var i = 0; i < Width; ++i)
        {
            for (var j = 0; j < Height; ++j)
            {
                var c = new GameObject();
                c.name = "Cell_" + i + "_" + j;
                c.transform.parent = transform;
                c.transform.localScale = Vector3.one;
                c.transform.localPosition = new Vector2(i, j);
                c.AddComponent<CellScript>();

                var cell = c.GetComponent<CellScript>();
                cell.ColIndex = i;
                cell.RowIndex = j;
                cells[i, j] = cell;
                cell.Cell = grid.Cells[i, j];

                var e = Instantiate(ElementPrefab) as ElementScript;
                e.grid = this;
                //				e.State = (State)rand;
                //e.Init(Sprites[0]);
                e.transform.parent = transform;
                e.transform.localPosition = new Vector2(i, j);
                //e.State = State.s1;
             //   elements.Add(e);
                cell.Element = e;
                e.Element = cell.Cell.Element;
            }

        }
        if (Regime == Regimes.Scroll)
        {
            Vert();
            Hor();
        }
	}


    bool animationComplete=true;
    private void Vert()
    {
        if (!animationComplete) return;
      //  while (true)
        {
            var vm = grid.FindVerticalMatch();
            if (vm != null)
            {
                foreach (var c in vm)
                {
                    cells[c.RowIndex, c.ColIndex].DestroyElement();
                }
                StartCoroutine(WaitAndDestroyVertical());
             //   grid.DestroyVerticalSequence(vm);
                
            }
            else
                return;
        }
    }

    private void Hor()
    {
        if (!animationComplete) return;
        //  while (true)
        {
            var vm = grid.FindHorizontalMatch();
            if (vm != null)
            {
                foreach (var c in vm)
                {
                    cells[c.RowIndex, c.ColIndex].DestroyElement();
                }
                StartCoroutine(WaitAndDestroyHorizontal());
                //   grid.DestroyVerticalSequence(vm);

            }
            else
                return;
        }
    }
    

    IEnumerator WaitAndDestroyVertical()
    {
        //while (true)
        //{
        //    var vm = grid.FindVerticalMatch();
        //    if (vm != null)
        //    {
        //        foreach (var c in vm)
        //        {
        //            cells[c.RowIndex, c.ColIndex].DestroyElement();
        //        }
                yield return new WaitForSeconds(1);
        //        foreach (var c in vm)
        //            cells[c.RowIndex, c.ColIndex].InitElement();

        //        grid.DestroyVerticalSequence(vm);
        //        print("VM");

        //    }
        //    else
        //        break;
        //}
    }

    IEnumerator WaitAndDestroyHorizontal()
    {
        while (true)
        {
            var vm = grid.FindHorizontalMatch();
            if (vm != null)
            {
                foreach (var c in vm)
                {
                    cells[c.RowIndex, c.ColIndex].DestroyElement();
                }
                yield return new WaitForSeconds(1);
                foreach (var c in vm)
                    cells[c.RowIndex, c.ColIndex].InitElement();

                grid.DestroyHorizontalSequence(vm);
                print("VM");

            }
            else
                break;
        }
    }

    private int steps;

    IEnumerator WaitAndDestroySequence()
    {
		var seqToDestroy = grid.FindSequenceToDestroy(selectedCells.ToArray());
        var vm =  selectedCells.ToArray();
        Player.Player.CollectElements(vm.Select(cell=>cell.Element).ToArray());
        Player.Player.AttackEnemy();
        steps++;
        if (steps == 3)
        {
            steps = 0;
            Monster.Monster.AttackEnemy();
        }
        if (vm != null)
        {
            foreach (var c in seqToDestroy)
            {
                cells[c.ColIndex, c.RowIndex].DestroyElement();
            }
            yield return new WaitForSeconds(1);
            foreach (var c in seqToDestroy)
                cells[c.ColIndex, c.RowIndex].InitElement();

            grid.DestroySequence(vm);
            print("VM");

        }
    }

    private void ScrollRegimeUpdate(){
        if (!isDown)
        {
            foreach (var cell in cells)
                cell.Element.transform.position = cell.transform.position;
        }

        if (Input.GetMouseButtonDown(0))
        {
            initMousePos = Input.mousePosition;
            prevMousePos = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var cell = hit.collider.GetComponent<CellScript>();
                if (cell != null)
                {
                    //					cell.Element.renderer.material.color = Color.green;
                    selectedCell = cell;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {

            if (movingCol)
            {
                var delta = initMousePos.y - Input.mousePosition.y;
                grid.ScrollColumn(selectedCell.ColIndex, Mathf.RoundToInt(delta / 100));
                Hor();
            }
            else if (movingRow)
            {
                var delta = initMousePos.x - Input.mousePosition.x;
                grid.ScrollRow(selectedCell.RowIndex, Mathf.RoundToInt(delta / 100));
                Vert();
            }
            isDown = false;
            movingCol = false;
            movingRow = false;

        }
        if (!Input.GetMouseButton(0))
        {
            return;
        }


        deltaMousePos = Input.mousePosition - prevMousePos;
        prevMousePos = Input.mousePosition;

        //if (deltaMousePos.x > 1 || deltaMousePos.y>1) 
        {
            if (Mathf.Abs(Input.mousePosition.x - initMousePos.x) > Mathf.Abs(Input.mousePosition.y - initMousePos.y))
            {
                if (!isDown)
                {
                    isDown = true;
                    movingRow = true;
                    movingCol = false;
                }
            }
            else if (Mathf.Abs(Input.mousePosition.x - initMousePos.x) < Mathf.Abs(Input.mousePosition.y - initMousePos.y))
            {
                if (!isDown)
                {
                    isDown = true;
                    movingCol = true;
                    movingRow = false;
                }
            }
        }

        if (movingRow)
            MoveRow(selectedCell.RowIndex, deltaMousePos.x);
        else if (movingCol)
            MoveColumn(selectedCell.ColIndex, deltaMousePos.y);


        //  for (var i = 1; i < Width-1; ++i)
        {
            for (var j = 0; j < Height - 1; ++j)
            {
                //      CheckRows(j);
            }
        }

    }

    private List<Model.Cell> selectedCells = new List<Model.Cell>();
    public List<CellScript> SelectedCells = new List<CellScript>();
    bool isSelecting;
    public Model.State seqElementsState;
    private void LineMarkRegimeUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var cell = hit.collider.GetComponent<CellScript>();
                if (cell != null)
                {
                    seqElementsState = cell.Cell.Element.State;
                    selectedCells.Add(cell.Cell);
                    SelectedCells.Add(cell);
                }
            }
        }
        if(isSelecting)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var cell = hit.collider.GetComponent<CellScript>();
                if (cell != null)
                {
                    //					cell.Element.renderer.material.color = Color.green;
                    if (!selectedCells.Contains(cell.Cell) && (cell.Cell.Element.State == seqElementsState || cell.Cell.Element.IsUniversal))
                    {
                        var lastAddedCell = selectedCells[selectedCells.Count - 1];
                        if (Mathf.Abs(cell.ColIndex - lastAddedCell.ColIndex) < 2 && Mathf.Abs(cell.RowIndex - lastAddedCell.RowIndex) < 2)
                        {
                            selectedCells.Add(cell.Cell);
                            SelectedCells.Add(cell);
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
			if(selectedCells.Count<3){
				selectedCells.Clear();
            	SelectedCells.Clear();
				isSelecting = false;
				return;
			}
            isSelecting = false;
      //      grid.DestroySequence(selectedCells.ToArray()); 
            StartCoroutine(WaitAndDestroySequence());
            selectedCells.Clear();
            SelectedCells.Clear();

        }
    }
    void Update()
    {
        switch (Regime){
            case Regimes.Scroll:
                ScrollRegimeUpdate();
                break;
            case Regimes.LineMark:
                LineMarkRegimeUpdate();
                break;
        }
        
    }

    private void MoveColumn(int columnIndex, float value)
    {
        var elements = new List<ElementScript>();

        for (var j = 0; j < Height; ++j)
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

    private void MoveRow(int rowIndex, float value)
    {
        var elements = new List<ElementScript>();

        for (var j = 0; j < Height; ++j)
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

}

public enum Regimes
{
    Scroll,
    LineMark
}
