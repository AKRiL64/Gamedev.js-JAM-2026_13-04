using UnityEngine;
using UnityEngine.UI;

namespace Hub
{
    public class AvailableFlowerEntry: MonoBehaviour
    {
        [SerializeField] public Button assignFlowerButton;
        [SerializeField] public Image assignableFlowerIcon;
        public PlantSo plantSo;
    }
}