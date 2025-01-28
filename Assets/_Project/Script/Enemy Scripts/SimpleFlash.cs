using System.Collections;
using UnityEngine;

public class SimpleFlash : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;

    public SpriteRenderer sr;
    public Material originalMaterial;
    private Coroutine flashRoutine;

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sr.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        sr.material = originalMaterial;

        flashRoutine = null;
    }
}
