﻿using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.TokenService
{
    public interface ITokenService
    {
        public string GenerateToken(AppUser appUser);
    }
}
