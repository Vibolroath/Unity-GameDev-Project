using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{

    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;

    [SerializeField] private MeshRenderer cylinderMeshRenderer;


    void Awake()
    {
        cylinderMeshRenderer.materials[0].color = inactiveColor;
    }

    private void LateUpdate()
    {
        if (IsCurrentCheckPoint() == false)
        {
            cylinderMeshRenderer.materials[0].color = inactiveColor;
            GetComponent<Collider>().enabled = true;
        }
    }

    public void UpdateCheckpoint()
    {
        if (IsCurrentCheckPoint() == false)
        {
            GetComponent<Collider>().enabled = false;
            cylinderMeshRenderer.materials[0].color = activeColor;
            GameManager.Instance.Checkpoint(gameObject.transform);
        }
    }

    private bool IsCurrentCheckPoint()  // Check if the checkpoint is currently set in GameManager
    {
        return (transform == GameManager.Instance.LastCheckpoint);
    }
}
