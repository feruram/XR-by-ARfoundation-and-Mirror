using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class DrawPhysicLine : MonoBehaviour
{
    public Slider R;
    public Slider G;
    public Slider B;
    public Slider Size;
    public GameObject linePrefab;
    public float lineLength = 0.1f;
    public float lineWidth = 0.1f;
    private Vector3 startPos;
    private float length;
    private float width;
    public Vector3 p;
    public TextMeshProUGUI logText;
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        width=lineWidth*Size.value;
        length =lineLength*Size.value;
        p = Camera.main.transform.TransformPoint(0, 0, 0.1f);
        float x = Camera.main.transform.position.x;
        float y = Camera.main.transform.position.y;
        float z = Camera.main.transform.position.z;
        logText.text="( "+x.ToString()+" , "+y.ToString()+" , "+z.ToString()+" )";
        drawLine();
    }
    void drawLine()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            p = Camera.main.transform.TransformPoint(0, 0, 0.1f);

            if (touch.phase == TouchPhase.Began)
            {
                startPos = p;
                
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                Vector3 endPos = p;
                if ((endPos - startPos).magnitude > length)
                {
                    GameObject obj = Instantiate(linePrefab, transform.position, transform.rotation) as GameObject;
                    obj.GetComponent<Renderer>().material.color = new Color(R.value, G.value, B.value, 1.0f);
                    obj.transform.position = (startPos + endPos) / 2;
                    obj.transform.up = (endPos - startPos).normalized;

                    obj.transform.localScale = new Vector3(width,
                                                           (endPos - startPos).magnitude,
                                                           width);
                    obj.transform.parent = this.transform;
                    startPos = endPos;
                }
            }
        }
    }
}
