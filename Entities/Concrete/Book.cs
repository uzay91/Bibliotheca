using Core.Abstract;
using Core.Concrete.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Book : BaseEntity<Guid>, IEntity
    {
        public Guid GenreId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Edition { get; set; }
        public int TotalPage { get; set; }
       
    }
}
