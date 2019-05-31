using MemoPrintingUtility.BO;
using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemoPrintingUtility.Controllers
{
    public class LongMemoController : Controller
    {
        // GET: LongMemo
        public ActionResult Index()
        {
            return View();
        }

        int PageBraker = 0;

        [HttpPost]
        public JsonResult GenerateLongmemo(string course)
        {
            if (course == "BA (L)")
            {
                return null;//GenerateBA_L("BAL");
            }
            else
            {

                return GenerateReport(course);
            }
        }


        private JsonResult GenerateReport(string course)
        {
            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/TabularReport/");
                string fileNamedirectory = @"D:\LongMemo\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_" + course + ".txt";

                // check for Directory
                if (!Directory.Exists(fileNamedirectory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(fileNamedirectory);
                }


                // file information 
                FileInfo fi = new FileInfo(fileNamedirectory + filename);
                // Check if file already exists. If yes, delete it.     
                if (System.IO.File.Exists(fileNamedirectory + filename))
                {
                    System.IO.File.Delete(fileNamedirectory + filename);
                }

                MemoPrintService BoMemoService = new MemoPrintService();

                int year = 0;
                int sem = 0;


                // Create a new file   
                string hn = "";
                using (StreamWriter sw = fi.CreateText())
                {
                    //
                    var lstStudents = BoMemoService.GetLongMemoInstance().GetBCA_P_StudentDetailPRESBO();
                    var LstConStudents = BoMemoService.GetLongMemoInstance().GetBCA_P_StudentsConsDetailsBO();

                    var lstBCASubject = BoMemoService.getTabularReportInstance().GetBCAPSubjectInformation("BCA");

                    List<string> HallticketNumbers = lstStudents.OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();
                    string Syear = string.Empty;
                    string Ssem = string.Empty;
                    int yr = 0;
                    int sm = 0;
                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {
                        hn = HallticketNumbers[i];

                        if (hn == "084125009")
                        {

                        }
                        int Psem = 0;
                        var lstStunsfirst = lstStudents.OrderBy(y => y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                        var lstStunsCons = LstConStudents.Where(x => x.HTNO == HallticketNumbers[i]).ToList<ConsDataEntity>();


                        #region Sem declaration

                        if (Psem == 11)
                        {
                            Syear = "I";
                            yr = 1;
                            sm = 1;
                            Ssem = "I";

                        }
                        if (Psem == 21)
                        {
                            Syear = "II";
                            yr = 2;
                            sm = 1;
                            Ssem = "I";

                        }

                        if (Psem == 31)
                        {
                            Syear = "III";
                            yr = 3;
                            Ssem = "I";
                            sm = 1;
                        }
                        #endregion

                        string HallTicket_1 = lstStunsfirst[0].HallTicketNumber == null ? "" : lstStunsfirst[0].HallTicketNumber;
                        string FN_1 = lstStunsfirst[0].FatherName == null ? "" : lstStunsfirst[0].FatherName;
                        string SN_1 = lstStunsfirst[0].StudentName == null ? "" : lstStunsfirst[0].StudentName;
                        string CC_1 = lstStunsfirst[0].collegecode == null ? "" : lstStunsfirst[0].collegecode;

                        string CourseDetails = GetSpaces(13) + course + " " + Syear + " YR " + Ssem + " SEM   NOV/DEC ." + (DateTime.Now.Year - 1).ToString();
                        CourseDetails = CourseDetails + GetSpaces(72 - CourseDetails.Length) + DateTime.Now.ToString("dd-MM-");





                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");

                        sw.WriteLine(GetSpaces(66) + CC_1);
                        string Examination = "B.C.A. ,  NOV., 2018";
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");

                        sw.WriteLine(GetSpaces(13) + Examination + GetSpaces(44 - Examination.Length) + DateTime.Now.ToString("dd-MM-yyyy"));
                        sw.WriteLine(GetSpaces(13) + SN_1 + GetSpaces(44 - SN_1.Length));
                        sw.WriteLine(GetSpaces(13) + FN_1 + GetSpaces(44 - FN_1.Length) + HallTicket_1);
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        int[] yrs = new int[] { 1, 2, 3 };
                        int[] sems = new int[] { 1, 2};
                        int SubjectRowCount = 0;

                        int TotalMark = 0;
                        List<StudentInformation> lstMemoDetils = new List<StudentInformation>();
                        sw.WriteLine(""); SubjectRowCount++;
                        for (int j = 0; j < yrs.Length; j++)
                        {
                            for (int z = 0; z < sems.Length; z++)
                            {
                                List<StudentInformation> lstMemo = new List<StudentInformation>();
                                string consem = yrs[j].ToString() + sems[z].ToString();
                                var lstConData = lstStunsCons.Where(x => x.SEM == consem).ToList();

                                if (lstConData != null && lstConData.Count > 0)
                                {
                                    lstMemo = GetCondataFormat(lstConData[0].P1, lstConData[0].M1, lstConData[0].S1, lstConData[0].A1, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P2, lstConData[0].M2, lstConData[0].S2, lstConData[0].A2, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P3, lstConData[0].M3, lstConData[0].S3, lstConData[0].A3, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P4, lstConData[0].M4, lstConData[0].S4, lstConData[0].A4, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P5, lstConData[0].M5, lstConData[0].S5, lstConData[0].A5, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P6, lstConData[0].M6, lstConData[0].S6, lstConData[0].A6, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P7, lstConData[0].M7, lstConData[0].S7, lstConData[0].A7, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                }
                                var lstpressData = lstStunsfirst.Where(x => x.Year == yrs[j].ToString() && x.Sem == sems[z].ToString()).ToList();
                                foreach (var Pres in lstpressData)
                                {
                                    var conUpdate = lstMemo.Where(x => x.SubjectCode == Pres.SubjectCode).ToList().FirstOrDefault();
                                    if (conUpdate != null)
                                    {
                                        if (conUpdate.ExernalMarks.ChangeINT() < Pres.ExernalMarks.ChangeINT())
                                        {
                                            conUpdate.ExernalMarks = Pres.ExernalMarks;
                                            conUpdate.InternalMarks = Pres.InternalMarks;
                                            conUpdate.AcadmicYear = "n18".ChangeToMonthandYear();
                                        }
                                    }


                                }

                                SubjectRowCount++;

                                if (j == 0 & z == 0)
                                {
                                    sw.WriteLine("I YEAR:");
                                }

                                if (j == 1 & z== 0)
                                {
                                    sw.WriteLine("II YEAR I SEMESTER:");
                                }

                                if (j == 1 & z == 1)
                                {
                                    sw.WriteLine("II YEAR II SEMESTER:");
                                }


                                if (j == 2 & z == 0)
                                {
                                    sw.WriteLine("III YEAR I SEMESTER:");
                                }


                                if (j == 2 & z == 1)
                                {
                                    sw.WriteLine("III YEAR II SEMESTER:");
                                }



                                foreach (var merge in lstMemo)
                                {
                                    if (merge.SubjectName != null)
                                    {

                                        string Subject = merge.SubjectName.Length == 0 ? Convert.ToString(merge.SubjectCode) : merge.SubjectName;

                                        if (Subject == null)
                                        {
                                            Subject = "";
                                        }
                                        int totalMark = merge.SubjectExternalMarks.ChangeINT() + merge.SubjectInternalMarks.ChangeINT();
                                        string MinMark = merge.MinMarks;

                                        int OCMark = merge.ExernalMarks.ChangeINT() + merge.InternalMarks.ChangeINT();
                                        TotalMark = TotalMark + OCMark;
                                        sw.WriteLine(Subject + GetSpaces(42 - Subject.Length) + totalMark + GetSpaces(6 - totalMark.ToString().Length) + MinMark + GetSpaces(6 - MinMark.ToString().Length) + OCMark + GetSpaces(6 - OCMark.ToString().Length) + merge.AcadmicYear);
                                        SubjectRowCount++;
                                    }

                                }

                                sw.WriteLine(""); SubjectRowCount++;
                                sw.WriteLine(""); SubjectRowCount++;

                               
                            }

                        }
                        if (SubjectRowCount < 62)
                        {
                            int rCoun = 62 - SubjectRowCount;

                            for (int b = 0; b < 63; b++)
                            {
                                sw.WriteLine("");
                            }

                        }

                        sw.WriteLine(GetSpaces(13) + TotalMark.ToString() + GetSpaces(5) + "(" + TotalMark.NumberToWords().ToUpper() + ")");

                        string Division = string.Empty;
                        if (TotalMark < 1500 && lstStunsfirst[0].FinalResult == "COMPLETED")
                        {
                            Division = "";
                        }

                        if ((TotalMark < 1799 &&TotalMark > 1500 )&& lstStunsfirst[0].FinalResult == "COMPLETED")
                        {
                            Division = "SECOND DIVISION";
                        }

                        if ((TotalMark < 2099 && TotalMark > 1800) && lstStunsfirst[0].FinalResult == "COMPLETED")
                        {
                            Division = "FIRST DIVISION";
                        }


                        if ((TotalMark > 2100  && TotalMark < 3000) && lstStunsfirst[0].FinalResult == "COMPLETED")
                        {
                            Division = "FIRST DIVISION";
                        }

                        sw.WriteLine(GetSpaces(13) + lstStunsfirst[0].FinalResult + "/" + Division);

                    }


                }

                // Write file contents on console.     
                using (StreamReader sr = System.IO.File.OpenText(fileNamedirectory + filename))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                return Json(fileNamedirectory + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }


        }



        private string GetSpaces(int totalLength)
        {
            string result = string.Empty;
            for (int i = 0; i < totalLength; i++)
            {
                result += " ";
            }
            return result;
        }



        private List<StudentInformation> GetCondataFormat(string SubCode, string MarksScored, string internals, string Academic, List<StudentInformation> lst, List<BCAPSubjectINformation> lstSubjectInfo, string yr, string sem)
        {
            var sub = lstSubjectInfo.Where(x => x.SubjectCode == SubCode && x.Year == yr && x.Sem == sem).ToList();
            string SbName = string.Empty;
            string MaxMarks = string.Empty;
            string MinMarks = string.Empty;
            string Occupined = string.Empty;
            if (sub != null && sub.Count() > 0)
            {
                SbName = sub[0].SubjectName;
                MaxMarks = sub[0].MaxMark;
                MinMarks = sub[0].MinMark;

            }
            lst.Add(new StudentInformation()
            {
                SubjectCode = SubCode,
                SubjectName = SbName,
                MinMarks = MinMarks,
                SubjectExternalMarks = MaxMarks,
                ExernalMarks = MarksScored,
                InternalMarks = internals,
                AcadmicYear = Academic.ChangeToMonthandYear(),
                Year = yr,
                Sem = sem

            });

            return lst;
        }
    }


}
