namespace Utilities
{
    using UnityEngine;

    public static class VectorExtensions
    {
        public static Vector3 SetMagnitude(this Vector3 v, float newMagnitude)
        {
            v = v.normalized;
            v *= newMagnitude;
            return v;
        }

        /// <summary>
        /// Returns the Vector with one of its values set to 0.
        /// </summary>
        /// <param name="ignoredAxis">
        /// The axis which should be ignored.
        /// </param>
        public static Vector3 Planar(this Vector3 v, Axis ignoredAxis)
        {
            switch (ignoredAxis)
            {
                case Axis.x:
                    v.x = 0f;
                    break;
                case Axis.y:
                    v.y = 0f;
                    break;
                case Axis.z:
                    v.z = 0f;
                    break;
                default:
                    Debug.LogWarning("Invalid Argument given for Axis: " + ignoredAxis.ToString());
                    break;
            }
            return v;
        }

        public static Vector2 SetMagnitude(this Vector2 v, float newMagnitude)
        {
            v = v.normalized;
            v *= newMagnitude;
            return v;
        }

        /// <summary>
        /// Creates a Vector2 using all of the values from the Vector3 given except one.
        /// </summary>
        /// <param name="ignoredAxis">
        /// The axis which should be ignored.
        /// </param>
        /// 
        public static Vector2 ToVector2(this Vector3 v, Axis ignoredAxis)
        {
            Vector2 v2 = Vector2.zero;

            switch (ignoredAxis)
            {
                case Axis.x:
                    v2 = new Vector2(v.y, v.z);
                    break;
                case Axis.y:
                    v2 = new Vector2(v.x, v.z);

                    break;
                case Axis.z:
                    v2 = new Vector2(v.x, v.y);
                    break;
                default:
                    Debug.LogWarning("Invalid Argument given for Axis: " + ignoredAxis.ToString());
                    break;
            }
            return v2;
        }
    }

    public static class TransformExtensions
    {
        /// <summary>
        /// Sets the Transforms position on one axis to a given value
        /// </summary>
        /// <param name="axis">
        /// The axis which is used
        /// </param>
        /// <param name="val">
        /// The value to which the axis is set
        /// </param>
        /// <param name="global">
        /// If the changes should be applied in global space
        /// </param>
        public static void SetAxisPos(this Transform t, Axis axis, float val, bool global = true)
        {
            Vector3 p;
            if (global)
            {
                p = t.position;
            }
            else
            {
                p = t.localPosition;
            }

            switch (axis)
            {
                case Axis.x:
                    p = new Vector3(val, p.y, p.z);
                    break;
                case Axis.y:
                    p = new Vector3(p.x, val, p.z);
                    break;
                case Axis.z:
                    p = new Vector3(p.x, p.y, val);
                    break;
                default:
                    Debug.LogWarning("Invalid Argument given for Axis: " + axis.ToString());
                    break;
            }

            if (global)
            {
                t.position = p;
            }
            else
            {
                t.localPosition = p;
            }
        }

        /// <summary>
        /// Sets the Transforms position on one axis to a given value
        /// </summary>
        /// <param name="axis">
        /// The axis which is used
        /// </param>
        /// <param name="val">
        /// The value to which the axis is set
        /// </param>
        /// <param name="global">
        /// If the changes should be applied in global space
        /// </param>
        public static void SetAxisRot(this Transform t, Axis axis, float val, bool global = true)
        {
            Vector3 r;

            if (global)
            {
                r = t.eulerAngles;
            }
            else
            {
                r = t.localEulerAngles;
            }

            switch (axis)
            {
                case Axis.x:
                    r.x = val;
                    break;
                case Axis.y:
                    r.y = val;
                    break;
                case Axis.z:
                    r.z = val;
                    break;
                default:
                    break;
            }

            if (global)
            {
                t.eulerAngles = r;
            }
            else
            {
                t.localEulerAngles = r;
            }
        }
    }
}

