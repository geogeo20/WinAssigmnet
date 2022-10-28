using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private Image image;
    [SerializeField]
    public Sprite back;

    private Sprite face;

    public void Init(Sprite cardVisual)
    {
        face = cardVisual;
        transform.rotation = new Quaternion(0, 180, 0, 0);
        rect.anchoredPosition = Vector3.zero;
    }

    [ContextMenu("Test")]
    public void FlipCard()
    {
        StartCoroutine(FlipCardAnim());
    }

    public float speed = 300;
    private IEnumerator FlipCardAnim()
    {
        while (transform.eulerAngles.y >= 0 && transform.eulerAngles.y <= 180)
        {
            transform.Rotate(0, -speed * Time.deltaTime, 0);
            yield return null;
        }
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (transform.eulerAngles.y < 90 || transform.eulerAngles.y > 270)
        {
            image.sprite = face;
        }
        else
        {
            image.sprite = back;
        }
    }
}