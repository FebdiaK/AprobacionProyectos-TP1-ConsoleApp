using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsUserInRoleAsync(int userId, int roleId);
        Task<bool> IsUserInAnyRoleAsync(int userId, List<ApproverRole> roleNames);        
        Task<int> CreateUser (string name, string email, ApproverRole role);
        Task<User?> GetUserByIdAsync(int userId);
    }

}
