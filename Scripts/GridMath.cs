using UnityEngine;

namespace Grid
{
    public static class GridMath
    {
        public enum DistanceFormula { Manhattan, Euclidean, Chebyshev }

        public static Vector3 ScreenToWorldPos3D(Vector2 inputMousePosition, bool yAxis = false)
        {
            Camera mainCamera = Camera.main;
            Ray ray = mainCamera.ScreenPointToRay(inputMousePosition);

            Plane plane = GetPlaneByAxis(yAxis);

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                return hitPoint;
            }

            return Vector3.zero; // or some other default value
        }

        public static Plane GetPlaneByAxis(bool yAxis = false)
        {

            if (yAxis)
            {
                return new(Vector3.up, Vector3.zero);
            }

            else
            {
                return new(Vector3.forward, Vector3.zero);
            }

        }


        public static float CalculateDistance(Vector2 pos1, Vector2 pos2, DistanceFormula distanceFormula = DistanceFormula.Manhattan)
        {
            return distanceFormula switch
            {
                DistanceFormula.Euclidean => EuclideanDistance(pos1, pos2),
                DistanceFormula.Chebyshev => ChebyshevDistance(pos1, pos2),
                DistanceFormula.Manhattan => ManhattanDistance(pos1, pos2),
                _ => throw new System.NotImplementedException(),
            };
        }

        private static float ManhattanDistance(Vector2 pos1, Vector2 pos2)
        {
            return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        }

        private static float EuclideanDistance(Vector2 pos1, Vector2 pos2)
        {
            float deltaX = pos1.x - pos2.x;
            float deltaY = pos1.y - pos2.y;
            return Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        private static float ChebyshevDistance(Vector2 pos1, Vector2 pos2)
        {
            return Mathf.Max(Mathf.Abs(pos1.x - pos2.x), Mathf.Abs(pos1.y - pos2.y));
        }

        public static Vector2Int Normalize(this Vector2Int vector)
        {
            Vector2 normalizedVector = ((Vector2)vector).normalized;
            return new Vector2Int(Mathf.RoundToInt(normalizedVector.x), Mathf.RoundToInt(normalizedVector.y));
        }

    }
}

