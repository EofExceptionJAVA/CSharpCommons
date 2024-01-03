using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PortalToExApi.Common.DataAccess
{
    public class DBHelper
    {
        public static T ExecuteResult<T>(string connectionString, string procedureNmae, params object[] parameters) where T : new() {
            OutputValues output = new OutputValues();
            return ExecuteResult<T>(connectionString, procedureNmae, ref output, parameters);
        }
        public static T ExecuteResult<T>(string connectionString, string procedureName, ref OutputValues output, params object[] parameters) where T : new()
        {
            return ExecuteResult<T>(connectionString, null, procedureName, ref output, parameters);
        }
        public static List<T> ExecuteResults<T>(string connectionString, string procedureNmae, params object[] parameters) where T : new()
        {
            OutputValues output = new OutputValues();
            return ExecuteResults<T>(connectionString, procedureNmae, ref output, parameters);
        }
        public static List<T> ExecuteResults<T>(string connectionString, string procedureName, ref OutputValues output, params object[] parameters) where T : new()
        {
            return ExecuteResults<T>(connectionString, null, procedureName, ref output, parameters);
        }
        public static int ExecuteNonQuery(string connectionString, string procedureName, ref OutputValues output, params object[] parameters) {
            return ExecuteNonQuery(connectionString, null,procedureName, ref output, parameters);
        }
        public static T ExecuteResultQuery<T>(string connectionString, string quertString) where T : new()
        {
            OutputValues output = new OutputValues();
            return ExecuteResultQuery<T>(connectionString, quertString, ref output);
        }
        public static T ExecuteResultQuery<T>(string connectionString, string procedureName, ref OutputValues output) where T : new()
        {
            return ExecuteResultQuery<T>(connectionString, null, procedureName);
        }

        public static List<T> ExecuteResultsQuery<T>(string connectionString, string quertString) where T : new()
        {
            OutputValues output = new OutputValues();
            return ExecuteResultsQuery<T>(connectionString, quertString, ref output);
        }
        public static List<T> ExecuteResultsQuery<T>(string connectionString, string procedureName, ref OutputValues output) where T : new()
        {
            return ExecuteResultsQuery<T>(connectionString, null, procedureName);
        }
        public static int ExecuteNonQuery(string connectionString, int? commandTimeout, string procedureName, ref OutputValues output, params object[] parameters) {
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = procedureName;
                    command.Connection.Open();
                    SqlCommandBuilder.DeriveParameters(command);
                    if (parameters != null)
                    {
                        for (int i = 1, j = parameters.Length; i <= j; i++)
                        {
                            object paramValue = parameters[i - 1];
                            if (paramValue == null)
                            {
                                paramValue = DBNull.Value;
                            }

                            if (paramValue is DateTime)
                            {
                                DateTime dt = (DateTime)paramValue;
                                if (dt == DateTime.MinValue)
                                {
                                    paramValue = DBNull.Value;
                                }
                            }
                            command.Parameters[i].Value = paramValue;
                        }
                        

                    }
                    
                    result = command.ExecuteNonQuery();
                    command.Connection.Close();
                    output = GetOutputValues(command);
                }
            }
            catch (Exception ex) { }
                
                return 0;
        }
        private static OutputValues GetOutputValues(SqlCommand command)
        {
            OutputValues output = new OutputValues();

            // check parameters
            for (int i = 0, j = command.Parameters.Count; i < j; i++)
            {
                if (command.Parameters[i].Direction == ParameterDirection.ReturnValue)
                {
                    output.ReturnValue = Convert.ToInt32(command.Parameters[0].Value);
                    continue;
                }

                if ((command.Parameters[i].Direction == ParameterDirection.InputOutput)
                    || (command.Parameters[i].Direction == ParameterDirection.Output))
                {
                    output.OutputParams.Add(command.Parameters[i].ParameterName, command.Parameters[i]);
                }
            }

            return output;
        }


        public static T ExecuteResult<T>(string connectionString, int? commandTimeout, string procedureName, ref OutputValues output, params object[] parameters) where T : new() {
            T item = new T();
            try {
                using (SqlConnection conn = new SqlConnection(connectionString)) {
                    SqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = procedureName;
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    SqlCommandBuilder.DeriveParameters(command);
                    if (parameters != null) {
                        for (int i = 1, j = parameters.Length; i <= j; i++) {
                            object paramValue = parameters[i - 1];
                            if (paramValue == null) {
                                paramValue = DBNull.Value;
                            }

                            if (paramValue is DateTime) {
                                DateTime dt = (DateTime)paramValue;
                                if (dt == DateTime.MinValue) {
                                    paramValue = DBNull.Value;
                                }
                            }
                            command.Parameters[i].Value = paramValue;
                        }
                        
                    }
                    

                    

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    item = ConvertDataSetToType<T>(ds).FirstOrDefault();
                }
            }
            catch (Exception ex) { }

            return item;
        }
        public static List<T> ExecuteResults<T>(string connectionString, int? commandTimeout, string procedureName, ref OutputValues output, params object[] parameters) where T : new()
        {
            List<T> item = new List<T>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = procedureName;
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    SqlCommandBuilder.DeriveParameters(command);
                    if (parameters != null)
                    {
                        for (int i = 1, j = parameters.Length; i <= j; i++)
                        {
                            object paramValue = parameters[i - 1];
                            if (paramValue == null)
                            {
                                paramValue = DBNull.Value;
                            }

                            if (paramValue is DateTime)
                            {
                                DateTime dt = (DateTime)paramValue;
                                if (dt == DateTime.MinValue)
                                {
                                    paramValue = DBNull.Value;
                                }
                            }
                            command.Parameters[i].Value = paramValue;
                        }

                    }




                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    item = ConvertDataSetToType<T>(ds);
                }
            }
            catch (Exception ex) { }

            return item;
        }
        public static List<T> ConvertDataSetToType<T>(DataSet dataSet) where T : new()
        {
            if (dataSet == null || dataSet.Tables.Count == 0)
                return new List<T>();

            DataTable dataTable = dataSet.Tables[0];

            List<T> resultList = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T item = new T();

                foreach (DataColumn col in dataTable.Columns)
                {
                    // 각 열의 이름을 속성으로 가정하고 해당 속성이 있다면 값을 설정
                    var property = typeof(T).GetProperty(col.ColumnName);
                    if (property != null)
                    {
                        object value = row[col];

                        // 값이 DBNull.Value이면 null로 설정, 그렇지 않으면 그대로 사용
                        object convertedValue = value == DBNull.Value ? null : value;

                        // Nullable 타입인지 확인
                        var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null;

                        // 형식이 다르면 변환 시도
                        if (isNullable)
                        {
                            // Nullable 타입일 경우
                            if (convertedValue != null && Nullable.GetUnderlyingType(property.PropertyType) != convertedValue.GetType())
                            {
                                // 값이 null이 아니고, 형식이 다르면 변환 시도
                                convertedValue = Convert.ChangeType(convertedValue, Nullable.GetUnderlyingType(property.PropertyType));
                            }
                        }
                        else
                        {
                            // Nullable 타입이 아닐 경우
                            if (convertedValue != null && property.PropertyType != convertedValue.GetType())
                            {
                                // 값이 null이 아니고, 형식이 다르면 변환 시도
                                convertedValue = Convert.ChangeType(convertedValue, property.PropertyType);
                            }
                        }

                        property.SetValue(item, convertedValue);
                    }
                }

                resultList.Add(item);
            }

            return resultList;
        }
        public static T ExecuteResultQuery<T>(string connectionString, int? commandTimeout, string quertString) where T : new()
        {
            T item = new T();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = quertString;
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    item = ConvertDataSetToType<T>(ds).FirstOrDefault();
                }
            }
            catch (Exception ex) { }

            return item;
        }
        public static List<T> ExecuteResultsQuery<T>(string connectionString, int? commandTimeout, string quertString) where T : new()
        {
            List<T> item = new List<T>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = conn.CreateCommand();
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = quertString;
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();

                    da.Fill(ds);
                    item = ConvertDataSetToType<T>(ds);
                }
            }
            catch (Exception ex) { }

            return item;
        }

    }
}
