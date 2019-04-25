using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Geometry
    {
        public static (double x, double y) Center(double[][][] coordinates)
        {
            if (coordinates.Length == 0 || coordinates[0].Length == 0) return (0, 0);

            double minX = coordinates[0][0][0];
            double maxX = coordinates[0][0][0];
            double minY = coordinates[0][0][1];
            double maxY = coordinates[0][0][1];

            foreach (var c in coordinates[0]) {
                if (c[0] < minX) minX = c[0];
                if (c[0] > maxX) maxX = c[0];
                if (c[1] < minY) minY = c[1];
                if (c[1] > maxY) maxY = c[1];
            };

            return ((minX + maxX) / 2, (minY + maxY) / 2);
        }

        public static (double x, double y) AverageCenter(double[][][] coordinates)
        {
            if (coordinates.Length == 0 || coordinates[0].Length == 0) return (0, 0);

            double x = 0;
            double y = 0;

            double[][] coords = coordinates[0].Take(coordinates[0].Length - 1).ToArray();

            foreach (var c in coords)
            {
                x += c[0];
                y += c[1];
            };

            return (x / coords.Length, y / coords.Length);
        }
    }
}
