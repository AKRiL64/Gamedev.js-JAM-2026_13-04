using Hub.Hives;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HoneyAmountEntryUI: MonoBehaviour
    {
        [SerializeField] public Image honeyImage;
        [SerializeField] public TextMeshProUGUI honeyAmount;
        [HideInInspector] public Honey currentHoney;
    }
}