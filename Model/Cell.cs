using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Cell
    {
        public Cell Left{get;set;}
        public Cell Right{get;set;}
        public Cell Up{get;set;}
        public Cell Down { get; set; }

        public int RowIndex, ColIndex;

//        public Vector2 CellPosition;
        public Element Element { get; set; }

        /// <summary>
        /// Создать новую ячейку
        /// </summary>
        /// <param name="row">Индекс строки для данной ячейки</param>
        /// <param name="col">Индекс столбца для данной ячейки</param>
        public Cell(int col, int row)
        {
            RowIndex = row;
            ColIndex = col;
        }
    }
}
