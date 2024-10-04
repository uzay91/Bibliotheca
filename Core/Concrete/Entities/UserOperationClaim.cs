using Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Entities
{
    public class UserOperationClaim : BaseEntity<Guid>, IEntity
    {
        //authorization işlemi bu tablo aracılığıyla sağlanır.
        public Guid UserId { get; set; } // foreign key
        public Guid OperationClaimId { get; set; } // foreign key
    }
}
