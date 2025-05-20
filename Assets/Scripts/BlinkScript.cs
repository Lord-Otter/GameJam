using System.Collections;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    public bool blink;
    private SpriteRenderer sr;
    float alphaValue = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blink)
        {
            StartCoroutine(PingObject());
            blink = false;
        }
    }

    private IEnumerator PingObject()
    {
        alphaValue = 1;
        transform.localScale = Vector3.one;
        while (alphaValue >= 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * 1.1f, transform.localScale.y * 1.1f, transform.localScale.z);
            alphaValue -= 0.05f;
            Color tmp = new Color(sr.color.r, sr.color.g, sr.color.b, alphaValue);
            sr.color = tmp;
            Debug.Log(tmp.a);
            yield return new WaitForSeconds(0.05f);
        }
        blink = true;
    }
}

