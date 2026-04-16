using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ClosingWindow: MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            closeButton.onClick.AddListener(Close);
            gameObject.SetActive(false);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}