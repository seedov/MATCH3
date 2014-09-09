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

        public float ApplyDamage(Element[] elements, float multiplier=1)
        {
            var damageApplied = 0f;
            var elementsByState = elements.GroupBy(e => e.State);
            foreach (var grouppedElements in elementsByState)
            {
                foreach(var e in grouppedElements){
                    ApplyDamage(e.DamageMultiplier);
                    damageApplied += e.DamageMultiplier;
                //if (e.State == State.uni)
                //{
                //    if(chainState == State.s1)
                //}
                }
                //ApplyDamage(grouppedElements.Count()*multiplier);
            }
            return damageApplied;
        }


        public override void AttackEnemy()
        {
            Enemy.ApplyDamage(rnd.Next(minDamage, maxDamage));
        }

        

    }
}
