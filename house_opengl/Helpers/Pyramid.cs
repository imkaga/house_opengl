using System;

namespace Project.Helpers
{
	public class Pyramid
	{
		public Triangle[] triangles { get; set; }

		public Pyramid(Punkt baseCenter, float width, float height, float depth, string texturePath)
		{
			Punkt closeLeft = new Punkt {
				x = baseCenter.x - (width / 2),
				y = baseCenter.y,
				z = baseCenter.z + (depth / 2)
			};
            Punkt farLeft = new Punkt
            {
                x = baseCenter.x - (width / 2),
                y = baseCenter.y,
                z = baseCenter.z - (depth / 2)
            };
            Punkt farRight = new Punkt
            {
                x = baseCenter.x + (width / 2),
                y = baseCenter.y,
                z = baseCenter.z - (depth / 2)
            };
            Punkt closeRight = new Punkt
            {
                x = baseCenter.x + (width / 2),
                y = baseCenter.y,
                z = baseCenter.z + (depth / 2)
            };
            Punkt pinacle = new Punkt
            {
                x = baseCenter.x,
                y = baseCenter.y + height,
                z = baseCenter.z
            };

            triangles = new Triangle[]
            {
                new Triangle(closeLeft, farLeft, pinacle, texturePath),
                new Triangle(farLeft, farRight, pinacle, texturePath),
                new Triangle(farRight, closeRight, pinacle, texturePath),
                new Triangle(closeRight, closeLeft, pinacle, texturePath)
            };
        }
	}
}
