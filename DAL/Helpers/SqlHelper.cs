using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

public class SqlHelper
{
    private readonly string _connectionString;

    // Constructor để nhận chuỗi kết nối
    public SqlHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    // 1. Phương thức lấy kết nối
    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    // 2. Phương thức thực thi Reader (SELECT)
    // Trả về SqlDataReader, việc đóng kết nối được xử lý bởi CommandBehavior.CloseConnection
    public async Task<SqlDataReader> ExecuteReaderAsync(
        string sql,
        CommandType commandType,
        params SqlParameter[] parameters)
    {
        var connection = GetConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(sql, connection);
        command.CommandType = commandType;

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters);
        }
        return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
    }

    // 3. Phương thức thực thi NonQuery (INSERT, UPDATE, DELETE)
    public async Task<int> ExecuteNonQueryAsync(
        string sql,
        CommandType commandType,
        params SqlParameter[] parameters)
    {
        using (var connection = GetConnection()) 
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return await command.ExecuteNonQueryAsync();
            }
        }
    }

    // 4. Phương thức thực thi Scalar (Lấy 1 giá trị duy nhất như COUNT)
    public async Task<object> ExecuteScalarAsync(
        string sql,
        CommandType commandType,
        params SqlParameter[] parameters)
    {
        using (var connection = GetConnection())
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return await command.ExecuteScalarAsync();
            }
        }
    }
}