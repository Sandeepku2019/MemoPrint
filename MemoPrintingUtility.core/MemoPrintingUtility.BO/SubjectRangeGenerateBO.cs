using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;
using MemoPrintingUtility.DA;

namespace MemoPrintingUtility.BO
{
    public class SubjectRangeGenerateBO : ISubjectRangeGenerateBO
    {
        public List<SubjectRangeEntity> GetSubjectDetailsForRange()
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetSubjectRangeService().GetSubjectDetailsForRange();
        }

        public void InsertSubjectCode(SubjectRangeEntity LstsubjectCode)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
             DAFactory.GetSubjectRangeService().InsertSubjectCode(LstsubjectCode);
        }
    }
}
