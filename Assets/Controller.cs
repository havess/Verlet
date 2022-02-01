using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject myPointPrefab;
    public GameObject mySegmentPrefab;
    private Solver mySolver;

    private Point mySelection = null;

    void Start() {
        mySolver = GameObject.FindObjectOfType<Solver>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            mySolver.ToggleSim();

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (!hit)
                return;
            
            var name = hit.transform.name;
            if (hit.transform.name == "PointSpawner") {
                this.SpawnPoint();
                return;
            }

            if (Input.GetKey(KeyCode.LeftShift))
                return;

            var pointHandler = hit.transform.gameObject.GetComponent<PointHandler>();
            if (pointHandler)
                this.SpawnSegment(pointHandler.GetPoint());
        }
    }

    private Point SpawnPoint() {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return this.SpawnPoint(pos);
    }

    private Point SpawnPoint(Vector3 pos) {
        // Place the points slightly above the background collider.
        pos.z = -1;
        Point p = new Point(pos);
        mySolver.AddPoint(p);
        GameObject newPoint = Instantiate(myPointPrefab, pos, Quaternion.identity);
        PointHandler pHandler = newPoint.GetComponent<PointHandler>();
        pHandler.AttachPoint(p);

        // Reset the selection.
        mySelection = null;
        return p;
    }

    private Segment SpawnSegment(Point p1, Point p2) {
        var len = (p2.position - p1.position).magnitude;
        var s = new Segment(p1, p2, len);
        mySolver.AddSegment(s);

        GameObject newSegment = Instantiate(mySegmentPrefab, p1.position, Quaternion.identity);
        SegmentHandler sHandler = newSegment.GetComponent<SegmentHandler>();
        sHandler.AttachSegment(s);
        return s;
    }

    private Segment SpawnSegment(Point p) {
        if (mySelection == null) {
            mySelection = p;
            return null;
        }
        var s = SpawnSegment(mySelection, p);
        mySelection = s.second;
        return s;
    }

    public void SpawnGrid() {
        this.Clear();
        const float half_width = 12.0f;
        const float half_height = 8.0f;
        const float hSep = 1.0f;
        const float vSep = 1.0f;
        float h = -half_height;
        List<Point> points = new List<Point>();
        while (h < half_height) {
            float w = -half_width;
            while (w < half_width) {
                points.Add(this.SpawnPoint(new Vector3(w, h, -1)));
                if (w > -half_width)
                    this.SpawnSegment(points[points.Count - 2], points[points.Count - 1]);
                w += hSep;
            }
            h += vSep;

            int ptsInRow = (int) (2.0f*half_width/hSep);
            if (points.Count < 2*ptsInRow)
                continue;
            for (int i = 0; i < ptsInRow; i++) {
                var p0 = points[points.Count - 2*ptsInRow + i];
                var p1 = points[points.Count - ptsInRow + i];
                this.SpawnSegment(p0, p1);
            }
        }
    }

    public void SpawnPendant() {
        this.Clear();
        Point anchor = this.SpawnPoint(new Vector3(0, 8.0f, -1));
        anchor.isPinned = true;
        Point last = anchor;
        List<Point> pts = new List<Point>();
        for (int i = 0; i < 10; i += 2) {
            var p = this.SpawnPoint(new Vector3((float) i, 8.0f, -1));
            this.SpawnSegment(last, p);
            last = p;
            pts.Add(p);
        }
        Vector3 center = last.position + new Vector3(4.0f, 0.0f, 0.0f);
        int count = pts.Count; 
        int nCircPts = 16;
        for (float a = 0.0f; a < Mathf.PI*2.0f; a += Mathf.PI/(nCircPts*0.5f)) {
            var pos = new Vector3(Mathf.Cos(a), Mathf.Sin(a), -1);
            pts.Add(this.SpawnPoint(center + 2.0f*pos));
        }

        last = pts[count];
        for (int i = count + 1; i < pts.Count; i++) {
            this.SpawnSegment(last, pts[i]);
            last = pts[i];
        }
        for (int i = 0; i < nCircPts/2; i++)
            this.SpawnSegment(pts[count + i], pts[count + i + nCircPts/2]);
        this.SpawnSegment(pts[pts.Count - 1], pts[count]);
        this.SpawnSegment(pts[count - 1], pts[count + nCircPts/2]);
    }

    public void Clear() {
        foreach(var obj in GameObject.FindObjectsOfType<PointHandler>())
            Destroy(obj.gameObject);
        foreach(var obj in GameObject.FindObjectsOfType<SegmentHandler>())
            Destroy(obj.gameObject);
        mySolver.Reset();
    }
}
