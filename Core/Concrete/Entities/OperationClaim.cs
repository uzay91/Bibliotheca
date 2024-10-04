using Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Entities
{
    public class OperationClaim : BaseEntity<Guid>, IEntity
    {
        public string Role { get; set; }
    }
}
