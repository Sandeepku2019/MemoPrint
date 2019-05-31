using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;
using MemoPrintingUtility.DA;

namespace MemoPrintingUtility.BO
{
    public class LongMemoService : ILongMemoService
    {
        public List<StudentInformation> GetBCA_P_StudentDetailPRESBO()
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetLongMemoService().GetBCA_P_StudentDetailPRES();

        }

        public List<ConsDataEntity> GetBCA_P_StudentsConsDetailsBO()
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetLongMemoService().GetBCA_P_StudentsConsDetails();

        }
    }
}
