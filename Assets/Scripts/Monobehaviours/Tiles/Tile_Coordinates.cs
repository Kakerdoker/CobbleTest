using UnityEngine;

//A lot of this code is ripped from here: https://www.redblobgames.com/grids/hexagons/
namespace Tile_Coordinates
{

    public struct Axial
    {
        public int q;
        public int r;

        public Axial(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        /// <summary>
        /// Converts Axial coordinates to Worldpsace coordinates.
        /// </summary>
        /// <param name="scale">Scale of the Tiled hexagons</param>
        public Vector3 ToWorldspace(float scale)
        {
            float x = Mathf.Sqrt(3) * q + Mathf.Sqrt(3) / 2 * r;
            float z = 3f / 2f * r;
            return new Vector3(x, 0, z) * scale;
        }

        /// <summary>
        /// Returns the shortest amount of moves needed to move from one set of coordinates to another, moving as if there were no obstacles. 
        /// </summary>
        static public int GetDistance(Axial from, Axial to)
        {
            return (Mathf.Abs(from.q - to.q) + Mathf.Abs(from.q + from.r - to.q - to.r) + Mathf.Abs(from.r - to.r)) / 2;
        }

        /// <summary>
        /// Converts the given <c>Worldspace</c> coordinates and returns them as <c>Axial</c>.
        /// </summary>
        /// <param name="scale">Scale of the Tiled hexagons</param>
        public static Axial WorldspaceToAxial(Vector3 world, float scale)
        {
            float q = (Mathf.Sqrt(3f) / 3f * world.x - 1f / 3f * world.z)/scale;
            float r = (2f / 3f * world.z) / scale;
            return Round(q,r);

        }

        /// <summary>
        /// Rounds the rational Q and R coordinates to integers and returns them as an <c>Axial</c> object.
        /// </summary>
        private static Axial Round(float Q, float R)
        {
            int q = (int)Mathf.Round(Q);
            int r = (int)Mathf.Round(R);
            int s = (int)Mathf.Round(-Q - R);

            int q_diff = (int)Mathf.Abs(q - Q);
            int r_diff = (int)Mathf.Abs(r - R);
            int s_diff = (int)Mathf.Abs(s - (-Q - R));

            if (q_diff > r_diff && q_diff > s_diff)
                q = -r - s;
            else if (r_diff > s_diff)
                r = -q - s;

            return new Axial(q,r);
        }

        public static Axial operator+ (Axial a, Axial b)
       => new Axial(a.q+b.q, a.r+b.r);

    }

}