using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDataAccessLayer
{
    public interface IValidateCodeManager
    {
        string CreateValidateCode(int length);
        byte[] CreateValidateGraphic(string validateCode);
    }
}
