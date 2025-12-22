using Common_DAL.Interfaces;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_DTOs.Entities;
using Common_DAL.Interfaces;

namespace Common_DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;
        public ProjectRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            var projects = new List<Project>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Projects", conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                projects.Add(MapToProject(reader));
            }
            return projects;
        }
        public async Task<Project?> GetProjectByIdAsync(Guid projectId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT TOP 1 * FROM Projects WHERE ProjectId = @ProjectId", conn);
            cmd.Parameters.AddWithValue("@ProjectId", projectId);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToProject(reader);
            }
            return null;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("INSERT INTO Projects (ProjectId, ProjectName, Description, Location, Developer, Status, CreatedAt) VALUES (@ProjectId, @ProjectName, @Description, @Location, @Developer, @Status, @CreatedAt)", conn);
            cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
            cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
            cmd.Parameters.AddWithValue("@Description", project.Description);
            cmd.Parameters.AddWithValue("@Location", project.Location);
            cmd.Parameters.AddWithValue("@Developer", project.Developer);
            cmd.Parameters.AddWithValue("@Status", project.Status);
            cmd.Parameters.AddWithValue("@CreatedAt", project.CreatedAt);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return project;
        }
        public async Task<Project> UpdateProjectAsync(Project project)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("UPDATE Projects SET ProjectName = @ProjectName, Description = @Description, Location = @Location, Developer = @Developer, Status = @Status, UpdatedAt = @UpdatedAt WHERE ProjectId = @ProjectId", conn);
            cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
            cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
            cmd.Parameters.AddWithValue("@Description", project.Description);
            cmd.Parameters.AddWithValue("@Location", project.Location);
            cmd.Parameters.AddWithValue("@Developer", project.Developer);
            cmd.Parameters.AddWithValue("@Status", project.Status);
            cmd.Parameters.AddWithValue("@UpdatedAt", project.UpdatedAt ?? (object)DBNull.Value);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return project;
        }
        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM Projects WHERE ProjectId = @ProjectId", conn);
            cmd.Parameters.AddWithValue("@ProjectId", projectId);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        private Project MapToProject(SqlDataReader reader)
        {
            return new Project
            {
                ProjectId = (Guid)reader["ProjectId"],
                ProjectName = reader["ProjectName"].ToString(),
                Description = reader["Description"].ToString(),
                Location = reader["Location"].ToString(),
                Developer = reader["Developer"].ToString(),
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] as DateTime?
            };
        }
        public async Task<bool> DeleteProjectAsync(List<Guid> projectIds)
        {
            if (projectIds == null || projectIds.Count == 0)
                return false;
            using var conn = new SqlConnection(_connectionString);
            var idParams = string.Join(", ", projectIds.Select((id, index) => $"@Id{index}"));
            using var cmd = new SqlCommand($"DELETE FROM Projects WHERE ProjectId IN ({idParams})", conn);
            for (int i = 0; i < projectIds.Count; i++)
            {
                cmd.Parameters.AddWithValue($"@Id{i}", projectIds[i]);
            }
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}
