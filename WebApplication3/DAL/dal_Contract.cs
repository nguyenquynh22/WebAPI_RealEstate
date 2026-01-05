using Microsoft.Data.SqlClient;
using REstate.DTO;
using System.Data;

namespace REstate.DAL
{
    public class ContractDAL
    {
        private readonly string _connectionString =
        "Server=LAPTOP-T2MKG56O\\SQLEXPRESS;Database=hdvu;Trusted_Connection=True;TrustServerCertificate=True;";

        public async Task<List<dto_ConTract>> GetAllAsync()
        {
            var list = new List<dto_ConTract>();

            var sql = @"
                SELECT 
                    ContractId, ListingId, SellerId, BuyerId,
                    ContractType, Price, PriceUnit,
                    DurationMonths, DepositAmount, DownPayment,
                    Status, PdfDocumentUrl,
                    SigningDate, EffectiveDate,
                    CreatedAt, UpdatedAt
                FROM Contracts";

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new dto_ConTract
                {
                    ContractId = reader.GetGuid(0),
                    ListingId = reader.GetGuid(1),
                    SellerId = reader.GetGuid(2),
                    BuyerId = reader.GetGuid(3),
                    ContractType = reader.GetString(4),
                    Price = reader.GetDecimal(5),
                    PriceUnit = reader.GetString(6),
                    DurationMonths = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                    DepositAmount = reader.IsDBNull(8) ? null : reader.GetDecimal(8),
                    DownPayment = reader.IsDBNull(9) ? null : reader.GetDecimal(9),
                    Status = reader.IsDBNull(10) ? null : reader.GetString(10),
                    PdfDocumentUrl = reader.IsDBNull(11) ? null : reader.GetString(11),
                    SigningDate = reader.IsDBNull(12) ? null : reader.GetDateTime(12),
                    EffectiveDate = reader.IsDBNull(13) ? null : reader.GetDateTime(13),
                    CreatedAt = reader.IsDBNull(14) ? null : reader.GetDateTime(14),
                    UpdatedAt = reader.IsDBNull(15) ? null : reader.GetDateTime(15)
                });
            }

            return list;
        }

        public async Task<dto_ConTract?> GetByIdAsync(Guid id)
        {
            var sql = @"SELECT * FROM Contracts WHERE ContractId = @id";

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;

            return new dto_ConTract
            {
                ContractId = reader.GetGuid(reader.GetOrdinal("ContractId")),
                ListingId = reader.GetGuid(reader.GetOrdinal("ListingId")),
                SellerId = reader.GetGuid(reader.GetOrdinal("SellerId")),
                BuyerId = reader.GetGuid(reader.GetOrdinal("BuyerId")),
                ContractType = reader.GetString(reader.GetOrdinal("ContractType")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                PriceUnit = reader.GetString(reader.GetOrdinal("PriceUnit")),
                DurationMonths = reader["DurationMonths"] as int?,
                DepositAmount = reader["DepositAmount"] as decimal?,
                DownPayment = reader["DownPayment"] as decimal?,
                Status = reader["Status"] as string,
                PdfDocumentUrl = reader["PdfDocumentUrl"] as string,
                SigningDate = reader["SigningDate"] as DateTime?,
                EffectiveDate = reader["EffectiveDate"] as DateTime?,
                CreatedAt = reader["CreatedAt"] as DateTime?,
                UpdatedAt = reader["UpdatedAt"] as DateTime?
            };
        }

        public async Task<bool> CreateAsync(dto_ConTract dto)
        {
            var sql = @"
                INSERT INTO Contracts
                (ContractId, ListingId, SellerId, BuyerId, ContractType,
                 Price, PriceUnit, DurationMonths, DepositAmount,
                 DownPayment, Status, PdfDocumentUrl, CreatedAt)
                VALUES
                (@ContractId, @ListingId, @SellerId, @BuyerId, @ContractType,
                 @Price, @PriceUnit, @DurationMonths, @DepositAmount,
                 @DownPayment, @Status, @PdfDocumentUrl, GETUTCDATE())";

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ContractId", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@ListingId", dto.ListingId);
            cmd.Parameters.AddWithValue("@SellerId", dto.SellerId);
            cmd.Parameters.AddWithValue("@BuyerId", dto.BuyerId);
            cmd.Parameters.AddWithValue("@ContractType", dto.ContractType);
            cmd.Parameters.AddWithValue("@Price", dto.Price);
            cmd.Parameters.AddWithValue("@PriceUnit", dto.PriceUnit);
            cmd.Parameters.AddWithValue("@DurationMonths", (object?)dto.DurationMonths ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DepositAmount", (object?)dto.DepositAmount ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DownPayment", (object?)dto.DownPayment ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", (object?)dto.Status ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PdfDocumentUrl", (object?)dto.PdfDocumentUrl ?? DBNull.Value);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Guid id, dto_ConTract dto)
        {
            var sql = @"
                UPDATE Contracts SET
                    ContractType = @ContractType,
                    Price = @Price,
                    PriceUnit = @PriceUnit,
                    DurationMonths = @DurationMonths,
                    DepositAmount = @DepositAmount,
                    DownPayment = @DownPayment,
                    Status = @Status,
                    PdfDocumentUrl = @PdfDocumentUrl,
                    UpdatedAt = GETUTCDATE()
                WHERE ContractId = @Id";

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@ContractType", dto.ContractType);
            cmd.Parameters.AddWithValue("@Price", dto.Price);
            cmd.Parameters.AddWithValue("@PriceUnit", dto.PriceUnit);
            cmd.Parameters.AddWithValue("@DurationMonths", (object?)dto.DurationMonths ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DepositAmount", (object?)dto.DepositAmount ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DownPayment", (object?)dto.DownPayment ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", (object?)dto.Status ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PdfDocumentUrl", (object?)dto.PdfDocumentUrl ?? DBNull.Value);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var sql = "DELETE FROM Contracts WHERE ContractId = @Id";

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
