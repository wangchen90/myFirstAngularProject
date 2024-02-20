using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace BAL.Configurations
{
    public class MySqlConnection : AppConfiguration, IDisposable
    {
        // added by priyam
        // modified by Gautam
        public string DbConnection { get { return _connectionString; } }
        private IntPtr handle;  // Pointer to an external unmanaged resource.
        public MySql.Data.MySqlClient.MySqlConnection _Connection; // Other managed resource this class uses.
        private bool disposed = false;  // Track whether Dispose has been called.
        public MySqlConnection()
        {
            _Connection = new MySql.Data.MySqlClient.MySqlConnection
            {
                ConnectionString = DbConnection
            };
            _Connection.Open(); // Modified by Gautam

        }
         
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Close()
        {
            // Calls the Dispose method without parameters.
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_Connection != null)
                    {
                        if (_Connection.State == System.Data.ConnectionState.Open)
                            _Connection.Close();
                        _Connection.Dispose();
                    }
                }
                CloseHandle(handle);
                handle = IntPtr.Zero;
            }
            disposed = true;
        }

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        ~MySqlConnection()
        {
            Dispose(true);
        }
    }

    public class MySqlCommand : IDisposable
    {
        private IntPtr handle; // Pointer to an external unmanaged resource.
        private readonly MySql.Data.MySqlClient.MySqlConnection _Connection;      // Other managed resource this class uses.
        private MySql.Data.MySqlClient.MySqlDataAdapter _MyDataAdaptor;
        private readonly MySql.Data.MySqlClient.MySqlCommand _sqlCommand;
        private bool disposed = false;  // Track whether Dispose has been called.

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public MySqlCommand(MySql.Data.MySqlClient.MySqlConnection MyConnection)
        {
            _Connection = MyConnection;
            _sqlCommand = new MySql.Data.MySqlClient.MySqlCommand
            {
                Connection = _Connection
            };
        }
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    if (_sqlCommand != null)
                    {
                        if (_sqlCommand.Parameters.Count > 0)
                            _sqlCommand.Parameters.Clear();

                        _sqlCommand.Dispose();
                    }
                    if (_MyDataAdaptor != null)
                    {
                        _MyDataAdaptor.Dispose();
                    }
                }
                // Release unmanaged resources. If disposing is false, 
                // only the following code is executed.
                CloseHandle(handle);
                handle = System.IntPtr.Zero;
                // Note that this is not thread safe.
                // Another thread could start disposing the object
                // after the managed resources are disposed,
                // but before the disposed flag is set to true.
                // If thread safety is necessary, it must be
                // implemented by the client.
            }
            disposed = true;
        }
        // Use interop to call the method necessary  
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~MySqlCommand()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        // Do not make this method virtual.
        // A derived class should not be allowed
        // to override this method.
        public void Close()
        {
            // Calls the Dispose method without parameters.
            Dispose();
        }
      
      



        /// <summary>
        /// Add a Parameter to current command object
        /// </summary>
        /// <param name="ParameterName">Parameter Name</param>
        /// <param name="Value">Parameter Value</param>
        public void Add_Parameter_WithValue(string ParameterName, object Value)
        {
            try
            {
                if (ParameterName.IndexOf("@") < 0)
                    ParameterName = "@" + ParameterName;
                _sqlCommand.Parameters.AddWithValue(ParameterName, Value);
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new Exception(SqEx.Message);
            }
            catch (System.Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Add a Parameter to current command object
        /// </summary>
        /// <param name="ParameterName">Parameter Name</param>
        /// <param name="DataType">Parameter Data Type</param>
        /// <param name="Value">Parameter Value</param>
        public void Add_Parameter(string ParameterName, MySql.Data.MySqlClient.MySqlDbType DataType, object Value)
        {
            try
            {
                if (ParameterName.IndexOf("@") < 0)
                    ParameterName = "@" + ParameterName;
                _sqlCommand.Parameters.Add(ParameterName, DataType).Value = Value;
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new Exception(SqEx.Message);
            }
            catch (System.Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Add a query or commandtext to current command object
        /// </summary>
        /// <param name="CommandText">Pass Command Text here </param>
        public void Add_CommandText(string CommandText)
        {
            try
            {
                if (CommandText != null)
                    _sqlCommand.CommandText = CommandText;
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new Exception(SqEx.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }


        /// <summary>
        /// Clears parameter from current command object
        /// </summary>

        public bool Clear_CommandParameter()
        {
            try
            {
                if (_sqlCommand.Parameters.Count > 0)
                    _sqlCommand.Parameters.Clear();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new Exception(SqEx.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Add a transaction locktype to the current commention
        /// </summary>
        /// <param name="MyTransaction">Pass Transaction object </param>

        public void Add_Transaction(MySql.Data.MySqlClient.MySqlTransaction MyTransaction)
        {
            try
            {
                _sqlCommand.Transaction = MyTransaction;
            }
            catch (MySql.Data.MySqlClient.MySqlException Sqex)
            {
                throw new Exception(Sqex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Execute query in the databaxse
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or 
        /// StoredProcedure if you passed name of Stored Procedure as Query</param>
        public async Task<bool> Execute_Query_WithTransaction(string Query, System.Data.CommandType CmdType,
            MySql.Data.MySqlClient.MySqlTransaction MyTransaction,
            bool UseTransaction)
        {
            try
            {
               
                _sqlCommand.CommandText = Query;
                _sqlCommand.CommandType = CmdType;
                if (UseTransaction == true)
                    _sqlCommand.Transaction = MyTransaction;
                _sqlCommand.CommandTimeout = 0;
                 _sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }


        /// <summary>
        /// Execute query in the databaxse
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or 
        /// StoredProcedure if you passed name of Stored Procedure as Query</param>
        public async Task<bool> Execute_Query(string Query, System.Data.CommandType CommandType)
        {
            try
            {
               
                _sqlCommand.CommandText = Query;
                _sqlCommand.CommandType = CommandType;
                _sqlCommand.CommandTimeout = 0;
                var _result =  _sqlCommand.ExecuteNonQuery();
                return _result > 0;
            }
            catch (System.Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }


        /// <summary>
        /// returns single string value from database
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or StoredProcedure if you passed name of Stored Procedure as Query</param>

        public async Task<string> Select_Scalar(string Query, System.Data.CommandType CommandType)
        {
            try
            {
                
                _sqlCommand.CommandText = Query;
                _sqlCommand.CommandType = CommandType;
                _sqlCommand.CommandTimeout = 0;
                return Convert.ToString(_sqlCommand.ExecuteScalar());
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// returns single datatable from database
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or StoredProcedure if you passed name of Stored Procedure as Query</param>

        public async Task<DataTable> Select_Table(string Query, System.Data.CommandType CommandType)
        {
            DataTable DT = new DataTable();
            try
            {
                _sqlCommand.CommandText = Query;
                _sqlCommand.CommandType = CommandType;
                _sqlCommand.CommandTimeout = 0;

                _MyDataAdaptor = new MySql.Data.MySqlClient.MySqlDataAdapter(_sqlCommand);
                await _MyDataAdaptor.FillAsync(DT);
                return DT;
            }
            catch (System.Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
        /// <summary>
        /// returns one or more datatable from database
        /// </summary>
        /// <param name="Query">Write either SQl Select statment or name  of Stored Procedure </param>
        /// <param name="CommandType">Specify command type as Text if you passed Text as Query or StoredProcedure if you passed name of Stored Procedure as Query</param>

        public async Task<System.Data.DataSet> Select_TableSet(string Query, System.Data.CommandType CommandType)
        {

            DataSet DS = new DataSet();
            try
            {
                
                _sqlCommand.CommandText = Query;
                _sqlCommand.CommandType = CommandType;
                _sqlCommand.CommandTimeout = 0;

                _MyDataAdaptor = new MySql.Data.MySqlClient.MySqlDataAdapter(_sqlCommand);
                await _MyDataAdaptor.FillAsync(DS);
                return DS;
            }
            catch (System.Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Author: Gautam Sharma
        /// Adds Parameter with value using Model Data values
        /// </summary>
        /// <typeparam name="T">Pass the Model Data here</typeparam>
        /// <param name="ModelData"></param>
        public void Add_Parameter_WithValues<T>(string SPInitials,T ModelData) where T : class
        {
            try
            {
                if (_sqlCommand.Parameters.Count > 0)
                    _sqlCommand.Parameters.Clear();

                Type temp = typeof(T);
                PropertyInfo[] propertyInfos = temp.GetProperties();

                foreach (var prop in propertyInfos)
                {
                    _sqlCommand.Parameters.AddWithValue((prop.Name.IndexOf("@") < 0) ? $"@{SPInitials}{prop.Name}" :
                        SPInitials+ prop.Name, prop.GetValue(ModelData));
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException SqEx)
            {
                throw new Exception(SqEx.Message);
            }
            catch (System.Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        /// <summary>
        /// Author: Gautam Sharma
        /// Method to insert List/Single Data to a particular table
        /// <param name="SPName"> Provide the Stored Procedure name </param>
        /// <param name="ListData<Model>"> List of data (Model Values); Default - null</param>
        /// <param name="SingleRowData"> If only single row needs to be inserted, pass it here, Default - null</param>
        /// <param name="SPInitials"> If stored procedure parameter's values is given with some initials, pass it here, 
        /// default - null</param>
        /// </summary>
        public async Task<bool> AddOrEditWithStoredProcedure<T>(string SPName, List<T> ListData,
            T SingleRowData, string SPInitials = "") where T : class
        {
            Type temp = typeof(T);

            PropertyInfo[] propertyInfos = temp.GetProperties();
            try
            {
                _sqlCommand.CommandText = SPName;
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.CommandTimeout = 0;

                if (ListData != null)
                {
                    foreach (var item in ListData)
                    {
                        Clear_CommandParameter();
                        await InsertRecords(SPInitials, propertyInfos, item);
                    }
                }
                else if (SingleRowData != null)
                {
                    Clear_CommandParameter();
                    await InsertRecords(SPInitials, propertyInfos, SingleRowData);
                }
                return true;
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;
            }
        }

        private Task InsertRecords<T>(string SPInitials, PropertyInfo[] propertyInfos, T item) where T : class
        {
            foreach (var pro in propertyInfos)
            {
                var _itemValue = pro.GetValue(item);
                // Convert DateTime to MySql DateTime Format
                if (pro.PropertyType.Name == "DateTime")
                {
                    _itemValue = Convert.ToDateTime(_itemValue).ToUniversalTime();
                }
                if (pro.PropertyType.Name == "Guid")
                {
                    _itemValue = _itemValue.ToString();
                }
                _sqlCommand.Parameters.AddWithValue($"@{SPInitials}{pro.Name}", _itemValue);
            }
            return _sqlCommand.ExecuteNonQueryAsync();
        }



        /// <summary>
        /// Author : Sandeep Chauhan
        /// Description :  ExecuteStoredProcedureAsync is an asynchronous method designed to execute a given SQL stored procedure and retrieve its results in a DataTable. 
        /// This method requires 3 parameters:-
        /// <param name="spName"> Name of the stored procedure </param> 
        /// <param name="modelName"> Model class name, which has properties like -> public int64 mobilenumber {get; set;} </param>
        /// <param name="parameterPrefix"> An optional prefix for the parameter names.</param>
        /// </summary>
        public async Task<DataTable> ExecuteStoredProcedureAsync(string spName, object? modelName, string parameterPrefix = "")
        {
            _sqlCommand.CommandText = spName;
            _sqlCommand.CommandType = CommandType.StoredProcedure;
            _sqlCommand.Parameters.Clear();                                          //e.g., like->   _sqlCommand.Clear_CommandParameter();

            if (modelName != null)
            {
                foreach (var property in modelName.GetType().GetProperties())
                {
                    var paramName = parameterPrefix + property.Name;                   //e.g., ->    "prm_" + "mobilenumber" = "prm_mobilenumber"
                    var paramValue = property.GetValue(modelName) ?? DBNull.Value;    //e.g., ->     property.GetValue(modelName) = 9876543210  //If the value is null, it uses DBNull.Value (which is the way to represent null in databases).
                    _sqlCommand.Parameters.AddWithValue(paramName, paramValue);      //e.g., like->  _sqlCommand.Add_Parameter_WithValue("prm_mobilenumber", 9876543210);
                }
            }

            //Checks if the connection associated with _sqlCommand is not already open.
            //If connection is not already open, then it opens the connection asynchronously.
            if (_sqlCommand.Connection.State != ConnectionState.Open)
            {
                await _sqlCommand.Connection.OpenAsync();
            }

            //Creating an empty DataTable
            DataTable result = new DataTable();

            //The method executes the stored procedure and reads the results using a data reader. It then loads these results into the DataTable.
            using (var reader = await _sqlCommand.ExecuteReaderAsync())
            {
                result.Load(reader);   // Storing Rows and Columns in result 
            }

            _sqlCommand.Connection.Close();

            return result;
        }



    }
}
