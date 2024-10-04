using Core.Abstract;
using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Genre : BaseEntity<Guid>,IEntity 
    {
        public string Name { get; set; }
                
    }
}
