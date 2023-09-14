using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaterFlowAnimation : MonoBehaviour
{ 
    [SerializeField] float _scrollSpeed;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        float x = Mathf.Repeat(Time.time * _scrollSpeed, 1);
        Vector2 offset = new Vector2(x, 0);
        _spriteRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}