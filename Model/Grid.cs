using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{

    public class Grid
    {
        private Cell[,] cells;
        public Cell[,] Cells { get { return cells; } }
        private List<Element> elements = new List<Element>();



        public int Width { get; set; }
        public int Height { get; set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            cells = new Cell[Width, Height];
            for (var i = 0; i < Width; ++i)
            {
                for (var j = 0; j < Height; ++j)
                {
                    var cell = new Cell(i, j);

                    cells[i, j] = cell;

                    var element = new Element();
                    element.Init();
                    elements.Add(element);
                    cell.Element = element;
                }
            }

            for (var i = 0; i < Width; ++i)
                for (var j = 0; j < Height; ++j)
                {
                    if (i > 0) cells[i, j].Left = cells[i - 1, j];
                    if (i < Width - 1) cells[i, j].Right = cells[i + 1, j];
                    if (j > 0) cells[i, j].Down = cells[i, j - 1];
                    if (j < Height - 1) cells[i, j].Up = cells[i, j + 1];
                }


            //        CheckRows();
            //        CheckColumns();

        }


        /// <summary>
        /// Прокрутить полностью весь столбец с индексом <see cref="columnIndex"/> на количество ячеек <see cref="length"/>. Элементы скроллятся по кругу.
        /// </summary>
        /// <param name="columnIndex">Индекс столбца для скролла</param>
        /// <param name="length">Количество ячеек на которое происходит скролл. Если больше нуля - скролл вверх, если меньше - вниз</param>
        public void ScrollColumn(int columnIndex, int length)
        {
            while (length >= Height) 
                length -= Height;
            while (length < 0)
                length += Height;

            var elem = cells[columnIndex, 0].Element;
            for (var i=0; i<Height; ++i)
            {
                var rowIndex = i + length >= Height ? i+length -Height : i + length;
                cells[columnIndex, i].Element = cells[columnIndex, rowIndex].Element;
            }

        }

        /// <summary>
        /// Прокрутить полностью всю строку с индексом <see cref="rowIndex"/> на количество ячеек <see cref="length"/>. Элементы скроллятся по кругу.
        /// </summary>
        /// <param name="columnIndex">Индекс строки для скролла</param>
        /// <param name="length">Количество ячеек на которое происходит скролл. Если больше нуля - скролл вправо, если меньше - влево</param>
        private void ScrollRow(int rowIndex, int length)
        {
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
            var destroyableElements = new List<Element>();
            foreach (var c in sequence)
                destroyableElements.Add(c.Element);


            var aboveCell = sequence[sequence.Count - 1].Up;
            var currCell = sequence[0];
            while (aboveCell != null)
            {
                currCell.Element = aboveCell.Element;
//                currCell.Element.MoveToPosition(currCell.transform.position);
                currCell = currCell.Up;
                aboveCell = aboveCell.Up;
            }

            foreach (var e in destroyableElements)
            {
                currCell.Element = e;
                e.Init();
//                currCell.Element.transform.position = currCell.transform.position;
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
    //                cell.Element.MoveToPosition(cell.transform.position);
                    cell = cell.Up;
                }
                destroyableElement.Init();
                cell.Element = destroyableElement;
    //            cell.Element.transform.position = cell.transform.position;
            }
        }
    }
}
