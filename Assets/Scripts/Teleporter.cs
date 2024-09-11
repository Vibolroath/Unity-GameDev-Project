using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private Color teleporterColor;

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.materials[0].color = teleporterColor;
    }
}
