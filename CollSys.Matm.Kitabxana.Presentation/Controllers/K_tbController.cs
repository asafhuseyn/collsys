using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using CollSys.Matm.Kitabxana.BusinessLogic.Managers;
using CollSys.Matm.Kitabxana.BusinessLogic.Services;
using CollSys.Matm.Kitabxana.Entities.Tables;
using Matm.Kitabxana.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using WebDav;
using X.PagedList;


namespace CollSys.Matm.Kitabxana.Presentation.Controllers
{
    [Authorize]
    public class K_tbController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public K_tbController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public static void WriteLog(String message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
        public ViewResult Index
            (
            string sortOrder, int? page,
            string currentFilterForInventarNo, string currentFilterForDkNo, string currentFilterForUmumiAd, string currentFilterForIstehsalYeriId,
            string currentFilterForTehvilAktNo, string currentFilterForTehvilAlinmaTarixi,
            string currentFilterForYazarId, 
            //string currentFilterForMetbeeId,
            string currentFilterForDercOlunmaTarixi,
            string searchInputForInventarNo, string searchInputForDkNo, string searchInputForUmumiAd, string searchInputForIstehsalYeriId,
            string searchInputForTehvilAktNo, string searchInputForTehvilAlinmaTarixi,
            string searchInputForYazarId,  
            //string searchInputForMetbeeId
            string searchInputForDercOlunmaTarixi
            )
        {
            IRegionService regionService = new RegionManager();
            regionService.Instance(true);
            ViewBag.Regions = regionService.Read();

            IYazarService yazarService = new YazarManager();
            yazarService.Instance(true);
            ViewBag.Yazarlar = yazarService.Read();
            //IMetbeeService metbeeService = new MetbeeManager();
            //metbeeService.Instance(true);
            //ViewBag.Metbeeler = metbeeService.Read();
            
            ViewBag.CurrentSort = sortOrder;

            ViewBag.SortById = String.IsNullOrEmpty(sortOrder) ? "Id_desc" : "";
            ViewBag.SortByInventarNo = sortOrder == "InventarNo" ? "InventarNo_desc" : "InventarNo";
            ViewBag.SortByDkNo = sortOrder == "DkNo" ? "DkNo_desc" : "DkNo";
            ViewBag.SortByTehvilAktNo = sortOrder == "TehvilAktNo" ? "TehvilAktNo_desc" : "TehvilAktNo";
            ViewBag.SortByTehvilAlinmaTarixi = sortOrder == "TehvilAlinmaTarixi" ? "TehvilAlinmaTarixi_desc" : "TehvilAlinmaTarixi";
            ViewBag.SortByUmumiAd = sortOrder == "UmumiAd" ? "UmumiAd_desc" : "UmumiAd";
            ViewBag.SortByIstehsalYeriId = sortOrder == "IstehsalYeriId" ? "IstehsalYeriId_desc" : "IstehsalYeriId";
            ViewBag.SortByYazarId = sortOrder == "YazarId" ? "YazarId_desc" : "YazarId";
            //ViewBag.SortByMetbeeId = sortOrder == "MetbeeId" ? "MetbeeId_desc" : "MetbeeId";
            ViewBag.SortByDercOlunmaTarixi = sortOrder == "DercOlunmaTarixi" ? "DercOlunmaTarixi_desc" : "DercOlunmaTarixi";
            ViewBag.SortBySaxlanmaYeri = sortOrder == "SaxlanmaYeri" ? "SaxlanmaYeri_desc" : "SaxlanmaYeri";


            if (
                    searchInputForInventarNo != null
                    || searchInputForDkNo != null
                    || searchInputForIstehsalYeriId != null
                    || searchInputForYazarId != null
                    //|| searchInputForMetbeeId != null
                    || searchInputForDercOlunmaTarixi != null
                    || searchInputForTehvilAktNo != null
                    || searchInputForTehvilAlinmaTarixi != null
                    || searchInputForUmumiAd != null
                )
            {
                page = 1;
            }
            else
            {
                searchInputForInventarNo = currentFilterForInventarNo;
                searchInputForDkNo = currentFilterForDkNo;
                searchInputForIstehsalYeriId = currentFilterForIstehsalYeriId;
                searchInputForYazarId = currentFilterForYazarId;
                //searchInputForMetbeeId = currentFilterForMetbeeId;
                searchInputForDercOlunmaTarixi = currentFilterForDercOlunmaTarixi;
                searchInputForTehvilAktNo = currentFilterForTehvilAktNo;
                searchInputForTehvilAlinmaTarixi = currentFilterForTehvilAlinmaTarixi;
                searchInputForUmumiAd = currentFilterForUmumiAd;
            }

            ViewBag.CurrentFilterForInventarNo = searchInputForInventarNo;
            ViewBag.CurrentFilterForDkNo = searchInputForDkNo;
            ViewBag.CurrentFilterForIstehsalYeriId = searchInputForIstehsalYeriId;
            ViewBag.CurrentFilterForYazarId = searchInputForYazarId;
            //ViewBag.CurrentFilterForMetbeeId = searchInputForMetbeeId;
            ViewBag.CurrentFilterForDercOlunmaTarixi = searchInputForDercOlunmaTarixi;
            ViewBag.CurrentFilterForTehvilAktNo = searchInputForTehvilAktNo;
            ViewBag.CurrentFilterForTehvilAlinmaTarixi = searchInputForTehvilAlinmaTarixi;
            ViewBag.CurrentFilterForUmumiAd = searchInputForUmumiAd;


            IExhibitService exhibitService = new ExhibitManager();
            exhibitService.Instance(true);
            var exhibits = exhibitService.Read(c => c.Format == Format.K_tb);
            if (exhibits == null)
            {
                return View();
            }
            IEnumerable<ExhibitModel> index;

            if (
                (!String.IsNullOrEmpty(searchInputForInventarNo))
                || (!String.IsNullOrEmpty(searchInputForDkNo))
                || (!String.IsNullOrEmpty(searchInputForIstehsalYeriId))
                || (!String.IsNullOrEmpty(searchInputForYazarId))
                //|| (!String.IsNullOrEmpty(searchInputForMetbeeId))
                || (!String.IsNullOrEmpty(searchInputForDercOlunmaTarixi))
                || (!String.IsNullOrEmpty(searchInputForTehvilAktNo))
                || (!String.IsNullOrEmpty(searchInputForTehvilAlinmaTarixi))
                || (!String.IsNullOrEmpty(searchInputForUmumiAd))
                )
            {

                string InventarNo = null;
                int? DkNo = null;
                int? IstehsalYeriId = null;
                int? YazarId = null;
                //int? MetbeeId = null;
                string DercOlunmaTarixi = null;
                int? TehvilAktNo = null;
                DateTime? TehvilAlinmaTarixi = null;
                string UmumiAd = null;

                if (!string.IsNullOrEmpty(searchInputForInventarNo))
                {
                    InventarNo = searchInputForInventarNo;
                }

                if (!string.IsNullOrEmpty(searchInputForDkNo))
                {
                    DkNo = Convert.ToInt32(searchInputForDkNo);
                }

                if (!string.IsNullOrEmpty(searchInputForIstehsalYeriId))
                {
                    IstehsalYeriId = Convert.ToInt32(searchInputForIstehsalYeriId);
                }
                
                if (!string.IsNullOrEmpty(searchInputForYazarId))
                {
                    YazarId = Convert.ToInt32(searchInputForYazarId);
                }
                //if (!string.IsNullOrEmpty(searchInputForMetbeeId))
                //{
                //    MetbeeId = Convert.ToInt32(searchInputForMetbeeId);
                //}

                if (!string.IsNullOrEmpty(searchInputForDercOlunmaTarixi))
                {
                    DercOlunmaTarixi = searchInputForDercOlunmaTarixi;
                }

                if (!string.IsNullOrEmpty(searchInputForTehvilAktNo))
                {
                    TehvilAktNo = Convert.ToInt32(searchInputForTehvilAktNo);
                }

                if (!string.IsNullOrEmpty(searchInputForTehvilAlinmaTarixi))
                {
                    TehvilAlinmaTarixi = Convert.ToDateTime(searchInputForTehvilAlinmaTarixi);
                }

                if (!string.IsNullOrEmpty(searchInputForUmumiAd))
                {
                    UmumiAd = searchInputForUmumiAd;
                }

                var accurateResults = exhibits.Where
                    (
                        c => (c.Format == Format.K_tb)
                                && (InventarNo == null || (c.InventarNo?.Contains(InventarNo) ?? false))
                                && (DkNo == null || c.DkNo == DkNo)
                                 && (IstehsalYeriId == null || c.IstehsalYeriId == IstehsalYeriId)
                                 && (YazarId == null || c.YazarId == YazarId)
                                //&& (MetbeeId == null || c.MetbeeId == MetbeeId)
                                && (DercOlunmaTarixi == null || (c.DercOlunmaTarixi?.Contains(DercOlunmaTarixi) ?? false))
                                && (TehvilAktNo == null || c.TehvilAktNo == TehvilAktNo)
                                && (TehvilAlinmaTarixi == null || c.TehvilAlinmaTarixi == TehvilAlinmaTarixi)
                                && (UmumiAd == null || (c.UmumiAd?.Contains(UmumiAd) ?? false))
                    );

                if (accurateResults.Count() == 0)
                {
                    var generalResults = exhibits.Where
                        (
                            c => (c.Format == Format.K_tb)
                                && ((InventarNo != null && (c.InventarNo?.Contains(InventarNo) ?? false))
                                || (DkNo != null && c.DkNo == DkNo)
                                || (IstehsalYeriId != null && c.IstehsalYeriId == IstehsalYeriId)
                                || (YazarId != null && c.YazarId == YazarId)
                                //|| (MetbeeId != null && c.MetbeeId == MetbeeId)
                                || (DercOlunmaTarixi != null && (c.DercOlunmaTarixi?.Contains(DercOlunmaTarixi) ?? false))
                                || (TehvilAktNo != null && c.TehvilAktNo == TehvilAktNo)
                                || (TehvilAlinmaTarixi != null && c.TehvilAlinmaTarixi == TehvilAlinmaTarixi)
                                || (UmumiAd != null && (c.UmumiAd?.Contains(UmumiAd) ?? false))
                                )
                        );

                    index = generalResults;
                }
                else
                {
                    index = accurateResults;
                }

            }
            else
            {
                index = exhibits;
            }

            index = sortOrder switch
            {
                "InventarNo_desc" => index.OrderByDescending(s => s.InventarNo).ToList(),
                "InventarNo" => index.OrderBy(s => s.InventarNo).ToList(),
                "DkNo_desc" => index.OrderByDescending(s => s.DkNo).ToList(),
                "DkNo" => index.OrderBy(s => s.DkNo).ToList(),
                "TehvilAktNo_desc" => index.OrderByDescending(s => s.TehvilAktNo).ToList(),
                "TehvilAktNo" => index.OrderBy(s => s.TehvilAktNo).ToList(),
                "TehvilAlinmaTarixi_desc" => index.OrderByDescending(s => s.TehvilAlinmaTarixi).ToList(),
                "TehvilAlinmaTarixi" => index.OrderBy(s => s.TehvilAlinmaTarixi).ToList(),
                "UmumiAd_desc" => index.OrderByDescending(s => s.UmumiAd).ToList(),
                "UmumiAd" => index.OrderBy(s => s.UmumiAd).ToList(),
                "IstehsalYeriId_desc" => index.OrderByDescending(s => s.IstehsalYeriId).ToList(),
                "IstehsalYeriId" => index.OrderBy(s => s.IstehsalYeriId).ToList(),
                "YazarId_desc" => index.OrderByDescending(s => s.YazarId).ToList(),
                "YazarId" => index.OrderBy(s => s.YazarId).ToList(),
                //"MetbeeId_desc" => index.OrderByDescending(s => s.MetbeeId).ToList(),
                //"MetbeeId" => index.OrderBy(s => s.MetbeeId).ToList(),
                "DercOlunmaTarixi_desc" => index.OrderByDescending(s => s.DercOlunmaTarixi).ToList(),
                "DercOlunmaTarixi" => index.OrderBy(s => s.DercOlunmaTarixi).ToList(),
                "SaxlanmaYeri_desc" => index.OrderByDescending(s => s.SaxlanmaYeri).ToList(),
                "SaxlanmaYeri" => index.OrderBy(s => s.SaxlanmaYeri).ToList(),
                _ => index.OrderByDescending(s => s.ModificationTime).ThenByDescending(s=>s.Id).ToList()
            };
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.ToplamSonuc = index.Count();
            return View(index.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Search()
        {
            IRegionService regionService = new RegionManager();
            regionService.Instance(true);
            ViewBag.Regions = regionService.Read();
            
            IYazarService yazarService = new YazarManager();
            yazarService.Instance(true);
            ViewBag.Yazarlar = yazarService.Read();
            //IMetbeeService metbeeService = new MetbeeManager();
            //metbeeService.Instance(true);
            //ViewBag.Metbeeler = metbeeService.Read();
            
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(ExhibitModel exhibit)
        {
            IExhibitService exhibitService = new ExhibitManager();
            exhibitService.Instance(true);
            List<ExhibitModel> searchResults = new List<ExhibitModel>();

            var accurateResults = exhibitService.Read(c => (c.Format == Format.K_tb)
                            && ((exhibit.InventarNo != null && c.InventarNo == exhibit.InventarNo)
                            && (exhibit.DkNo != null && c.DkNo == exhibit.DkNo)
                            && (exhibit.IstehsalYeriId != null && c.IstehsalYeriId == exhibit.IstehsalYeriId)
                            && (exhibit.YazarId != null && c.YazarId == exhibit.YazarId)
                            //&& (exhibit.MetbeeId != null && c.MetbeeId == exhibit.MetbeeId)
                            && (exhibit.DercOlunmaTarixi != null && c.DercOlunmaTarixi == exhibit.DercOlunmaTarixi)
                            && (exhibit.TehvilAktNo != null && c.TehvilAktNo == exhibit.TehvilAktNo)
                            && (exhibit.TehvilAlinmaTarixi != null && c.TehvilAlinmaTarixi == exhibit.TehvilAlinmaTarixi)
                            && (exhibit.UmumiAd != null && c.UmumiAd == exhibit.UmumiAd)
                            ));


            if (accurateResults != null)
            {
                searchResults = accurateResults;
            }
            else
            {
                var generalResults = exhibitService.Read(c => (c.Format == Format.K_tb)
                                && ((exhibit.InventarNo != null && c.InventarNo == exhibit.InventarNo)
                                || (exhibit.DkNo != null && c.DkNo == exhibit.DkNo)
                                || (exhibit.IstehsalYeriId != null && c.IstehsalYeriId == exhibit.IstehsalYeriId)
                                || (exhibit.YazarId != null && c.YazarId == exhibit.YazarId)
                                //|| (exhibit.MetbeeId != null && c.MetbeeId == exhibit.MetbeeId)
                                || (exhibit.DercOlunmaTarixi != null && c.DercOlunmaTarixi == exhibit.DercOlunmaTarixi)
                                || (exhibit.TehvilAktNo != null && c.TehvilAktNo == exhibit.TehvilAktNo)
                                || (exhibit.TehvilAlinmaTarixi != null && c.TehvilAlinmaTarixi == exhibit.TehvilAlinmaTarixi)
                                || (exhibit.UmumiAd != null && c.UmumiAd == exhibit.UmumiAd)
                                ));

                searchResults = generalResults;
            }

            return View("Index",searchResults as IPagedList<ExhibitModel>);
        }

        // GET: K_tb/Details/5
        public ActionResult Details(int id)
        {
            IExhibitService exhibitService = new ExhibitManager();
            exhibitService.Instance(true);
            var exhibit = exhibitService.Take(c => c.Id == id);
            if (exhibit == null)
            {
                return View("Error");
            }
            else
            {
                if (exhibit.CurrencyId != null)
                {
                    ICurrencyService currencyService = new CurrencyManager();
                    currencyService.Instance(true);
                    ViewBag.Currency = currencyService.Take(c => c.Id == exhibit.CurrencyId).Currency;
                }

                if (exhibit.SaxlanmaVeziyyetiId != null)
                {
                    ISaxlanmaVeziyyetiService saxlanmaVeziyyetiService = new SaxlanmaVeziyyetiManager();
                    saxlanmaVeziyyetiService.Instance(true);
                    ViewBag.SaxlanmaVeziyyeti = saxlanmaVeziyyetiService.Take(c => c.Id == exhibit.SaxlanmaVeziyyetiId)
                        .SaxlanmaVeziyyeti;
                }

                IMeasurementUnitService measurementUnitService = new MeasurementUnitManager();
                measurementUnitService.Instance(true);
                if (exhibit.EnUnitId != null)
                    ViewBag.EnUnit = measurementUnitService.Take(c => c.Id == exhibit.EnUnitId).MeasurementUnit;

                if (exhibit.UzunluqUnitId != null)
                    ViewBag.UzunluqUnit =
                        measurementUnitService.Take(c => c.Id == exhibit.UzunluqUnitId).MeasurementUnit;
                if (exhibit.HundurlukUnitId != null)
                    ViewBag.HundurlukUnit = measurementUnitService.Take(c => c.Id == exhibit.HundurlukUnitId)
                        .MeasurementUnit;
                if (exhibit.DiametrUnitId != null)
                    ViewBag.DiametrUnit =
                        measurementUnitService.Take(c => c.Id == exhibit.DiametrUnitId).MeasurementUnit;

                if (exhibit.MaterialId != null)
                {
                    IMaterialService materialService = new MaterialManager();
                    materialService.Instance(true);
                    ViewBag.Material = materialService.Take(c => c.Id == exhibit.MaterialId).Material;
                }
                
                if (exhibit.YazarId != null)
                {
                    IYazarService yazarService = new YazarManager();
                    yazarService.Instance(true);
                    ViewBag.Yazar = yazarService.Take(c => c.Id == exhibit.YazarId).Yazar;
                }
                if (exhibit.MetbeeId != null)
                {
                    IMetbeeService metbeeService = new MetbeeManager();
                    metbeeService.Instance(true);
                    ViewBag.Metbee = metbeeService.Take(c => c.Id == exhibit.MetbeeId).Metbee;
                }

                IRegionService regionService = new RegionManager();
                regionService.Instance(true);
                if (exhibit.IstehsalYeriId != null)
                    ViewBag.IstehsalYeri = regionService.Take(c => c.Id == exhibit.IstehsalYeriId).Region;

                IImageService imageService = new ImageManager();
                imageService.Instance(true);

                var images = imageService.Read(c => c.ExhibitId == id);
                if (images.Count > 0)
                {
                    var listImagePathes = new List<string>();
                    foreach (var item in images)
                    {
                        listImagePathes.Add(Path.Combine(item.Folder, item.Name));
                    }

                    ViewBag.Images = listImagePathes;
                }

                return View(exhibit);
            }
        }

        // GET: K_tb/Create
        public ActionResult Create(string InventarNo, int? DkNo, string UmumiAd, int? TehvilAktNo, string TehvilAlinmaTarixi,
            int? Qiymet, int? CurrencyId, int? Miqdar, string Menbe, string SaxlanmaYeri, int? SaxlanmaVeziyyetiId,
            int? En, int? EnUnitId, int? Uzunluq, int? UzunluqUnitId, int? Hundurluk, int? HundurlukUnitId, int? Diametr, int? DiametrUnitId,
            int? MaterialId, int? YazarId, int? MetbeeId, int? SehifeSayi, string ISBN, string DercOlunmaTarixi, int? IstehsalYeriId, string Tesvir, string hata)
        {
            ViewBag.InventarNo = InventarNo;
            ViewBag.DkNo = DkNo;
            ViewBag.UmumiAd = UmumiAd;

            ViewBag.YazarId = YazarId;
            ViewBag.MetbeeId = MetbeeId;
            ViewBag.SehifeSayi = SehifeSayi;
            ViewBag.DercOlunmaTarixi = DercOlunmaTarixi;
            ViewBag.ISBN = ISBN;
            
            ViewBag.TehvilAktNo = TehvilAktNo;
            ViewBag.TehvilAlinmaTarixi = TehvilAlinmaTarixi;

            ViewBag.Qiymet = Qiymet;
            ViewBag.CurrencyId = CurrencyId;
            ViewBag.Miqdar = Miqdar;
            ViewBag.Menbe = Menbe;
            ViewBag.SaxlanmaYeri = SaxlanmaYeri;
            ViewBag.SaxlanmaVeziyyetiId = SaxlanmaVeziyyetiId;
            ViewBag.En = En;
            ViewBag.EnUnitId = EnUnitId;
            ViewBag.Uzunluq = Uzunluq;
            ViewBag.UzunluqUnitId = UzunluqUnitId;
            ViewBag.Hundurluk = Hundurluk;
            ViewBag.HundurlukUnitId = HundurlukUnitId;
            ViewBag.Diametr = Diametr;
            ViewBag.DiametrUnitId = DiametrUnitId;
            ViewBag.MaterialId = MaterialId;
            ViewBag.IstehsalYeriId = IstehsalYeriId;
            ViewBag.Tesvir = Tesvir;
            ViewBag.Hata = hata;

            IRegionService regionService = new RegionManager();
            regionService.Instance(true);

            ICurrencyService currencyService = new CurrencyManager();
            currencyService.Instance(true);

            IMaterialService materialService = new MaterialManager();
            materialService.Instance(true);

            IYazarService yazarService = new YazarManager();
            yazarService.Instance(true);
            IMetbeeService metbeeService = new MetbeeManager();
            metbeeService.Instance(true);
            
            ISaxlanmaVeziyyetiService saxlanmaVeziyyetiService = new SaxlanmaVeziyyetiManager();
            saxlanmaVeziyyetiService.Instance(true);

            IMeasurementUnitService measurementUnitService = new MeasurementUnitManager();
            measurementUnitService.Instance(true);

            ViewBag.Regions = regionService.Read();
            ViewBag.Currencies = currencyService.Read();
            ViewBag.Materials = materialService.Read();
            ViewBag.Yazarlar = yazarService.Read();
            ViewBag.Metbeeler = metbeeService.Read();
            ViewBag.SaxlanmaVeziyyetleri = saxlanmaVeziyyetiService.Read();
            ViewBag.MeasurementUnits = measurementUnitService.Read();
            
            return View();
        }

        // POST: K_tb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExhibitModel exhibit, DateTime? TehvilAlinmaTarixi, IFormFile[] files)
        {
            string webRoot = _webHostEnvironment.WebRootPath;

            exhibit.CreatorUser = User.Identity.Name;
            exhibit.ModifierUser = User.Identity.Name;
            exhibit.CreationTime = DateTime.Now;
            exhibit.ModificationTime = DateTime.Now;
            exhibit.Format = Format.K_tb;
            exhibit.TehvilAlinmaTarixi = TehvilAlinmaTarixi;

            IExhibitService exhibitService = new ExhibitManager();
            exhibitService.Instance(true);

            try
            {
                exhibitService.Create(exhibit);
                exhibitService.Save();
                WriteLog(" = Success = Exhibit with Id: " + exhibit.Id + " was saved to database by " + User.Identity.Name );
            }
            catch (Exception e)
            {
                WriteLog(" = FAILED = Exhibit with Id: " + exhibit.Id + " was not saved to database. User: " + User.Identity.Name + "Error: " + e);
                
                string dateS = null;



                if (exhibit.TehvilAlinmaTarixi.HasValue)
                {
                    string dayS = exhibit.TehvilAlinmaTarixi.Value.Day.ToString();
                    string monthS = exhibit.TehvilAlinmaTarixi.Value.Month.ToString();
                    string yearS = exhibit.TehvilAlinmaTarixi.Value.Year.ToString();

                    int monthI = Convert.ToInt32(monthS);
                    int dayI = Convert.ToInt32(dayS);

                    if (monthI < 10)
                    {
                        monthS = "0" + monthS;
                    }
                    if (dayI < 10)
                    {
                        dayS = "0" + dayS;

                    }
                    dateS = yearS + "-" + monthS + "-" + dayS;
                }

                string hata = " *İnventar nömrəsi təkrarlana bilməz!";
                return Redirect("~/K_tb/Create" +
                    "?InventarNo=" + WebUtility.UrlEncode(exhibit.InventarNo) +
                    "&DkNo=" + exhibit.DkNo +
                    "&UmumiAd=" + WebUtility.UrlEncode(exhibit.UmumiAd) +
                    "&TehvilAktNo=" + exhibit.TehvilAktNo +
                    "&TehvilAlinmaTarixi=" + dateS +
                    "&Qiymet=" + exhibit.Qiymet +
                    "&CurrencyId=" + exhibit.CurrencyId +
                    "&Miqdar=" + exhibit.Miqdar +
                    "&Menbe=" + WebUtility.UrlEncode(exhibit.Menbe) +
                    "&SaxlanmaYeri=" + WebUtility.UrlEncode(exhibit.SaxlanmaYeri) +
                    "&SaxlanmaVeziyyetiId=" + exhibit.SaxlanmaVeziyyetiId +
                    "&En=" + exhibit.En +
                    "&EnUnitId=" + exhibit.EnUnitId +
                    "&Uzunluq=" + exhibit.Uzunluq +
                    "&UzunluqUnitId=" + exhibit.UzunluqUnitId +
                    "&Hundurluk=" + exhibit.Hundurluk +
                    "&HundurlukUnitId=" + exhibit.HundurlukUnitId +
                    "&Diametr=" + exhibit.Diametr +
                    "&DiametrUnitId=" + exhibit.DiametrUnitId +
                    "&MaterialId=" + exhibit.MaterialId +
                    "&YazarId=" + exhibit.YazarId +
                    "&MetbeeId=" + exhibit.MetbeeId +
                    "&SehifeSayi=" + exhibit.SehifeSayi +
                    "&DercOlunmaTarixi=" + WebUtility.UrlEncode(exhibit.DercOlunmaTarixi) +
                    "&ISBN=" + WebUtility.UrlEncode(exhibit.ISBN) +
                    "&IstehsalYeriId=" + exhibit.IstehsalYeriId +
                    "&Tesvir=" + WebUtility.UrlEncode(exhibit.Tesvir) +
                    "&hata=" + WebUtility.UrlEncode(hata)
                    );
            }


            if (files.Length > 0)
            {
                IImageService imageService = new ImageManager();
                imageService.Instance(true);

                // BEGIN_OWNCLOUD
                var clientParams = new WebDavClientParams
                {
                    BaseAddress = new Uri("<YOUR_OWNCLOUD_PATH>/"),
                    Credentials = new NetworkCredential("<YOUR_OWNCLOUD_USERNAME>", "<YOUR_OWNCLOUD_PASSWORD>")
                };
                var client = new WebDavClient(clientParams);
                // END_OWNCLOUD

                int i = 1;
                foreach (var file in files)
                {
                    ImageModel image = new ImageModel();

                    string fileName = exhibit.Id + "_" + i + ".jpg";
                    string folderName = Convert.ToString(exhibit.Id);
                    string pathName = image.Path;
                    
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(webRoot, pathName, folderName));
                        string path = Path.Combine(webRoot, pathName, folderName, fileName);

                        var img = Image.Load(file.OpenReadStream());

                        if (img.Width > img.Height)
                        {
                            if (img.Width > 7680 || img.Height > 4320)
                            {
                                img.Mutate(ctx => ctx.Resize(7680, 4320));
                            }

                        }
                        else if (img.Width < img.Height)
                        {
                            if (img.Width > 4320 || img.Height > 7680)
                            {
                                img.Mutate(ctx => ctx.Resize(4320, 7680));
                            }
                        }
                        else
                        {
                            if (img.Width > 7680 || img.Height > 7680)
                            {
                                img.Mutate(ctx => ctx.Resize(7680, 7680));
                            }
                        }


                        // BEGIN_OWNCLOUD
                        string pathForOwnCloud = "<YOUR_OWNCLOUD_PATH>/CollSys/Kitabxana/" + pathName + "/Originals/" + folderName;
                        client.Mkcol(pathForOwnCloud); // create a directory
                        Stream imageForCloud = new MemoryStream();
                        img.SaveAsJpeg(imageForCloud);
                        imageForCloud.Position = 0;
                        client.PutFile(pathForOwnCloud + "/" + fileName, imageForCloud);
                        WriteLog(" = SUCCESS = Images of exhibit with Id: " + exhibit.Id + " was saved to owncloud by " + User.Identity.Name );
                        // END_OWNCLOUD

                        if (img.Width > img.Height)
                        {
                            if (img.Width > 720 || img.Height > 480)
                            {
                                img.Mutate(ctx => ctx.Resize(720, 480));
                            }

                        }
                        else if (img.Width < img.Height)
                        {
                            if (img.Width > 480 || img.Height > 720)
                            {
                                img.Mutate(ctx => ctx.Resize(480, 720));
                            }
                        }
                        else
                        {
                            if (img.Width > 720 || img.Height > 720)
                            {
                                img.Mutate(ctx => ctx.Resize(720, 720));
                            }
                        }

                        img.Save(path);
                        WriteLog(" = SUCCESS = Images of exhibit with Id: " + exhibit.Id + " was saved to base path by " + User.Identity.Name );

                        image.Name = fileName;
                        image.Folder = folderName;
                        image.Path = pathName;
                        image.ExhibitId = exhibit.Id;
                        imageService.Create(image);
                        imageService.Save();
                        WriteLog(" = SUCCESS = Images of exhibit with Id: " + exhibit.Id + " was saved to database by " + User.Identity.Name );
                    }
                    catch (Exception e)
                    {
                        WriteLog(" = FAILED = Images of exhibit with Id: " + exhibit.Id + " was not saved. User: " + User.Identity.Name + "Error: " + e);
                        return View("Error");
                    }

                    image = null;

                    i++;
                }
            }



            return RedirectToAction("Index");
        }

        // GET: K_tb/Edit/5
        public ActionResult Edit(int id, string hata)
        {
            IExhibitService exhibitService = new ExhibitManager();
            exhibitService.Instance(true);
            var edit = exhibitService.Take(c => c.Id == id);
            
            if (edit == null)
            {
                return View("Error");
            }
            else
            {
                ViewBag.Hata = hata;

                IRegionService regionService = new RegionManager();
                regionService.Instance(true);

                ICurrencyService currencyService = new CurrencyManager();
                currencyService.Instance(true);

                IMaterialService materialService = new MaterialManager();
                materialService.Instance(true);
                
                IYazarService yazarService = new YazarManager();
                yazarService.Instance(true);
                
                IMetbeeService metbeeService = new MetbeeManager();
                metbeeService.Instance(true);
                
                ISaxlanmaVeziyyetiService saxlanmaVeziyyetiService = new SaxlanmaVeziyyetiManager();
                saxlanmaVeziyyetiService.Instance(true);

                IMeasurementUnitService measurementUnitService = new MeasurementUnitManager();
                measurementUnitService.Instance(true);

                ViewBag.Regions = regionService.Read();
                ViewBag.Currencies = currencyService.Read();
                ViewBag.Materials = materialService.Read();
                ViewBag.Yazarlar = yazarService.Read();
                ViewBag.Metbeeler = metbeeService.Read();
                ViewBag.SaxlanmaVeziyyetleri = saxlanmaVeziyyetiService.Read();
                ViewBag.MeasurementUnits = measurementUnitService.Read();


                ViewBag.InventarNo = edit.InventarNo;
                ViewBag.DkNo = edit.DkNo;


                IImageService imageService = new ImageManager();
                imageService.Instance(true);
                var images = imageService.Read(c => c.ExhibitId == id);
                var listImagePathes = new List<string>();
                foreach (var item in images)
                {
                    listImagePathes.Add(Path.Combine(item.Folder, item.Name));
                }

                ViewBag.Images = listImagePathes;

                return View(edit);
            }
        }

        // POST: K_tb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExhibitModel exhibit, IFormCollection formCollection, IFormFile[] files, DateTime? tehvilAlinmaTarixi)
        {
            string webRoot = _webHostEnvironment.WebRootPath;

            if (exhibit.InventarNo == null)
            {
                string hata = " *İnventar nömrəsi boş saxlana bilməz!";
                return Redirect("~/K_tb/Edit/" + exhibit.Id + "?hata=" + WebUtility.UrlEncode(hata));
            }

            exhibit.ModifierUser = User.Identity.Name;
            exhibit.ModificationTime = DateTime.Now;

            exhibit.Format = Format.K_tb;

            exhibit.TehvilAlinmaTarixi = tehvilAlinmaTarixi;


            IExhibitService exhibitService = new ExhibitManager();
            exhibitService.Instance(true);

            ExhibitModel nonEditedExibit = exhibitService.Take(c => c.Id == exhibit.Id);

            if (exhibit.InventarNo != nonEditedExibit.InventarNo)
            {
                List<ExhibitModel> liste = exhibitService.Read(c => c.InventarNo == exhibit.InventarNo).Where(c => c.Format == exhibit.Format).ToList();

                if (liste.Count != 0)
                {
                    string hata = " *İnventar nömrəsi təkrarlana bilməz!";
                    return Redirect("~/K_tb/Edit/" + exhibit.Id + "?hata=" + WebUtility.UrlEncode(hata));
                }

            }

            exhibitService.Detach(nonEditedExibit);
            try
            {
                exhibitService.Update(exhibit);
                exhibitService.Save();
                WriteLog(" = Success = Exhibit with Id: " + exhibit.Id + " was updated in database by " + User.Identity.Name );
            }
            catch (Exception e)
            {
                WriteLog(" = FAILED = Exhibit with Id: " + exhibit.Id + " was not updated in database. User: " + User.Identity.Name + "Error: " + e);
                return View("Error");
            }
            

            IImageService imageService = new ImageManager();
            imageService.Instance(true);

            if (formCollection["selectedImages[]"].Count != 0)
            {
                // BEGIN_OWNCLOUD
                var clientParams = new WebDavClientParams
                {
                    BaseAddress = new Uri("<YOUR_OWNCLOUD_PATH>/"),
                    Credentials = new NetworkCredential("<YOUR_OWNCLOUD_USERNAME>", "<YOUR_OWNCLOUD_PASSWORD>")
                };
                var client = new WebDavClient(clientParams);
                // END_OWNCLOUD
                
                string[] images = Convert.ToString(formCollection["selectedImages[]"]).Split(',');

                foreach (string imagePath in images)
                {
                    string fullPath = Path.Combine(webRoot, "Images/" + imagePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        WriteLog(" = Success = Path: " + fullPath + " was deleted from base directory by " + User.Identity.Name );
                    }
                    
                    // BEGIN_OWNCLOUD
                    string pathForOwnCloud = "<YOUR_OWNCLOUD_PATH>/CollSys/Kitabxana/Images/Originals/" + imagePath;
                    client.Delete(pathForOwnCloud);
                    WriteLog(" = Success = Path: " + pathForOwnCloud + " was deleted from owncloud by " + User.Identity.Name );
                    // END_OWNCLOUD
                    
                    var seperated = imagePath.Split(Path.DirectorySeparatorChar);
                    var imageName = seperated[^1];
                    var img = imageService.Take(c => c.Name == imageName);
                    imageService.Delete(img);
                    imageService.Save();
                    WriteLog(" = Success = Images of exhibit with Id: " + exhibit.Id + " was deleted from database by " + User.Identity.Name );
                }
            }

            if (files.Length > 0)
            {
                var listOfImages = imageService.Read(c => c.ExhibitId == exhibit.Id);
                int maxValue;
                if (listOfImages.Count > 0)
                {
                    int[] imageNames = new int[listOfImages.Count];
                    for (int i = 0; i < listOfImages.Count; ++i)
                    {
                        int indexOfDot = listOfImages[i].Name.IndexOf('.');
                        string nameWithoutExtension = listOfImages[i].Name.Substring(0, indexOfDot);
                        imageNames[i] = Convert.ToInt32(nameWithoutExtension.Split('_')[1]);
                    }
                    maxValue = imageNames.Max();
                }
                else
                {
                    maxValue = 0;
                }

                // BEGIN_OWNCLOUD
                var clientParams = new WebDavClientParams
                {
                    BaseAddress = new Uri("<YOUR_OWNCLOUD_PATH>/"),
                    Credentials = new NetworkCredential("<YOUR_OWNCLOUD_USERNAME>", "<YOUR_OWNCLOUD_PASSWORD>")
                };
                var client = new WebDavClient(clientParams);
                // END_OWNCLOUD

                int j = maxValue + 1;
                foreach (var file in files)
                {
                    ImageModel image = new ImageModel();

                    string fileName = exhibit.Id + "_" + j + ".jpeg";
                    string folderName = Convert.ToString(exhibit.Id);
                    string pathName = image.Path;
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(webRoot, pathName, folderName));
                        string path = Path.Combine(webRoot, pathName, folderName, fileName);

                        var img = Image.Load(file.OpenReadStream());

                        if (img.Width > img.Height)
                        {
                            if (img.Width > 7680 || img.Height > 4320)
                            {
                                img.Mutate(ctx => ctx.Resize(7680, 4320));
                            }

                        }
                        else if (img.Width < img.Height)
                        {
                            if (img.Width > 4320 || img.Height > 7680)
                            {
                                img.Mutate(ctx => ctx.Resize(4320, 7680));
                            }
                        }
                        else
                        {
                            if (img.Width > 7680 || img.Height > 7680)
                            {
                                img.Mutate(ctx => ctx.Resize(7680, 7680));
                            }
                        }


                        // BEGIN_OWNCLOUD
                        string pathForOwnCloud = "<YOUR_OWNCLOUD_PATH>/CollSys/Kitabxana/" + pathName + "/Originals/" + folderName;
                        client.Mkcol(pathForOwnCloud); // create a directory
                        Stream imageForCloud = new MemoryStream();
                        img.SaveAsJpeg(imageForCloud);
                        imageForCloud.Position = 0;
                        client.PutFile(pathForOwnCloud + "/" + fileName, imageForCloud);
                        WriteLog(" = SUCCESS = Images of exhibit with Id: " + exhibit.Id + " was saved to owncloud by " + User.Identity.Name );
                        // END_OWNCLOUD

                        if (img.Width > img.Height)
                        {
                            if (img.Width > 7680 || img.Height > 4320)
                            {
                                img.Mutate(ctx => ctx.Resize(7680, 4320));
                            }

                        }
                        else if (img.Width < img.Height)
                        {
                            if (img.Width > 4320 || img.Height > 7680)
                            {
                                img.Mutate(ctx => ctx.Resize(4320, 7680));
                            }
                        }
                        else
                        {
                            if (img.Width > 7680 || img.Height > 7680)
                            {
                                img.Mutate(ctx => ctx.Resize(7680, 7680));
                            }
                        }

                        img.Save(path);
                        WriteLog(" = SUCCESS = Images of exhibit with Id: " + exhibit.Id + " was saved to base directory by " + User.Identity.Name );

                        image.Name = fileName;
                        image.Folder = folderName;
                        image.Path = pathName;
                        image.ExhibitId = exhibit.Id;
                        imageService.Create(image);
                        imageService.Save();
                        WriteLog(" = SUCCESS = Images of exhibit with Id: " + exhibit.Id + " was saved to database by " + User.Identity.Name );
                    }
                    catch (Exception e)
                    {
                        WriteLog(" = FAILED = Images of exhibit with Id: " + exhibit.Id + " was not saved. User: " + User.Identity.Name + "Error: " + e);
                        return View("Error");
                    }


                    image = null;

                    j++;
                }
            }


            return RedirectToAction("Index");
        }

