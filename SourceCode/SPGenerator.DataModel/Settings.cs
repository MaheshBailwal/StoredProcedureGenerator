using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPGenerator.DataModel
{
    public class Settings
    {
        public string prefixWhereParameter = "@w_";
        public string prefixInputParameter = "@p_";
        public string prefixInsertSp = "sp_insert_";
        public string prefixUpdateSp = "sp_update_";
        public string  errorHandling = "Yes";
    }
}
