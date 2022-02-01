using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Solver : MonoBehaviour
{
    private const int Substeps = 4;
    private const float Gravity = -0.2f;
    private List<Point> myPoints;
    private List<Segment> mySegments;
    private bool mySimRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        // Init sim
        myPoints = new List<Point>();
        mySegments = new List<Segment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mySimRunning)
            return;
        this.UpdatePointPositions();
        foreach (var i in Enumerable.Range(0, Solver.Substeps))
            this.ApplyConstraints();
    }

    private void UpdatePointPositions() {
        for (var i = 0; i < myPoints.Count; i++) {
            Point p = myPoints[i];
            if (p.isPinned)
                continue;
            Vector3 velocity = p.position - p.lastPosition;
            p.lastPosition = p.position;
            p.position += velocity;
            p.position += (new Vector3(0, Solver.Gravity, 0)) * Time.deltaTime;
        }
    }

    private void ApplyConstraints() {
        // Randomize constraint application to get better stability.
        System.Random r = new System.Random();
        foreach (var i in Enumerable.Range(0, mySegments.Count).OrderBy(item => r.Next())) {
            Segment s = mySegments[i];
            Point p0 = s.first;
            Point p1 = s.second;
            var dir = p1.position - p0.position;
            float length = dir.magnitude;
            dir.Normalize();
            float res = 0.5f*(length - s.restLength);
            if (!p0.isPinned)
                p0.position += dir*res;
            if (!p1.isPinned)
                p1.position -= dir*res;
        }
    }


    // Public API to add points and segments.
    // TODO: Ability to remove points.
    public void AddPoint(Point p) {
        myPoints.Add(p);
    }

    public void AddSegment(Segment s) {
        mySegments.Add(s);
    }

    public void ToggleSim() {
        mySimRunning = !mySimRunning;
    }

    public void Reset() {
        myPoints.Clear();
        myPoints = new List<Point>();
        mySegments.Clear();
        mySegments = new List<Segment>();
    }
}
