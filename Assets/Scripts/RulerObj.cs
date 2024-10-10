using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulerObj : MonoBehaviour
{
    [SerializeField] private List<Transform> objList = new List<Transform>();
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform textObj;
    [SerializeField] private TextMesh text;
    public Transform mainCamTransform;

    public void SetInit(Vector3 pos)
    {
        objList[0].transform.position = pos;
        lineRenderer.SetPosition(0,pos);
    }

    public void SetObj(Vector3 pos)
    {
        objList[1].transform.position = pos;
        lineRenderer.SetPosition(1, pos);
    }

    private void Update()
    {
        Vector3 textDirVec = objList[1].position - objList[0].transform.position;
        textObj.position = objList[0].position + textDirVec * 0.5f;

        float textDistance = textDirVec.magnitude;
        string textDistanceText = string.Format($"{textDistance:F2}m");
        text.text = textDistanceText;

        textObj.LookAt(mainCamTransform);
    }
}
