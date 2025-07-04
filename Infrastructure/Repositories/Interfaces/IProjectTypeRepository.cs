﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Infrastructure.Repositories.Interfaces
{
    internal interface IProjectTypeRepository
    {
        Task<List<ProjectType>> GetAllAsync(); 
    }
}
