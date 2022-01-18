using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarousellController : MonoBehaviour
{
    public RectTransform contentRect; 
    public List<RectTransform> templateButtons = new List<RectTransform>();
    public RectTransform center;

    private float[] distancesFromCenter;
    private float[] distancesforLoop;
    private bool dragging = false;
    private int distanceFromButton;
    private int closestToCenterPosition;

    private void Start()
    {
        distancesforLoop = new float[templateButtons.Count];

        distancesFromCenter = new float[templateButtons.Count];

        distanceFromButton = (int)Mathf.Abs(templateButtons[1].anchoredPosition.x - templateButtons[0].anchoredPosition.x);
    }    
    
    private void Update()
    {
        for (int i = 0; i < templateButtons.Count; i++)
        {
            distancesforLoop[i] = center.position.x - templateButtons[i].position.x;
            distancesFromCenter[i] = Mathf.Abs(distancesforLoop[i]);

            if(distancesforLoop[i] > 4100)
            {
                float currentX = templateButtons[i].anchoredPosition.x;

                Vector2 newPosition = new Vector2(currentX + (templateButtons.Count * distanceFromButton), templateButtons[i].anchoredPosition.y);
                templateButtons[i].anchoredPosition = newPosition;
            }

            if (distancesforLoop[i] < -4100)
            {
                float currentX = templateButtons[i].anchoredPosition.x;

                Vector2 newPosition = new Vector2(currentX - (templateButtons.Count * distanceFromButton), templateButtons[i].anchoredPosition.y);
                templateButtons[i].anchoredPosition = newPosition;
            }
        }

        float minDistance = Mathf.Min(distancesFromCenter);

        for (int i = 0; i < templateButtons.Count; i++)
            if(minDistance == distancesFromCenter[i])
            {
                closestToCenterPosition = i;
                templateButtons[i].transform.localScale = Vector2.Lerp(templateButtons[i].transform.localScale, new Vector2(1f, 1f), 0.1f);
            }
            else
                templateButtons[i].transform.localScale = Vector2.Lerp(templateButtons[i].transform.localScale, new Vector2(0.8f, 0.8f), 0.1f);


        if (!dragging)
            ClipToCenter(-templateButtons[closestToCenterPosition].anchoredPosition.x);

    }

    public void ClipToCenter(float position)
    {
        float positionX = Mathf.Lerp(contentRect.anchoredPosition.x, position, Time.deltaTime * 10);

        contentRect.anchoredPosition = new Vector2(positionX, contentRect.anchoredPosition.y);
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void StopDrag()
    {
        dragging = false;
    }
}
