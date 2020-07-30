namespace CodeGenerater.Utility.EasyMacro
{
	public struct MPoint
	{
		#region Constructor
		public MPoint(int X, int Y) : this()
		{
			this.X = X;
			this.Y = Y;
		}
		#endregion

		#region Field
		public int X;

		public int Y;
		#endregion

		#region Method
		public MPoint Offset(int X, int Y)
		{
			return new MPoint(this.X + X, this.Y + Y);
		}
		#endregion
	}
}