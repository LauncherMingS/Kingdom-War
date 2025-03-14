using System.Collections;
using UnityEngine;

public class AttackedEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown()
    {
        StartCoroutine(hitFalsh());
    }
    IEnumerator hitFalsh()
    {
        spriteRenderer.color = new Color32(255, 150, 150, 255);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = new Color32(255, 255, 255, 255);
    }
    //public Material attackedMaterial;

    //public Material originalMaterial;
    //public Renderer _renderer;

    //private void Start()
    //{
    //    _renderer = GetComponent<Renderer>();
    //    originalMaterial = _renderer.material;
    //}
    //private void OnMouseDown()
    //{
    //    StartCoroutine(hitFlash());
    //}
    //public void ApplyAttackedEffect()
    //{
    //    _renderer.material = attackedMaterial;
    //}
    //public void RemoveAttackedEffect()
    //{
    //    _renderer.material = originalMaterial;
    //}
    //IEnumerator hitFlash()
    //{
    //    _renderer.material = attackedMaterial;
    //    yield return new WaitForSeconds(0.1f);
    //    _renderer.material = originalMaterial;
    //}
}