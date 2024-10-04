using Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Dtos
{
    public class UserOperationClaimDto:IDto
    {
        //OperationClaim id si 
        public Guid Id { get; set; }
        public string Role{ get; set; }
    }
}
