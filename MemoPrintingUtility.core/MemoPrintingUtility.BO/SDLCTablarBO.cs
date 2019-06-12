using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;
using MemoPrintingUtility.DA;

namespace MemoPrintingUtility.BO
{
    public class SDLCTablarBO : ISDLCTablarBO
    {
        public List<SDLCEntityCON> GetConDataforSDLC(string Course)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetSDLCInstance().GetConDataforSDLC(Course);
        }

        public List<SDLCEntityPRES> GetPRESDataforSDLC(string Course)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetSDLCInstance().GetPRESDataforSDLC(Course);
        }

        public List<SDLCEntitySUB> GetSUBJECTforSDLC(string Course)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetSDLCInstance().GetSUBJECTforSDLC(Course);
        }
    }
}
