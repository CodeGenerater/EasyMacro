using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MathNet.Numerics;

namespace CodeGenerater.Utility.EasyMacro
{
	//Can't Use
	public class MouseMover
	{
		#region Constructor
		MouseMover()
		{

		}
		#endregion

		#region Field
		static MouseMover Instance;
		#endregion

		#region Property
		public static MouseMover Default
		{
			get
			{
				if (Instance == null)
					Instance = new MouseMover();
				return Instance;
			}
		}

		public int DistanceCalculatingCount
		{
			set;
			get;
		} = 10000;

		public int MovingDistancePerLoop
		{
			set;
			get;
		} = 4;

		public int IntervalPerMove
		{
			set;
			get;
		} = 0;
		#endregion

		#region Method
		public void Move(MPoint[] Points)
		{
			Move(Points, Points.Length);
		}

		public void Move(MPoint[] Points, int Order)
		{
			if (Points.Length > Order)
				throw new ArgumentException(nameof(Order));

			MPoint LastPoint = Points[Points.Length - 1];
			MPoint StartPoint = MacroMouse.Default.Position;
			double[] x_array = new double[Points.Length + 1];
			double[] y_array = new double[Points.Length + 1];
			x_array[0] = StartPoint.X;
			y_array[0] = StartPoint.Y;
			for (int i = 0; i < Points.Length - 1; i++)
			{
				x_array[i + 1] = Points[i].X;
				y_array[i + 1] = Points[i].Y;
			}

			var f = Fit.PolynomialFunc(x_array, y_array, Order);

			double px = x_array[0], py = y_array[0], total_distance = 0;
			double x_delta = (double)(LastPoint.X - StartPoint.X) / DistanceCalculatingCount;
			x_array = new double[DistanceCalculatingCount];
			y_array = new double[DistanceCalculatingCount];
			for (int i = 0; i < DistanceCalculatingCount; i++)
			{
				double x = StartPoint.X + x_delta * i;
				double y = f(x);
				double distance = calcDistance(px, x, py, y);
				total_distance += distance;
				x_array[i] = total_distance;
				y_array[i] = x;
				px = x;
				py = y;
			}

			var g = Fit.PolynomialFunc(x_array, y_array, Order + 1);

			int count = (int)(total_distance / MovingDistancePerLoop);
			for (int i = 0; i < count - 1; i++)
			{
				double d = MovingDistancePerLoop * i;
				double x = g(d);
				double y = f(x);
				MacroMouse.Default.SetPos((int)Math.Round(x), (int)Math.Round(y)).Delay(IntervalPerMove);
			}
			MacroMouse.Default.Position = LastPoint;
		}
		#endregion

		#region Helper
		double calcDistance(double x1, double x2, double y1, double y2)
		{
			return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
		}
		#endregion
	}
}