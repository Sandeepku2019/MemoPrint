using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;
using MemoPrintingUtility.DA;

namespace MemoPrintingUtility.BO
{
    public class CourseService : ICourseService
    {
        public List<Courses> GetCourseDetails()
        {
            MemoPrintDAFactory PrintDAFactory = new MemoPrintDAFactory();
            return PrintDAFactory.GetCourseInstance().GetCourseList();
        }
    }
}
