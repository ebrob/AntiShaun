#region

using System;

#endregion

namespace AntiShaun
{
	public class OdfMetadata
	{
		private readonly Type _type;

		public OdfMetadata(Type type)
		{
			_type = type;
		}


		public virtual Type Type
		{
			get { return _type; }
		}
	}
}