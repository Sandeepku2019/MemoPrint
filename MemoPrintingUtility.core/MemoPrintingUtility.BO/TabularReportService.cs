using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoPrintingUtility.Entity;
using MemoPrintingUtility.DA;

namespace MemoPrintingUtility.BO
{
    public class TabularReportService : ITabularReportService
    {
        public List<StudentInformation> GetStudentDetail(string Course, int Semister, int sem, int year)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetStudentDetail(Course, Semister, sem, year);
        }

        public List<ConsDataEntity> GetStudentsConsDetails(string Course, int Semister)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetStudentsConsDetails(Course, Semister);

        }


        public List<StudentInformation> GetMallPractHtno(string Course, int Semister, int sem, int year)
        {

            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetMallPractHtno(Course, Semister, sem, year);

        }

        public List<TotalsubjectRecord> getTotalandPassed(string Course, int year)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().getTotalandPassed(Course, year);

        }

        public List<BALPresEntity> GetBalPresInformation(string course)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetBalPresInformation(course);
        }

        public List<BALConEntity> GetBALConInformaion()
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetBALConInformaion();
        }

        public List<StudentInformation> GetBCA_P_StudentDetailPRES(string sem, string year)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetBCA_P_StudentDetailPRES(sem, year);
        }

        public List<ConsDataEntity> GetBCA_P_StudentsConsDetails(string Semister)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetBCA_P_StudentsConsDetails(Semister);
        }

        public List<BALSubjectInformation> GetBALSubjectInformation(string CourseName)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetBALSubjectInformation(CourseName);
        }

        public List<BCAPSubjectINformation> GetBCAPSubjectInformation(string CourseName)
        {
            MemoPrintDAFactory DAFactory = new MemoPrintDAFactory();
            return DAFactory.GetTabularDAService().GetBCAPSubjectInformation(CourseName);
        }
    }

    public static class StringExtensions
    {

        /// <summary>
        /// Truncates string so that it is no longer than the specified number of characters.
        /// </summary>
        /// <param name="str">String to truncate.</param>
        /// <param name="length">Maximum string length.</param>
        /// <returns>Original string or a truncated one if the original was too long.</returns>
        public static string Truncate(this string str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
            }

            if (str == null)
            {
                return null;
            }

            int maxLength = Math.Min(str.Length, length);
            return str.Substring(0, maxLength);
        }


        public static string CheckNumeric(this string str, int length)
        {
            int n;
            bool isNumeric = int.TryParse(str, out n);

            if (isNumeric)
            {
                str = str.Insert(str.Length - 2, "/");
                return str;
            }
            else
            {
                return str;
            }

        }

        public static int ChangeINT(this string str)
        {
            int n;
            bool isNumeric = int.TryParse(str, out n);

            if (isNumeric)
            {

                return Convert.ToInt32(str);
            }

            else
            {
                return 0;
            }

        }


        public static string NumberToWords(this int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string ChangeToMonthandYear(this string academic)
        {
            string Month = academic[0].ToString();
            string Yeatr = "'20"+academic.Substring(1, 2);
            switch (Month.ToLower())
            {

                case "1":
                    Month = "Jan";
                    break;

                case "2":
                    Month = "Feb";
                    break;

                case "3":
                    Month = "Mar";
                    break;

                case "4":
                    Month = "Apr";
                    break;
                case "5":
                    Month = "May";
                    break;

                case "6":
                    Month = "June";
                    break;
                case "7":
                    Month = "July";
                    break;

                case "8":
                    Month = "Aug";
                    break;

                case "9":
                    Month = "Sep";
                    break;

                case "o":
                    Month = "Oct";
                    break;
               
                case "n":
                    Month = "Nov";
                    break;

                case "d":
                    Month = "Dec";
                    break;

                   
                default:
                    Month = "";
                    break;
            }

            return Month + Yeatr;

        }


    }
}
