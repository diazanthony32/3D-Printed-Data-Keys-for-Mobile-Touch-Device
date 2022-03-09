using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDebugger : MonoBehaviour
{
    public List<TouchPoint> touchPoints = new List<TouchPoint>();

    public GameObject touchIndicator;

    public Vector2 originPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // When the player has something touching the screen
        if (Input.touchCount > 0)
        {
            foreach (Touch _touch in Input.touches)
            {
                Vector3 _pos = _touch.position;

                // Handle finger movements based on TouchPhase
                switch (_touch.phase)
                {
                    // When a touch has first been detected, spawn an indicator of the touch where the touch is indicated
                    case TouchPhase.Began:
                        // Check if there is an already existing list to prevent multiple additions to the list
                        if ((touchPoints.Count > 0) && (touchPoints.Count > _touch.fingerId))
                            touchPoints[_touch.fingerId] = Instantiate(touchIndicator, _pos, Quaternion.Euler(0, 0, 0), this.transform).GetComponent<TouchPoint>();
                        else
                            touchPoints.Insert(_touch.fingerId, (Instantiate(touchIndicator, _pos, Quaternion.Euler(0, 0, 0), this.transform)).GetComponent<TouchPoint>());

                        // Renames gameobject for debugging purposes
                        touchPoints[_touch.fingerId].name = "Touch " + _touch.fingerId;

                        // changing the visual indicator of the touch to match what is estimated by unity
                        touchPoints[_touch.fingerId].AdjustInputRadius(_touch.radius, _touch.radiusVariance);

                        break;

                    // Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        // Determine direction by comparing the current touch position with the initial one
                        touchPoints[_touch.fingerId].transform.position = _pos;

                        // changing the visual indicator of the touch to match what is estimated by unity
                        touchPoints[_touch.fingerId].AdjustInputRadius(_touch.radius, _touch.radiusVariance);

                        break;

                    // When the touch is done/lifted, remove the instance of the gameobject
                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        GameObject.Destroy(touchPoints[_touch.fingerId]);
                        touchPoints[_touch.fingerId] = null;

                        break;
                }
            }
        }

        // When there is no longer anything touching the screen
        else if ((Input.touchCount <= 0) && (transform.childCount > 0 || touchPoints.Count > 0))
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            touchPoints.Clear();
        }

    }

    Vector2 FindOriginPoint()
    {
        Vector2 originPoint = new Vector2(0, 0);

        foreach (TouchPoint touchPoint in touchPoints)
        {
            originPoint.x += touchPoint.transform.position.x;
            originPoint.y += touchPoint.transform.position.y;
        }

        originPoint /= touchPoints.Count;

        return originPoint;
    }

}
