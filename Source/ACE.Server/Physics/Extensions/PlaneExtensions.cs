using System;
using System.Numerics;
using ACE.Server.Physics.BSP;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Extensions
{
    public static class PlaneExtensions
    {
        public static Plane LocalToGlobal(this Plane plane, Position to, Position from, Plane localPlane)
        {
            var normal = from.Frame.LocalToGlobalVec(localPlane.Normal);
            var dist = to.LocalToGlobal(from, localPlane.Normal * -localPlane.D);

            return new Plane(normal, -Vector3.Dot(normal, dist));
        }

        public static Side GetSide(this Plane plane, Vector3 p, float bias = 0.0f)
        {
            var dist = Vector3.Dot(plane.Normal, p) + plane.D + bias;

            Side side = Side.Front;
            if (dist <= PhysicsGlobals.EPSILON)
                side = dist < -PhysicsGlobals.EPSILON ? Side.Behind : Side.Close;

            return side;
        }

        public static void SnapToPlane(this Plane p, ref Vector3 offset)
        {
            if (Math.Abs(p.Normal.Z) <= PhysicsGlobals.EPSILON)
                return;

            offset.Z = -(offset.Dot2D(p.Normal) + p.D) * (1.0f / p.Normal.Z) - 1.0f / p.Normal.Z * -p.D;
        }

        public static bool compute_time_of_intersection(this Plane p, Ray ray, ref float time)
        {
            var angle = Vector3.Dot(ray.Dir, p.Normal);
            if (Math.Abs(angle) < PhysicsGlobals.EPSILON)
                return false;

            time = (Vector3.Dot(ray.Point, p.Normal) + p.D) * (-1.0f / angle);
            return time >= 0.0f;
        }
    }
}
