using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
	public class Point3d
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
	}

	public class Outline
	{
		public Point3d Min { get; set; }
		public Point3d Max { get; set; }
	}

	public class Line
	{
		public Point3d Start { get; set; }
		public Point3d End { get; set; }
	}

	public enum PlaceDirection : byte
	{
		Left, Right, Top, Bottom
	}

	internal class ElementPlacer
	{
		private int distance = 1;
		private Outline targetPositionTest; // TODO: Delete after testing

		public ElementPlacer() { }

		public Outline GetTargetPositionTestVisual() => targetPositionTest; // TODO: Delete after testing

		public PlaceDirection ChoosePlaceDirection(Outline markedElementOutline, Outline targetElementOutline,
			ICollection<Outline> anotherObjectOutlines, ICollection<Line> anotherObjectLines)
		{
			var potentialPositions = new Dictionary<PlaceDirection, Outline>
		{
			{ PlaceDirection.Right, GetOutlineAtPosition(markedElementOutline, targetElementOutline, PlaceDirection.Right) },
			{ PlaceDirection.Bottom, GetOutlineAtPosition(markedElementOutline, targetElementOutline, PlaceDirection.Bottom) },
			{ PlaceDirection.Left, GetOutlineAtPosition(markedElementOutline, targetElementOutline, PlaceDirection.Left) },
			{ PlaceDirection.Top, GetOutlineAtPosition(markedElementOutline, targetElementOutline, PlaceDirection.Top) }
		};

			foreach (var position in new[] { PlaceDirection.Right, PlaceDirection.Bottom, PlaceDirection.Left, PlaceDirection.Top })
			{
				var targetPosition = potentialPositions[position];
				targetPositionTest = potentialPositions[position]; // TODO: Delete after testing
				if (!IsIntersecting(targetPosition, anotherObjectOutlines, anotherObjectLines))
					return position;
			}

			return PlaceDirection.Bottom;
		}

		private Outline GetOutlineAtPosition(Outline markedElement, Outline targetElement, PlaceDirection direction)
		{
			double width = targetElement.Max.X - targetElement.Min.X;
			double height = targetElement.Max.Y - targetElement.Min.Y;

			switch (direction)
			{
				case PlaceDirection.Right:
					return new Outline
					{
						Min = new Point3d { X = markedElement.Max.X + distance, Y = markedElement.Min.Y, Z = 0 },
						Max = new Point3d { X = markedElement.Max.X + distance + width, Y = markedElement.Min.Y + height, Z = 0 }
					};
				case PlaceDirection.Bottom:
					return new Outline
					{
						Min = new Point3d { X = markedElement.Min.X, Y = markedElement.Max.Y + distance, Z = 0 },
						Max = new Point3d { X = markedElement.Min.X + width, Y = markedElement.Max.Y + distance + height, Z = 0 }
					};
				case PlaceDirection.Left:
					return new Outline
					{
						Min = new Point3d { X = markedElement.Min.X - distance - width, Y = markedElement.Min.Y, Z = 0 },
						Max = new Point3d { X = markedElement.Min.X - distance, Y = markedElement.Min.Y + height, Z = 0 }
					};
				case PlaceDirection.Top:
					return new Outline
					{
						Min = new Point3d { X = markedElement.Min.X, Y = markedElement.Min.Y - distance - height, Z = 0 },
						Max = new Point3d { X = markedElement.Min.X + width, Y = markedElement.Min.Y - distance, Z = 0 }
					};
				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		private bool IsIntersecting(Outline targetOutline, ICollection<Outline> outlines, ICollection<Line> lines)
		{
			foreach (var outline in outlines)
				if (IsIntersecting(targetOutline, outline))
					return true;

			foreach (var line in lines)
				if (IsIntersecting(targetOutline, line))
					return true;

			return false;
		}

		private bool IsIntersecting(Outline a, Outline b) => !(a.Max.X < b.Min.X || a.Min.X > b.Max.X || a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y);

		private bool IsIntersecting(Outline outline, Line line)
		{
			if (line.Start.Y == line.End.Y)
			{
				double y = line.Start.Y;
				return y >= outline.Min.Y && y <= outline.Max.Y && (line.Start.X <= outline.Max.X && line.End.X >= outline.Min.X);
			}
			else if (line.Start.X == line.End.X) 
			{
				double x = line.Start.X;
				return x >= outline.Min.X && x <= outline.Max.X && (line.Start.Y <= outline.Max.Y && line.End.Y >= outline.Min.Y);
			}
			return false;
		}
	}
}