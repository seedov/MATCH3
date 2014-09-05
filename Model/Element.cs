using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Element
    {
        private static Random rnd= new Random();
        private static int amountOfStates=6;

        public State State { get; set; }
        public Effects Effect { get; set; }

        public Element()
        {
            Effect = Effects.no;
            
    //        amountOfStates = Enum.GetNames(typeof(State)).Length-1;
        }

        public void Init()
        {
            OnDestroyed();
            State = (State)GetNextRandomState();
            Effect = Effects.no;
        }
        private static int GetNextRandomState()
        {
            return rnd.Next(1, amountOfStates);
        }

        #region Events and invokations
        public event Action<Cell> CellChanged
        {
            add { cellChanged += value; }
            remove { cellChanged -= value; }
        }
        private Action<Cell> cellChanged;
        public void OnPositionChanged(Cell newCell)
        {
            var h = cellChanged;
            if (h != null) h(newCell);
        }

        public event Action Destroyed
        {
            add { destroyed += value; }
            remove { destroyed -= value; }
        }
        private Action destroyed;
        public void OnDestroyed()
        {
            var h = destroyed;
            if (h != null) h();
        }

        #endregion
    }

    public enum State
    {
        empty=0, s1 = 1, s2, s3, s4, s5, s6, uni
    }

    public enum Effects
    {
        no,//нет эффекта
        radius1,//уничтожает еще все элементы в радиусе 1
        radius2,//уничтожает еще все элементы в радиусе 2 
        cross,//уничтожает все элементы в данном столбце и строке
        star,//cross + уничтожает все элеметы по диагоналям
        all//уничтожает все элементы
    }
}
