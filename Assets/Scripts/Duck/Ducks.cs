using System.Collections.Generic;
using UnityEngine;

namespace Duck
{
    public class Ducks : MonoBehaviour
    {
        [SerializeField] private List<GameObject> activeDucks = new();
        public List<GameObject> ActiveDucks => activeDucks;
    }
}