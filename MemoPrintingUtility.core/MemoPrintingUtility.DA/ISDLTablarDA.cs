using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;

namespace MemoPrintingUtility.DA
{
   public interface ISDLTablarDA
    {
        List<SDLCEntityCON> GetConDataforSDLC(string Course);

        List<SDLCEntityPRES> GetPRESDataforSDLC(string Course);
        List<SDLCEntitySUB> GetSUBJECTforSDLC(string Course);
    }
}
