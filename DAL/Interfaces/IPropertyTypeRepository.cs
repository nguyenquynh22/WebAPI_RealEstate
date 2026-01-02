using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common_DTOs.Entities;

namespace Common_DAL.Interfaces
{
    public interface IPropertyTypeRepository
    {
        Task<List<PropertyTypes>> GetAllPropertyTypesAsync();
        Task<PropertyTypes?> GetPropertyTypeByIdAsync(int propertyTypeId);
        Task<int> CreatePropertyTypeAsync(PropertyTypes propertyType);
        Task<bool> UpdatePropertyTypeAsync(PropertyTypes propertyType);
        Task<bool> DeletePropertyTypeAsync(int propertyTypeId);
    }
}