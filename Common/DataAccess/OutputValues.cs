using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PortalToExApi.Common.DataAccess
{
	public class OutputValues
	{
		private int _returnValue;
		private Dictionary<string, IDbDataParameter> _outputParams;

		/// <summary>
		/// 반환 값
		/// </summary>
		public int ReturnValue
		{
			get { return _returnValue; }
			set { _returnValue = value; }
		}
		/// <summary>
		/// outputs
		/// </summary>
		public Dictionary<string, IDbDataParameter> OutputParams
		{
			get { return _outputParams; }
			set { _outputParams = value; }
		}

		/// <summary>
		/// constructor
		/// </summary>
		public OutputValues()
		{
			_outputParams = new Dictionary<string, IDbDataParameter>();
		}
	}
}
