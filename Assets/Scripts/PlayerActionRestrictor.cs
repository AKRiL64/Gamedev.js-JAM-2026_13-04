using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerActionRestrictor
    {
        private List<GameObject> restrictors = new List<GameObject>();
        
        private static PlayerActionRestrictor Instance;

        public static PlayerActionRestrictor GetInstance()
        {
            if (Instance == null)
            {
                Instance = new PlayerActionRestrictor();
            }
            return Instance;
        }

        public void AddRestrictor(GameObject restrictor)
        {
            restrictors.Add(restrictor);
        }

        public void RemoveRestrictor(GameObject restrictor)
        {
            restrictors.Remove(restrictor);
        }

        public bool IsRestricted()
        {
            return restrictors.Count > 0;
        }
    }
}