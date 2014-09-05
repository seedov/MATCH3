using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public abstract class Creature
    {
        public Creature Enemy { get; set; }
        public float Health { get; set; }
        public virtual void ApplyDamage(float Damage)
        {
            Health -= Damage;
            if (Health <= 0)
                OnDied();

        }

        public Creature()
        {
            Health = 100;
        }

        public abstract void AttackEnemy();

        #region Events and invokations
        public event Action Died
        {
            add { died += value; }
            remove { died -= value; }
        }
        private Action died;
        public void OnDied()
        {
            var h = died;
            if (h != null) died();
        }
        #endregion

    }
}
