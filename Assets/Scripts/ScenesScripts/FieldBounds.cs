using UnityEngine;

public class FieldBounds : MonoBehaviour
{
    private Rect _bounds;

    void Awake()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("FieldBounds: SpriteRenderer not found");
            _bounds = new Rect();
            return;
        }

        Bounds b = sr.bounds;
        _bounds = new Rect(
            b.min.x,
            b.min.y,
            b.size.x,
            b.size.y
        );
    }

    public Rect GetBounds() => _bounds;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Rect r = _bounds;
        if (r.width == 0 && r.height == 0)
        {
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                Bounds b = sr.bounds;
                r = new Rect(b.min.x, b.min.y, b.size.x, b.size.y);
            }
        }
        Gizmos.color = Color.cyan;
        Vector3 bl = new Vector3(r.xMin, r.yMin, 0f);
        Vector3 tl = new Vector3(r.xMin, r.yMax, 0f);
        Vector3 tr = new Vector3(r.xMax, r.yMax, 0f);
        Vector3 br = new Vector3(r.xMax, r.yMin, 0f);
        Gizmos.DrawLine(bl, tl);
        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(tr, br);
        Gizmos.DrawLine(br, bl);
    }
#endif
}
