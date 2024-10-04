﻿using Core.Concrete.Entities;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfOperationClaimDal : EfEntityRepository<OperationClaim, PostgreSqlDbContext>, IOperationClaimDal
    {
        public EfOperationClaimDal(PostgreSqlDbContext context) : base(context)
        {
        }
    }
}