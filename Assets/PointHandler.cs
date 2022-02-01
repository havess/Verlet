using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point {
    public Vector3 position;
    public Vector3 lastPosition;
    public bool isPinned;
    public int myID;

    public Point(Vector3 pos) {
        this.position = pos;
        this.lastPosition = pos;
    }
}

public class PointHandler : MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;
    private Point myPoint = null;
    private bool myClickPins = false;

    // Start is called before the first frame update
    void Start() {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        // Check if a mouse event should pin this point.
        myClickPins = Input.GetKey(KeyCode.LeftShift);
        transform.position = myPoint.position;
    }

    // Called when the mouse is clicked on the point collider.
    void OnMouseDown() {
        if (myClickPins) {
            if (!myPoint.isPinned)
                mySpriteRenderer.color = new Color(0.25f, 0.25f, 0.25f);
            else
                mySpriteRenderer.color = new Color(0.9f, 0.9f, 0.8f);
            myPoint.isPinned = !myPoint.isPinned;
        }
    }

    void OnMouseOver() {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    void OnMouseExit() {
        transform.localScale = new Vector3(0.25f,0.25f,0.25f);
    }

    public void AttachPoint(Point p) {
        myPoint = p;
    }

    public Point GetPoint() {
        return myPoint;
    }
}
