using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;

namespace MemoPrintingUtility.DA
{
    public class CourseDAService : ICourseDAService
    {
        public List<Courses> GetCourseList()
        {
            MemoPrintDBDataContext CourseDbContext = new MemoPrintDBDataContext();
            var courses = (from cour in CourseDbContext.PR_COURSE_SELECT(1, "", 0)
                           select new Courses()
                           {
                               Id = cour.Pk_CourseID,
                               CourseName = cour.CourseName
                           }).ToList<Courses>();


            return courses;


        }
    }
}
