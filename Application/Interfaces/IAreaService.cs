using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces
{
    public interface IAreaService
    {
        Task<List<Area>> GetAllAreasAsync();
       
    }
}
