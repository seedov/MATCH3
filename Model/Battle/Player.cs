using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Battle
{
    public class Player:Creature
    {
        public Storage Storage = new Storage();
        Element[] collectedElements;

        public override void ApplyDamage(float Damage, float multiplier=1)
        {
            base.ApplyDamage(Damage);
        }

        public override void AttackEnemy()
        {
            float multiplier = 1;
            foreach (var e in collectedElements)
            {
                Storage.Add(e);
                if (e.IsUniversal)
                {
                    if (e.State == State.s1)
                        multiplier *= 1.5f;
                    else if (e.State == State.s3)
                    {
                        Health += 0.1f;
                    }

                }
            }



            ((Monster)Enemy).ApplyDamage(collectedElements, multiplier);
        }

        public void CollectElements(Element[] elements)
        {
            collectedElements = elements;
        }

    }

    public class Storage
    {
        public void Add(Element e)
        {
            switch (e.State)
            {
                case State.s1:
                    FireCnt++;
                    break;
                case State.s2:
                    WaterCnt++;
                    break;
                case State.s3:
                    NatureCnt++;
                    break;
                case State.s4:
                    LightCnt++;
                    break;
                case State.s5:
                    DarknessCnt++;
                    break;
            }
        }

        public void Remove(Element e)
        {
            switch (e.State)
            {
                case State.s1:
                    FireCnt--;
                    break;
                case State.s2:
                    WaterCnt--;
                    break;
                case State.s3:
                    NatureCnt--;
                    break;
                case State.s4:
                    LightCnt--;
                    break;
                case State.s5:
                    DarknessCnt--;
                    break;
            }
        }
        public int FireCnt;
        public int WaterCnt;
        public int NatureCnt;
        public int LightCnt;
        public int DarknessCnt;
    }
}
