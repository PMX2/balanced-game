using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    private Camera sceneCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            // On désactive les components des autres joueurs sur notre instance
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera !=null)
            {
                sceneCamera.gameObject.SetActive(false);
            }           
        }
    }

    private void OnDisable()
    {
        if (sceneCamera !=null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}