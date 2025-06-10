using UnityEngine;

public class LogoBounce : MonoBehaviour
{
    public RectTransform target;
    public float scaleAmount = 0.05f;
    public float moveAmount = 5f;
    public float rotationAmount = 5f;
    public float duration = 5f;

    private Vector2 originalPos;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private float timer;

    void Start()
    {
        if (target == null) target = GetComponent<RectTransform>();
        originalPos = target.anchoredPosition;
        originalScale = target.localScale;
        originalRotation = target.localRotation;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Sin(timer * 2f * Mathf.PI / duration);
        
        target.localScale = originalScale + Vector3.one * t * scaleAmount;
        target.anchoredPosition = originalPos + Vector2.right * t * moveAmount;
        target.localRotation = originalRotation * Quaternion.Euler(0, 0, t * rotationAmount);
    }
}