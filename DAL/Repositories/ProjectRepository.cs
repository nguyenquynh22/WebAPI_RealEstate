using Common_DAL.Interfaces;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common_DTOs.Entities;

namespace Common_DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly SqlHelper _sqlHelper;

        public ProjectRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            var projects = new List<Project>();
            string sql = "SELECT * FROM Projects";

            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text);
            while (await reader.ReadAsync())
            {
                projects.Add(MapToProject(reader));
            }
            return projects;
        }

        public async Task<Project?> GetProjectByIdAsync(Guid projectId)
        {
            string sql = "SELECT TOP 1 * FROM Projects WHERE ProjectId = @ProjectId";
            var parameter = new SqlParameter("@ProjectId", projectId);

            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text, parameter);
            if (await reader.ReadAsync())
            {
                return MapToProject(reader);
            }
            return null;
        }

        public async Task<List<Project>> GetPagedProjectsAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            var projects = new List<Project>();
            string sql = @"
                SELECT * FROM Projects 
                WHERE (@Search IS NULL OR 
                       ProjectName LIKE @Search OR 
                       Location LIKE @Search OR 
                       Developer LIKE @Search)
                ORDER BY CreatedAt DESC
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var parameters = new[]
            {
                new SqlParameter("@Search", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : $"%{searchTerm}%"),
                new SqlParameter("@Offset", (pageNumber - 1) * pageSize),
                new SqlParameter("@PageSize", pageSize)
            };

            using var reader = await _sqlHelper.ExecuteReaderAsync(sql, CommandType.Text, parameters);
            while (await reader.ReadAsync())
            {
                projects.Add(MapToProject(reader));
            }
            return projects;
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            string sql = @"INSERT INTO Projects (ProjectId, ProjectName, Description, Location, Developer, Status, CreatedAt) 
                           VALUES (@ProjectId, @ProjectName, @Description, @Location, @Developer, @Status, @CreatedAt)";

            var parameters = new[]
            {
                new SqlParameter("@ProjectId", project.ProjectId),
                new SqlParameter("@ProjectName", project.ProjectName),
                new SqlParameter("@Description", project.Description ?? (object)DBNull.Value),
                new SqlParameter("@Location", project.Location ?? (object)DBNull.Value),
                new SqlParameter("@Developer", project.Developer ?? (object)DBNull.Value),
                new SqlParameter("@Status", project.Status),
                new SqlParameter("@CreatedAt", project.CreatedAt)
            };

            await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
            return project;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            project.UpdatedAt = DateTime.Now;

            string sql = @"UPDATE Projects SET ProjectName = @ProjectName, Description = @Description, 
                   Location = @Location, Developer = @Developer, Status = @Status, 
                   UpdatedAt = @UpdatedAt WHERE ProjectId = @ProjectId";

            var parameters = new[]
            {
                new SqlParameter("@ProjectId", project.ProjectId),
                new SqlParameter("@ProjectName", project.ProjectName),
                new SqlParameter("@Description", project.Description ?? (object)DBNull.Value),
                new SqlParameter("@Location", project.Location ?? (object)DBNull.Value),
                new SqlParameter("@Developer", project.Developer ?? (object)DBNull.Value),
                new SqlParameter("@Status", project.Status),
                new SqlParameter("@UpdatedAt", project.UpdatedAt) 
            };

            await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
            return project;
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            string sql = "DELETE FROM Projects WHERE ProjectId = @ProjectId";
            var parameter = new SqlParameter("@ProjectId", projectId);

            int result = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, parameter);
            return result > 0;
        }

        public async Task<bool> DeleteProjectAsync(List<Guid> projectIds)
        {
            if (projectIds == null || projectIds.Count == 0) return false;

            var idParams = string.Join(", ", projectIds.Select((id, index) => $"@Id{index}"));
            string sql = $"DELETE FROM Projects WHERE ProjectId IN ({idParams})";

            var parameters = projectIds.Select((id, index) => new SqlParameter($"@Id{index}", id)).ToArray();

            int result = await _sqlHelper.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
            return result > 0;
        }

        private Project MapToProject(SqlDataReader reader)
        {
            return new Project
            {
                ProjectId = (Guid)reader["ProjectId"],
                ProjectName = reader["ProjectName"]?.ToString() ?? string.Empty,
                Description = reader["Description"]?.ToString() ?? string.Empty,
                Location = reader["Location"]?.ToString() ?? string.Empty,
                Developer = reader["Developer"]?.ToString() ?? string.Empty,
                Status = reader["Status"]?.ToString() ?? "Active",
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader["UpdatedAt"] as DateTime?
            };
        }
    }
}