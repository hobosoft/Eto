using System;
using System.Linq;
using Eto.Drawing;
using System.Collections.Generic;

namespace Eto.Platform
{
	public static class SplineHelper
	{
		public static IEnumerable<PointF> SplineCurve (IEnumerable<PointF> points, float tension)
		{
			var pointList = points as IList<PointF> ?? points.ToArray ();
			PointF[] pts = null;
			if (pointList.Count == 2) {
				pts = CalculateSplineCurve (pointList[0], pointList[0], pointList[1], pointList[1], tension);
				if (pts != null) {
					for (int j = 0; j < pts.Length; j++)
						yield return pts[j];
				}
			} else {
				for (int i = 0; i < pointList.Count - 1; i++) {
					if (i == 0)
						pts = CalculateSplineCurve (pointList[0], pointList[0], pointList[1], pointList[2], tension);
					else if (i == pointList.Count - 2)
						pts = CalculateSplineCurve (pointList[i - 1], pointList[i], pointList[i + 1], pointList[i + 1], tension);
					else
						pts = CalculateSplineCurve (pointList[i - 1], pointList[i], pointList[i + 1], pointList[i + 2], tension);

					for (int j = 0; j < pts.Length; j++)
						yield return pts[j];
				}
			}
		}

		public static void Draw (IEnumerable<PointF> points, Action<PointF> connectStart, Action<PointF, PointF, PointF> drawBezier)
		{
			var enumerator = points.GetEnumerator ();
			if (!enumerator.MoveNext ())
				return;
			var start = enumerator.Current;
			connectStart(start);

			while (enumerator.MoveNext ()) {
				var control1 = enumerator.Current;
				if (!enumerator.MoveNext ())
					return;
				var control2 = enumerator.Current;
				if (!enumerator.MoveNext ())
					return;
				var end = enumerator.Current;
			
				drawBezier(control1, control2, end);
				start = end;
			}
		}

		static PointF[] CalculateSplineCurve (PointF point0, PointF point1, PointF point2, PointF point3, float tension)
		{
			var points = new PointF[20];
			float SX1 = tension * (point2.X - point0.X);
			float SY1 = tension * (point2.Y - point0.Y);
			float SX2 = tension * (point3.X - point1.X);
			float SY2 = tension * (point3.Y - point1.Y);
			float AX = SX1 + SX2 + 2 * point1.X - 2 * point2.X;
			float AY = SY1 + SY2 + 2 * point1.Y - 2 * point2.Y;
			float BX = -2 * SX1 - SX2 - 3 * point1.X + 3 * point2.X;
			float BY = -2 * SY1 - SY2 - 3 * point1.Y + 3 * point2.Y;
			float CX = SX1;
			float CY = SY1;
			float DX = point1.X;
			float DY = point1.Y;

			for (int i = 0; i < points.Length; i++) {
				float t = (float)i / (points.Length - 1);
				points[i].X = (float)(AX * t * t * t + BX * t * t + CX * t + DX);
				points[i].Y = (float)(AY * t * t * t + BY * t * t + CY * t + DY);
			}

			return points;
		}
	}
}

