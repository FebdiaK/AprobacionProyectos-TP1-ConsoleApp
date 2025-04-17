using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IApproverRoleRepository _approverRepository;

        public UserService(IUserRepository userRepository, IApproverRoleRepository approverRepository)
        {
            _userRepository = userRepository;
            _approverRepository = approverRepository;
        }

        public async Task<bool> IsUserInRoleAsync(int userId, int roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var role = await _approverRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return false;
            }

            return user.ApproverRole != null && user.ApproverRole.Id == role.Id; 
        }
        public async Task<bool> IsUserInAnyRoleAsync(int userId, List<ApproverRole> roleNames)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.ApproverRole == null)
            {
                return false;
            }
            var userRole = await _approverRepository.GetByIdAsync(user.ApproverRole.Id);
            if (userRole == null)
            {
                return false;
            }
            return roleNames.Any(role => role.Id == userRole.Id);

        }
        public Task<int> CreateUser(string name, string email, ApproverRole role)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                ApproverRole = role
            };
            _userRepository.CreateAsync(user);
            return Task.FromResult(user.Id);
        } 

    }
    
}
