using System.Data.SqlClient;

namespace Authorization
{
    public class DataBase
    {
        public readonly SqlConnection Connection;

        public DataBase(string databaseFullPath)
        {
            Connection = new SqlConnection($@"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename={databaseFullPath};Integrated Security=SSPI");
        }
        //(local)\SQLEXPRESS
        //
        //Initial Catalog=

        public void Open()
        {
            if (Connection.State == System.Data.ConnectionState.Closed)
                Connection.Open();
        }

        public void Close()
        {
            if (Connection.State == System.Data.ConnectionState.Open)
                Connection.Close();
        }
    }
}
