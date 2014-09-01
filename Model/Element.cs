using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Element
    {
        private Random rnd;
        private int amountOfStates;

        public State State { get; private set; }

        public Element()
        {
            rnd = new Random();
            amountOfStates = Enum.GetNames(typeof(State)).Length;
        }

        public void Init()
        {
            State = (State)GetNextRandomState();
        }
        private int GetNextRandomState()
        {
            return rnd.Next(0, amountOfStates);
        }
    }

    public enum State
    {
        s1 = 1, s2, s3, s4, s5, s6
    }
}
