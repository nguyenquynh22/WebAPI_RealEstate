using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

using System.Data.SqlClient;

public class ContractDAL
{
    private readonly string _connectionString;

    public ContractDAL()
    {
        _connectionString =
            "Server=LAPTOP-T2MKG56O\\SQLEXPRESS;" +
            "Database=hdvu;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";
    }

    // ví dụ test kết nối
    public bool TestConnection()
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        return conn.State == System.Data.ConnectionState.Open;
    }

    // GET ALL
    public List<Contract> GetAll()
    {
        var list = new List<Contract>();

        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT * FROM Contracts", conn);

        conn.Open();
        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Contract
            {
                ContractId = (int)reader["ContractId"],
                ListingId = (int)reader["ListingId"],
                SellerId = (int)reader["SellerId"],
                BuyerId = (int)reader["BuyerId"],
                ContractType = reader["ContractType"].ToString(),
                Price = (decimal)reader["Price"],
                PriceUnit = reader["PriceUnit"].ToString(),
                DurationMonths = (int)reader["DurationMonths"],
                DepositAmount = (decimal)reader["DepositAmount"],
                DownPayment = (decimal)reader["DownPayment"],
                Status = reader["Status"].ToString(),
                PdfDocumentUrl = reader["PdfDocumentUrl"].ToString(),
                SigningDate = (DateTime)reader["SigningDate"],
                EffectiveDate = (DateTime)reader["EffectiveDate"]
            });
        }

        return list;
    }

    // GET BY ID
    public Contract GetById(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(
            "SELECT * FROM Contracts WHERE ContractId = @id", conn);

        cmd.Parameters.AddWithValue("@id", id);
        conn.Open();

        var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new Contract
        {
            ContractId = (int)reader["ContractId"],
            ListingId = (int)reader["ListingId"],
            SellerId = (int)reader["SellerId"],
            BuyerId = (int)reader["BuyerId"],
            ContractType = reader["ContractType"].ToString(),
            Price = (decimal)reader["Price"],
            PriceUnit = reader["PriceUnit"].ToString(),
            DurationMonths = (int)reader["DurationMonths"],
            DepositAmount = (decimal)reader["DepositAmount"],
            DownPayment = (decimal)reader["DownPayment"],
            Status = reader["Status"].ToString(),
            PdfDocumentUrl = reader["PdfDocumentUrl"].ToString(),
            SigningDate = (DateTime)reader["SigningDate"],
            EffectiveDate = (DateTime)reader["EffectiveDate"]
        };
    }

    // CREATE
    public void Create(Contract c)
    {
        string sql = @"
        INSERT INTO Contracts
        (ListingId, SellerId, BuyerId, ContractType, Price, PriceUnit,
         DurationMonths, DepositAmount, DownPayment, Status,
         PdfDocumentUrl, SigningDate, EffectiveDate, CreatedAt, UpdatedAt)
        VALUES
        (@ListingId, @SellerId, @BuyerId, @ContractType, @Price, @PriceUnit,
         @DurationMonths, @DepositAmount, @DownPayment, @Status,
         @PdfDocumentUrl, @SigningDate, @EffectiveDate, GETDATE(), GETDATE())";

        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@ListingId", c.ListingId);
        cmd.Parameters.AddWithValue("@SellerId", c.SellerId);
        cmd.Parameters.AddWithValue("@BuyerId", c.BuyerId);
        cmd.Parameters.AddWithValue("@ContractType", c.ContractType);
        cmd.Parameters.AddWithValue("@Price", c.Price);
        cmd.Parameters.AddWithValue("@PriceUnit", c.PriceUnit);
        cmd.Parameters.AddWithValue("@DurationMonths", c.DurationMonths);
        cmd.Parameters.AddWithValue("@DepositAmount", c.DepositAmount);
        cmd.Parameters.AddWithValue("@DownPayment", c.DownPayment);
        cmd.Parameters.AddWithValue("@Status", c.Status);
        cmd.Parameters.AddWithValue("@PdfDocumentUrl", c.PdfDocumentUrl);
        cmd.Parameters.AddWithValue("@SigningDate", c.SigningDate);
        cmd.Parameters.AddWithValue("@EffectiveDate", c.EffectiveDate);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    // UPDATE
    public void Update(Contract c)
    {
        string sql = @"
        UPDATE Contracts SET
            ListingId=@ListingId,
            SellerId=@SellerId,
            BuyerId=@BuyerId,
            ContractType=@ContractType,
            Price=@Price,
            PriceUnit=@PriceUnit,
            DurationMonths=@DurationMonths,
            DepositAmount=@DepositAmount,
            DownPayment=@DownPayment,
            Status=@Status,
            PdfDocumentUrl=@PdfDocumentUrl,
            SigningDate=@SigningDate,
            EffectiveDate=@EffectiveDate,
            UpdatedAt=GETDATE()
        WHERE ContractId=@ContractId";

        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@ContractId", c.ContractId);
        cmd.Parameters.AddWithValue("@ListingId", c.ListingId);
        cmd.Parameters.AddWithValue("@SellerId", c.SellerId);
        cmd.Parameters.AddWithValue("@BuyerId", c.BuyerId);
        cmd.Parameters.AddWithValue("@ContractType", c.ContractType);
        cmd.Parameters.AddWithValue("@Price", c.Price);
        cmd.Parameters.AddWithValue("@PriceUnit", c.PriceUnit);
        cmd.Parameters.AddWithValue("@DurationMonths", c.DurationMonths);
        cmd.Parameters.AddWithValue("@DepositAmount", c.DepositAmount);
        cmd.Parameters.AddWithValue("@DownPayment", c.DownPayment);
        cmd.Parameters.AddWithValue("@Status", c.Status);
        cmd.Parameters.AddWithValue("@PdfDocumentUrl", c.PdfDocumentUrl);
        cmd.Parameters.AddWithValue("@SigningDate", c.SigningDate);
        cmd.Parameters.AddWithValue("@EffectiveDate", c.EffectiveDate);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    // DELETE
    public void Delete(int id)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(
            "DELETE FROM Contracts WHERE ContractId=@id", conn);

        cmd.Parameters.AddWithValue("@id", id);
        conn.Open();
        cmd.ExecuteNonQuery();
    }
}

