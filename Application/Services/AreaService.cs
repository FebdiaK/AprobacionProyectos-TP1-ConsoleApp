using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Application.Services
{
    internal class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        public AreaService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }
        public async Task<List<Area>> GetAllAreasAsync()
        {
            return await _areaRepository.GetAllAsync();
        }
    }
}
