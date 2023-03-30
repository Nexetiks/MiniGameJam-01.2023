using UnityEngine;

public class SetInteractiveShaderEffects : MonoBehaviour
{
    [SerializeField]
    private RenderTexture renderTexture;
    [SerializeField]
    private new Camera camera = default;
    [SerializeField]
    private Transform target;

    private void Awake()
    {
        Shader.SetGlobalTexture("_GlobalEffectRT", renderTexture);
        Shader.SetGlobalFloat("_OrthographicCamSize", camera.orthographicSize);
    }

    private void Update()
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 position = transform.position;
        position = new Vector3(targetPosition.x, position.y, targetPosition.z);
        transform.position = position;
        Shader.SetGlobalVector("_Position", position);
    }


}