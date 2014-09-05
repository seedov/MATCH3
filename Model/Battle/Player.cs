using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Battle
{
    public class Player:Creature
    {
        Element[] collectedElements;

        public override void ApplyDamage(float Damage)
        {
            base.ApplyDamage(Damage);
        }

        public override void AttackEnemy()
        {
            ((Monster)Enemy).ApplyDamage(collectedElements);
        }

        public void CollectElements(Element[] elements)
        {
            collectedElements = elements;
        }

    }
}
