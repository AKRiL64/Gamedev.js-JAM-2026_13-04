using System;
using UI;
using UnityEngine;

namespace Hub
{
    public class PlantPot: MonoBehaviour
    {
        public event Action<PlantSo> OnPlantChanged;
        
        [SerializeField] private Transform potTopPoint;
        [SerializeField] private InteractiveObject interactiveObject;
        [SerializeField] private GameObject closingWindow;
        private PlantSo plantSo;

        public void ChangePlant(PlantSo newPlantSo)
        {
            plantSo = newPlantSo;
            ClearTopPoint();
            
            if (newPlantSo != null)
            {
                UpdatePlant();
            }
            OnPlantChanged?.Invoke(plantSo);
        }

        public void GrowPlant()
        {
            if (plantSo == null) return;
            plantSo.GrowByOne();
        }

        private void UpdatePlant()
        {
            GameObject plant = Instantiate(plantSo.IsGrown ? plantSo.notGrownPlantPrefab : plantSo.grownPlantPrefab,
                potTopPoint.position, potTopPoint.rotation);
            plant.transform.SetParent(potTopPoint);
        }

        private void ClearTopPoint()
        {
            foreach (Transform child in potTopPoint)
            {
                Destroy(child.gameObject);
            }
        }

        private void ShowUi(GameObject player)
        {
            closingWindow.SetActive(true);
        }

        private void Start()
        {
            interactiveObject.OnInteract += ShowUi;
        }
    }
}