        // GET: K_tb/Delete/5
        public ActionResult Delete(int id)
        {
            IExhibitService exhibitService = new ExhibitManager();
            exhibitService.Instance(true);
            var exhibit = exhibitService.Take(c => c.Id == id);
            
            if (exhibit == null)
            {
                return View("Error");
            }
            else
            {
                return View(exhibit);
            }
        }

        // POST: K_tb/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ExhibitModel exhibit)
        {
            string webRoot = _webHostEnvironment.WebRootPath;

            IExhibitService exhibitService = new ExhibitManager();
            exhibitService.Instance(true);

            try
            {
                exhibitService.Delete(exhibit);
                exhibitService.Save();
                WriteLog(" = SUCCESS = Exhibit with Id: " + exhibit.Id + " was deleted from database by " + User.Identity.Name );

                // BEGIN_OWNCLOUD
                var clientParams = new WebDavClientParams
                {
                    BaseAddress = new Uri("<YOUR_OWNCLOUD_PATH>/"),
                    Credentials = new NetworkCredential("<YOUR_OWNCLOUD_USERNAME>", "<YOUR_OWNCLOUD_PASSWORD>")
                };
                var client = new WebDavClient(clientParams);
                // END_OWNCLOUD
                
                if (Directory.Exists(Path.Combine(webRoot, "Images/", Convert.ToString(exhibit.Id))))
                {
                    Directory.Delete(Path.Combine(webRoot, "Images/", Convert.ToString(exhibit.Id)), true);
                    WriteLog(" = SUCCESS = Image folder of exhibit with Id: " + exhibit.Id + " was deleted from base directory by " + User.Identity.Name );
                }
                
                // BEGIN_OWNCLOUD
                string pathForOwnCloud = "<YOUR_OWNCLOUD_PATH>/CollSys/Kitabxana/Images/Originals/" + Convert.ToString(exhibit.Id);
                client.Delete(pathForOwnCloud);
                WriteLog(" = SUCCESS = Image folder of exhibit with Id: " + exhibit.Id + " was deleted from owncloud by " + User.Identity.Name );
                // END_OWNCLOUD

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                WriteLog(" = FAILED = Exhibit with Id: " + exhibit.Id + " or its image folder was not deleted. User: " + User.Identity.Name );
                WriteLog("Error" + e.Message);
                return View("Error");
            }
        }

    }
}
