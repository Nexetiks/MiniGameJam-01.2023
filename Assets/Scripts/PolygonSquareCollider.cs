using UnityEngine;

namespace NoGround
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonSquareCollider : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Sizes of the X and Y internal sides")]
        private Vector2 innerSize = Vector2.one * 10f;
        [SerializeField]
        [Tooltip("Thickness of the collider")]
        private float thickness;
        private new PolygonCollider2D collider;

        private void OnValidate()
        {
            if (collider == null)
                FindCollider();

            float minimalSize = 1f;

            if (innerSize.x <= minimalSize)
                innerSize.x = minimalSize;

            if (innerSize.y <= minimalSize)
                innerSize.y = minimalSize;

            float minimalThickness = 0.01f;

            if (thickness <= minimalThickness)
                thickness = minimalThickness;

            RecalculateCollider();
        }

        private void RecalculateCollider()
        {
            // 5 points inside
            // 5 points outside
            int numberOfPoints = 10;

            // Inner points starting from top-right, going clockwise
            var points1 = new Vector2[numberOfPoints];
            points1[0] = new Vector2(innerSize.x / 2, innerSize.y / 2);
            points1[1] = new Vector2(innerSize.x / 2, -innerSize.y / 2);
            points1[2] = new Vector2(-innerSize.x / 2, -innerSize.y / 2);
            points1[3] = new Vector2(-innerSize.x / 2, innerSize.y / 2);
            // Wrap it back to first
            points1[4] = new Vector2(innerSize.x / 2, innerSize.y / 2);

            // Inner points starting from top-right, going counterclockwise
            points1[5] = points1[4] + new Vector2(1f, 1f) * thickness;
            points1[6] = points1[3] + new Vector2(-1f, 1f) * thickness;
            points1[7] = points1[2] + new Vector2(-1f, -1f) * thickness;
            points1[8] = points1[1] + new Vector2(1f, -1f) * thickness;
            points1[9] = points1[0] + new Vector2(1f, 1f) * thickness;

            collider.points = points1;
        }

        private void FindCollider()
        {
            collider = GetComponent<PolygonCollider2D>();

            if (collider == null)
                collider = gameObject.AddComponent<PolygonCollider2D>();
        }
    }
}