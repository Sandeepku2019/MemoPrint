using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.BO
{
   public interface ISDLCTablarBO
    {
        List<SDLCEntityCON> GetConDataforSDLC(string Course);

        List<SDLCEntityPRES> GetPRESDataforSDLC(string Course);
        List<SDLCEntitySUB> GetSUBJECTforSDLC(string Course);
    }
}
