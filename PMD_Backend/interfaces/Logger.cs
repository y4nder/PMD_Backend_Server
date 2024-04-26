using PMD_Backend.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMD_Backend.interfaces
{
    public interface Logger
    {
        public string CreateLog(Admin admin);
    }
}
