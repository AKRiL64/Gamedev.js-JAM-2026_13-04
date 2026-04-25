using Hub.Hives;
using UnityEngine;
using UnityEngine.UI;

namespace Hub
{
    public class BeeListEntryUI: MonoBehaviour
    {
        [SerializeField] public string beeName;
        [SerializeField] public Image beeIcon;
        [SerializeField] public Button beeInfoButton;
        public BeeSo beeSo;
    }
}