using MemoPrintingUtility.BO;
using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemoPrintingUtility.Controllers
{
    public class SubjectRangeCodeController : Controller
    {
        // GET: SubjectRangeCode
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetData()
        {
            try
            {

                MemoPrintService BoMemoService = new MemoPrintService();
                var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRange();
                lstSubjectDetails = lstSubjectDetails.OrderBy(x => x.Year).ThenBy(x => x.Sem).ThenBy(x => x.CourseName).ToList();

                TempData["SubjectList"] = lstSubjectDetails;
                TempData.Keep();

                return Json(lstSubjectDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        
        [HttpPost]
        public JsonResult GenerateRanges(Int32 RangeFrom, Int32 Gap)
        {

            try
            {
                int RF = RangeFrom;

                MemoPrintService BoMemoService = new MemoPrintService();
                var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRange();

                lstSubjectDetails = lstSubjectDetails.OrderBy(x => x.Year).ThenBy(x => x.Sem).ThenBy(x => x.CourseName).ToList();

                TempData["SubjectList"] = lstSubjectDetails;
                TempData.Keep();


                int semCode = 0;
                for (int i = 0; i < lstSubjectDetails.Count; i++)
                {
                    if (i == 0)
                    {
                        lstSubjectDetails[i].RangeStart = RF;
                        lstSubjectDetails[i].RangeEnd = RF + lstSubjectDetails[i].Count;
                    }
                    else
                    {
                        int NextSt = WriteNextStartCode(lstSubjectDetails[i - 1].RangeEnd.ToString(), Gap);
                        lstSubjectDetails[i].RangeStart = NextSt;
                        lstSubjectDetails[i].RangeEnd = NextSt + lstSubjectDetails[i].Count;

                    }
                }

                foreach (var s in lstSubjectDetails)
                {
                    if (s.Year == 1 && s.Sem == 1)
                    {
                        semCode = 1;
                    }
                    if (s.Year == 1 && s.Sem == 2)
                    {
                        semCode = 2;
                    }

                    if (s.Year == 2 && s.Sem == 1)
                    {
                        semCode = 3;
                    }

                    if (s.Year == 2 && s.Sem == 2)
                    {
                        semCode = 4;
                    }


                    if (s.Year == 3 && s.Sem == 1)
                    {
                        semCode = 5;
                    }


                    if (s.Year == 3 && s.Sem == 2)
                    {
                        semCode = 6;
                    }


                    s.RangeStart = Convert.ToInt32(semCode.ToString("D" + 1) + s.RangeStart.ToString("D" + 7));
                    s.RangeEnd = Convert.ToInt32(semCode.ToString("D" + 1) + (s.RangeEnd-1).ToString("D" + 7));
                }

                return Json(lstSubjectDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ImportData()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveCodes(List<SubjectRangeEntity> lstSubjects, Int32 RangeFrom, Int32 Gap)
        {
            bool status = false;
            int RF = RangeFrom;
            MemoPrintService BoMemoService = new MemoPrintService();

            var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRange() ;
            lstSubjectDetails = lstSubjectDetails.OrderBy(x => x.Year).ThenBy(x => x.Sem).ThenBy(x => x.CourseName).ToList();
            TempData["SubjectList"] = lstSubjectDetails;
            TempData.Keep();

            int NextStartCode = 0;
            bool flag = true;
            int semCode = 0;
            for (int i = 0; i < lstSubjectDetails.Count; i++)
            {
               



                if (i == 0)
                {
                    lstSubjectDetails[i].RangeStart = RF;
                    lstSubjectDetails[i].RangeEnd = RF + lstSubjectDetails[i].Count;
                }
                else
                {
                    int NextSt = WriteNextStartCode(lstSubjectDetails[i - 1].RangeEnd.ToString(), Gap);
                    lstSubjectDetails[i].RangeStart = NextSt;
                    lstSubjectDetails[i].RangeEnd = NextSt + lstSubjectDetails[i].Count;

                }

              
            }

            foreach (var s in lstSubjectDetails)
            {
                if (s.Year == 1 && s.Sem == 1)
                {
                    semCode = 1;
                }
                if (s.Year == 1 && s.Sem == 2)
                {
                    semCode = 2;
                }

                if (s.Year == 2 && s.Sem == 1)
                {
                    semCode = 3;
                }

                if (s.Year == 2 && s.Sem == 2)
                {
                    semCode = 4;
                }


                if (s.Year == 3 && s.Sem == 1)
                {
                    semCode = 5;
                }


                if (s.Year == 3 && s.Sem == 2)
                {
                    semCode = 6;
                }


                s.RangeStart = Convert.ToInt32(semCode.ToString("D"+1) +  s.RangeStart.ToString("D"+7));
                s.RangeEnd = Convert.ToInt32(semCode.ToString("D" + 1) + (s.RangeEnd - 1).ToString("D" + 7));

                BoMemoService.getSubjectRangeInstance().InsertSubjectCode(s);
            }


            return new JsonResult { Data = new { status = status } };
        }

        private int WriteNextStartCode(string endCode, int Gap)
        {
            string index = endCode.Substring(endCode.Length - 2);
            int h = 100;
            int Rounding = h - Convert.ToInt32(index);

            return Convert.ToInt32(endCode) + Rounding + Gap +1      ;

        }



        [HttpPost]
        public JsonResult GetCodeDataYrs()
        {

            try
            {

                MemoPrintService BoMemoService = new MemoPrintService();
                var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRangeYr();
                lstSubjectDetails = lstSubjectDetails.OrderBy(x => x.CourseName).ThenBy(x => x.Year).ToList();

                var data = new { lstSubjectDetails= lstSubjectDetails, count = lstSubjectDetails.Sum(x=>x.Count) };
                return Json(data,  JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        [HttpPost]
        public JsonResult GenerateRangesCodeYrs(Int32 RangeFrom, Int32 Gap)
        {

            try
            {
                int RF = RangeFrom;

                MemoPrintService BoMemoService = new MemoPrintService();
                var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRangeYr();

                lstSubjectDetails = lstSubjectDetails.OrderBy(x => x.CourseName).ThenBy(x => x.Year).ToList();

                TempData["SubjectList"] = lstSubjectDetails;
                TempData.Keep();


                int yrCode = 0;
                for (int i = 0; i < lstSubjectDetails.Count; i++)
                {
                    if (i == 0)
                    {
                        lstSubjectDetails[i].RangeStart = RF;
                        lstSubjectDetails[i].RangeEnd = RF + lstSubjectDetails[i].Count;
                    }
                    else
                    {
                        int NextSt = WriteNextStartCode(lstSubjectDetails[i - 1].RangeEnd.ToString(), Gap);
                        lstSubjectDetails[i].RangeStart = NextSt;
                        lstSubjectDetails[i].RangeEnd = NextSt + lstSubjectDetails[i].Count;

                    }
                }

                foreach (var s in lstSubjectDetails)
                {
                    if (s.Year == 1 )
                    {
                        yrCode = 1;
                    }
                   

                    if (s.Year == 2 )
                    {
                        yrCode = 2;
                    }

                   


                    if (s.Year == 3)
                    {
                        yrCode = 3;
                    }


                  


                    s.RangeStart = Convert.ToInt32(yrCode.ToString("D" + 1) + s.RangeStart.ToString("D" + 7));
                    s.RangeEnd = Convert.ToInt32(yrCode.ToString("D" + 1) + (s.RangeEnd - 1).ToString("D" + 7));
                }

                var data = new { lstSubjectDetails = lstSubjectDetails, count = lstSubjectDetails.Sum(x => x.Count) };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult SaveYrsCodes( Int32 RangeFrom, Int32 Gap)
        {
            bool status = false;
            int RF = RangeFrom;
            MemoPrintService BoMemoService = new MemoPrintService();

            var lstSubjectDetails = BoMemoService.getSubjectRangeInstance().GetSubjectDetailsForRangeYr();
            lstSubjectDetails = lstSubjectDetails.OrderBy(x => x.CourseName).ThenBy(x => x.Year).ToList();
            TempData["SubjectList"] = lstSubjectDetails;
            TempData.Keep();

            int NextStartCode = 0;
            bool flag = true;
            int yrCode = 0;
            for (int i = 0; i < lstSubjectDetails.Count; i++)
            {




                if (i == 0)
                {
                    lstSubjectDetails[i].RangeStart = RF;
                    lstSubjectDetails[i].RangeEnd = RF + lstSubjectDetails[i].Count;
                }
                else
                {
                    int NextSt = WriteNextStartCode(lstSubjectDetails[i - 1].RangeEnd.ToString(), Gap);
                    lstSubjectDetails[i].RangeStart = NextSt;
                    lstSubjectDetails[i].RangeEnd = NextSt + lstSubjectDetails[i].Count;

                }


            }

            foreach (var s in lstSubjectDetails)
            {
                if (s.Year == 1)
                {
                    yrCode = 1;
                }


                if (s.Year == 2)
                {
                    yrCode = 2;
                }




                if (s.Year == 3)
                {
                    yrCode = 3;
                }




                s.RangeStart = Convert.ToInt32(yrCode.ToString("D" + 1) + s.RangeStart.ToString("D" + 7));
                s.RangeEnd = Convert.ToInt32(yrCode.ToString("D" + 1) + (s.RangeEnd - 1).ToString("D" + 7));

                BoMemoService.getSubjectRangeInstance().InsertSubjectCodeYear(s);
            }


            return new JsonResult { Data = new { status = status } };
        }



    }


}
