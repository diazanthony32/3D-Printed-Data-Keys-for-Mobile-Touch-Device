using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoint : MonoBehaviour
{
    private TouchDebugger touchDebugger;

    internal TouchPoint connectedTo;
    internal TouchPoint connectedFrom;

    [SerializeField] private RectTransform touchIndicator;
    [SerializeField] private RectTransform minTouch;
    [SerializeField] private RectTransform maxTouch;

    // Start is called before the first frame update
    void Awake()
    {
        touchDebugger = GetComponentInParent<TouchDebugger>();
    }

    // Update is called once per frame
    void Update()
    {
        FindNearestNeighboringPoint();

        if(connectedTo)
            Debug.DrawLine(this.transform.position, this.connectedTo.transform.position);
    }

    // Adjusts Visuals
    public void AdjustInputRadius(float radius, float radiusVariance)
    {
        float diameter = radius * 2;

        touchIndicator.sizeDelta = new Vector2(diameter, diameter);

        minTouch.sizeDelta = new Vector2(diameter - radiusVariance, diameter - radiusVariance);
        maxTouch.sizeDelta = new Vector2(diameter + radiusVariance, diameter + radiusVariance);

    }

    // what we want is not a line based system, but a polygonal based system...
    void FindNearestNeighboringPoint()
    {
        // Loops through all the Touch Points on the Screen
        foreach (TouchPoint _touchPoint in touchDebugger.touchPoints)
        {
            // Checks if the Touch Point is valid, is not the same one and this one, and not a point its is already connected to
            if (_touchPoint && (_touchPoint != this) && (_touchPoint.connectedTo != this || this.connectedFrom != _touchPoint))
            {
                /* 

                We need to check if:
                - there is an existing connection
                - the new touchpoint is closer than the existing connection
                - the new touchpoint has an already existing connection

                */

                // Sets a default connection if current point doesnt have one, and the new point is not connected from anywhere 
                if (!this.connectedTo && !_touchPoint.connectedFrom)
                {
                    this.connectedTo = _touchPoint;
                    _touchPoint.connectedFrom = this;
                }

                // only runs if its connect to something
                if (this.connectedTo)
                {
                    // Checks distance between currently the new touch point and connected to point
                    float _newDist = Vector2.Distance(this.transform.position, _touchPoint.transform.position);
                    float _toDist = Vector2.Distance(this.transform.position, connectedTo.transform.position);

                    // if the new point is closer than the old connect point, create a new link
                    if (_newDist < _toDist)
                    {
                        connectedTo.connectedFrom = null;

                        this.connectedTo = _touchPoint;
                        _touchPoint.connectedFrom = this;
                    }
                }

            }
        }
    }
}
