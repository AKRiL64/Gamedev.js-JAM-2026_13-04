using UnityEngine;

namespace Hub
{
    public class PlantPotGround: MonoBehaviour
    {
        [SerializeField] private GameObject notWateredGround;
        [SerializeField] private GameObject wateredGround;

        public void SetWatered(bool isWatered)
        {
            if (isWatered)
            {
                wateredGround.SetActive(true);
                notWateredGround.SetActive(false);
            }
            else
            {
                wateredGround.SetActive(false);
                notWateredGround.SetActive(true);
            }
        }
        
        private void Awake()
        {
            SetWatered(false);
        }
    }
    
    
}