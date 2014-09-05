using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Battle
{
    public class Monster:Creature
    {
        private const int minDamage = 5, maxDamage = 30;
        private Random rnd = new Random();

        public void ApplyDamage(Element[] elements)
        {
            var elementsByState = elements.GroupBy(e => e.State);
            foreach (var grouppedElements in elementsByState)
            {
                ApplyDamage(grouppedElements.Count());
            }
        }


        public override void AttackEnemy()
        {
            Enemy.ApplyDamage(rnd.Next(minDamage, maxDamage));
        }



    }
}
