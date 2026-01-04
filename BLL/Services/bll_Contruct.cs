using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_BLL.Interfaces;
using Common_DAL.Interfaces;
using Common_DTOs.DTOs;

namespace Common_BLL.Services
{
    public class ContractBLL
    {
        private readonly ContractDAL _dal;

        public ContractBLL(ContractDAL dal)
        {
            _dal = dal;
        }

        public List<Contract> GetAll() => _dal.GetAll();

        public Contract GetById(int id) => _dal.GetById(id);

        public void Create(Contract c)
        {
            if (c.Price <= 0)
                throw new Exception("Price must be greater than 0");

            _dal.Create(c);
        }

        public void Update(Contract c)
        {
            _dal.Update(c);
        }

        public void Delete(int id)
        {
            _dal.Delete(id);
        }
    }

}
