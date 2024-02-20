namespace BAL.Configurations
{
    public class AppDbContext
    {
        // Changed by Gautam
        public MySqlConnection _connection { get; private set; } // privately set connections
        public MySqlCommand _sqlCommand { get; private set; }
        //public AppDbContext()
        //{
        //    _connection = new MySqlConnection();
        //    _sqlCommand = new MySqlCommand(_connection._Connection);
        //}
        public void OpenContext()
        {
            _connection = new MySqlConnection();
            _sqlCommand = new MySqlCommand(_connection._Connection);
        }
        public void CloseContext()
        {
            if (_sqlCommand != null)
                _sqlCommand.Close();
            if (_connection != null)
                _connection.Close();
        }
    }
}
