using UnityEngine;
using UnityEngine.UI;


public class Highlighter : MonoBehaviour
{
    [SerializeField] private RawImage _image;

    public void SetColor(Color color) => _image.color = color;

    public void Init(WorldUIContainer container, Transform selectedTransform)
    {
        transform.SetParent(container.ContainerTransform);
        transform.localPosition = new Vector3(0, 0.2f, 0);
        transform.localScale = selectedTransform.lossyScale;
    }
}
