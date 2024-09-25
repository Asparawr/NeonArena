using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopupController : MonoBehaviour
{
    public float dissapearTime;
    public float dissapearSpeed;

    public float downSpeedRange;
    public float upSpeedRange;
    public float leftSpeedRange;
    public float rightSpeedRange;
    public float ySpeed;
    public float xSpeed;

    private TextMeshPro textMesh;
    private Color textColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;
        ySpeed = Random.Range(downSpeedRange, upSpeedRange);
        xSpeed = Random.Range(leftSpeedRange, rightSpeedRange);
    }

    void Update()
    {
        //text fading
        dissapearTime -= Time.deltaTime;
        transform.position += Time.deltaTime * new Vector3(xSpeed, ySpeed);
        if (dissapearTime < 0)
        {
            textColor.a -= Time.deltaTime * dissapearSpeed;
            textMesh.color = textColor;
            if (textColor.a <= 0)
                Destroy(gameObject);
        }

    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }
}
