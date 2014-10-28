using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2VSoft.EnterpriseLibrary.Validation;

namespace T2VSoft.EnterpriseLibrary.T2VForm
{
    public class DataFieldConfig
    {
        public string DataFieldName { get; set; }


        public List<ValidRuler> Rulers { get; set; }

        public bool Valid { get; set; }

        #region construct method

        public DataFieldConfig()
        {

        }

        public DataFieldConfig(string dataFieldName)
        {
            this.DataFieldName = dataFieldName;
            this.Rulers = new List<ValidRuler>();
            //this.Rulers.Add(new ValidRuler(ValidationType.Number, ""));
        }
        public DataFieldConfig(string dataFieldName, string errorInfo)
        {
            this.DataFieldName = dataFieldName;
            this.Rulers = new List<ValidRuler>();
            this.Rulers.Add(new ValidRuler(ValidationType.Required, errorInfo));
        }

        public DataFieldConfig(string dataFieldName, ValidationType validType, string errorInfo)
        {
            this.DataFieldName = dataFieldName;
            this.Rulers = new List<ValidRuler>();
            this.Rulers.Add(new ValidRuler(validType, errorInfo));
        }

        #endregion


    }
}
