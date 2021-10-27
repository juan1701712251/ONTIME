﻿using SCAPE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCAPE.Domain.Interfaces
{
    public interface IEmployerRepository
    {
        Task<Employer> findEmployerByEmail(string email);
    }
}
