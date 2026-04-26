using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hub
{
    public class Exiter: MonoBehaviour
    {
        [SerializeField] private InteractiveObject interactiveObject;
        [SerializeField] private string sceneToLoad;
        [SerializeField] private HubSaveCoordinator saveCoordinator;

        private void Start()
        {
            interactiveObject.OnInteract += LoadNextScene;
        }

        private void LoadNextScene(GameObject player)
        {
            if (saveCoordinator != null)
            {
                saveCoordinator.SaveEntireHub();
            }
            else
            {
                Debug.LogWarning("Save Coordinator is missing! Saving might be incomplete.");
            }

            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveGame();
            }

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}