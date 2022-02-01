using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment {
    public Point first;
    public Point second;
    public float restLength;
    public Segment(Point first, Point second, float restLen) {
        this.first = first;
        this.second = second;
        this.restLength = restLen;
    }
}

public class SegmentHandler : MonoBehaviour
{
    
    private Segment mySegment = null;

    private LineRenderer myLineRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var p0 = mySegment.first.position;
        var p1 = mySegment.second.position;
        myLineRenderer.SetPositions(new Vector3[]{p0, p1});
    }

    public void AttachSegment(Segment s) {
        mySegment = s;
    }
}
