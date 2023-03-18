using CollSys.Matm.Kitabxana.BusinessLogic.Managers;
using CollSys.Matm.Kitabxana.BusinessLogic.Services;
using CollSys.Matm.Kitabxana.Entities.Tables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CollSys.Matm.Kitabxana.Presentation.Controllers
{
    public class ParametrlerController : Controller
    {
        // GET: Parametrler
        public ActionResult Index(string msgProqram, string msgProfil)
        {
            ViewBag.MsgProqram = msgProqram;
            ViewBag.MsgProfil = msgProfil;

            IMeasurementUnitService measurementService = new MeasurementUnitManager();
            measurementService.Instance(true);
            ViewBag.MeasurementUnits = measurementService.Read();

            ICurrencyService currencyService = new CurrencyManager();
            currencyService.Instance(true);
            ViewBag.Currencies = currencyService.Read();

            IMaterialService materialService = new MaterialManager();
            materialService.Instance(true);
            ViewBag.Materials = materialService.Read();
            
            IYazarService yazarService = new YazarManager();
            yazarService.Instance(true);
            ViewBag.Yazarlar = yazarService.Read();
            IMetbeeService metbeeService = new MetbeeManager();
            metbeeService.Instance(true);
            ViewBag.Metbeeler = metbeeService.Read();

            IRegionService regionService = new RegionManager();
            regionService.Instance(true);
            ViewBag.Regions = regionService.Read();

            ISaxlanmaVeziyyetiService saxlanmaVeziyyetiService = new SaxlanmaVeziyyetiManager();
            saxlanmaVeziyyetiService.Instance(true);
            ViewBag.SaxlanmaVeziyyetleri = saxlanmaVeziyyetiService.Read();

            ViewBag.User = User.Identity.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MeasurementUnit(string selectedValue, string writtenValue, string submitType)
        {
            string msgProqram = null;

            IMeasurementUnitService measurementService = new MeasurementUnitManager();
            measurementService.Instance(true);

            int id = 0;

            switch (submitType)
            {
                case ("Sil"):
                    if (selectedValue != null)
                    {
                        try
                        {
                            id = Convert.ToInt32(selectedValue);
                            MeasurementUnitModel measurementUnitModelDel = measurementService.Take(c => c.Id == id);
                            measurementService.Delete(measurementUnitModelDel);
                            measurementService.Save();

                        }
                        catch (Exception)
                        {
                            msgProqram = "* Silmək istədiyiniz məlumat artıq istifadə olunub. İstifadə olunmuş məlumatlar silinə bilməz!";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);

                        }
                    }
                    break;
                case ("Dəyişdir"):
                    if (selectedValue != null && writtenValue != null)
                    {
                        id = Convert.ToInt32(selectedValue);
                        MeasurementUnitModel measurementUnitModelUpd = measurementService.Take(c => c.Id == id);
                        measurementUnitModelUpd.MeasurementUnit = writtenValue;

                        try
                        {
                            measurementService.Update(measurementUnitModelUpd);
                            measurementService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                        
                    }
                    break;
                case ("Əlavə et"):
                    if (writtenValue != null)
                    {
                        MeasurementUnitModel measurementUnitModelAdd = new MeasurementUnitModel
                        {
                            MeasurementUnit = writtenValue
                        };

                        try
                        {
                            measurementService.Create(measurementUnitModelAdd);
                            measurementService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }

                    }
                    break;
            }

            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Currency(string selectedValue, string writtenValue, string submitType)
        {
            string msgProqram = null;

            ICurrencyService currencyService = new CurrencyManager();
            currencyService.Instance(true);

            int id = 0;

            switch (submitType)
            {
                case ("Sil"):
                    if (selectedValue != null)
                    {
                        try
                        {
                            id = Convert.ToInt32(selectedValue);
                            CurrencyModel currencyModelDel = currencyService.Take(c => c.Id == id);
                            currencyService.Delete(currencyModelDel);
                            currencyService.Save();

                        }
                        catch (Exception)
                        {
                            msgProqram = "* Silmək istədiyiniz məlumat artıq istifadə olunub. İstifadə olunmuş məlumatlar silinə bilməz!";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                    }
                    break;
                case ("Dəyişdir"):
                    if (selectedValue != null && writtenValue != null)
                    {
                        id = Convert.ToInt32(selectedValue);
                        CurrencyModel currencyModelUpd = currencyService.Take(c => c.Id == id);
                        currencyModelUpd.Currency = writtenValue;

                        try
                        {
                            currencyService.Update(currencyModelUpd);
                            currencyService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                        
                    }
                    break;
                case ("Əlavə et"):
                    if (writtenValue != null)
                    {
                        CurrencyModel currencyModelAdd = new CurrencyModel
                        {
                            Currency = writtenValue
                        };

                        try
                        {
                            currencyService.Create(currencyModelAdd);
                            currencyService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }

                    }
                    break;
            }
            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Material(string selectedValue, string writtenValue, string submitType)
        {
            string msgProqram = null;

            IMaterialService materialService = new MaterialManager();
            materialService.Instance(true);

            int id = 0;

            switch (submitType)
            {
                case ("Sil"):
                    if (selectedValue != null)
                    {
                        try
                        {
                            id = Convert.ToInt32(selectedValue);
                            MaterialModel materialModelDel = materialService.Take(c => c.Id == id);
                            materialService.Delete(materialModelDel);
                            materialService.Save();

                        }
                        catch (Exception)
                        {
                            msgProqram = "* Silmək istədiyiniz məlumat artıq istifadə olunub. İstifadə olunmuş məlumatlar silinə bilməz!";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                    }
                    break;
                case ("Dəyişdir"):
                    if (selectedValue != null && writtenValue != null)
                    {
                        id = Convert.ToInt32(selectedValue);
                        MaterialModel materialModelUpd = materialService.Take(c => c.Id == id);
                        materialModelUpd.Material = writtenValue;

                        try
                        {
                            materialService.Update(materialModelUpd);
                            materialService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                        
                    }
                    break;
                case ("Əlavə et"):
                    if (writtenValue != null)
                    {
                        MaterialModel materialModelAdd = new MaterialModel
                        {
                            Material = writtenValue
                        };

                        try
                        {
                            materialService.Create(materialModelAdd);
                            materialService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }

                    }
                    break;
            }
            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
        }

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Yazar(string selectedValue, string writtenValue, string submitType)
        {
            string msgProqram = null;

            IYazarService yazarService = new YazarManager();
            yazarService.Instance(true);

            int id = 0;

            switch (submitType)
            {
                case ("Sil"):
                    if (selectedValue != null)
                    {
                        try
                        {
                            id = Convert.ToInt32(selectedValue);
                            YazarModel yazarModelDel = yazarService.Take(c => c.Id == id);
                            yazarService.Delete(yazarModelDel);
                            yazarService.Save();

                        }
                        catch (Exception)
                        {
                            msgProqram = "* Silmək istədiyiniz məlumat artıq istifadə olunub. İstifadə olunmuş məlumatlar silinə bilməz!";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                    }
                    break;
                case ("Dəyişdir"):
                    if (selectedValue != null && writtenValue != null)
                    {
                        id = Convert.ToInt32(selectedValue);
                        YazarModel yazarModelUpd = yazarService.Take(c => c.Id == id);
                        yazarModelUpd.Yazar = writtenValue;

                        try
                        {
                            yazarService.Update(yazarModelUpd);
                            yazarService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                        
                    }
                    break;
                case ("Əlavə et"):
                    if (writtenValue != null)
                    {
                        YazarModel yazarModelAdd = new YazarModel
                        {
                            Yazar = writtenValue
                        };

                        try
                        {
                            yazarService.Create(yazarModelAdd);
                            yazarService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }

                    }
                    break;
            }
            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Metbee(string selectedValue, string writtenValue, string submitType)
        {
            string msgProqram = null;

            IMetbeeService metbeeService = new MetbeeManager();
            metbeeService.Instance(true);

            int id = 0;

            switch (submitType)
            {
                case ("Sil"):
                    if (selectedValue != null)
                    {
                        try
                        {
                            id = Convert.ToInt32(selectedValue);
                            MetbeeModel metbeeModelDel = metbeeService.Take(c => c.Id == id);
                            metbeeService.Delete(metbeeModelDel);
                            metbeeService.Save();

                        }
                        catch (Exception)
                        {
                            msgProqram = "* Silmək istədiyiniz məlumat artıq istifadə olunub. İstifadə olunmuş məlumatlar silinə bilməz!";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                    }
                    break;
                case ("Dəyişdir"):
                    if (selectedValue != null && writtenValue != null)
                    {
                        id = Convert.ToInt32(selectedValue);
                        MetbeeModel metbeeModelUpd = metbeeService.Take(c => c.Id == id);
                        metbeeModelUpd.Metbee = writtenValue;

                        try
                        {
                            metbeeService.Update(metbeeModelUpd);
                            metbeeService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                        
                    }
                    break;
                case ("Əlavə et"):
                    if (writtenValue != null)
                    {
                        MetbeeModel metbeeModelAdd = new MetbeeModel
                        {
                            Metbee = writtenValue
                        };

                        try
                        {
                            metbeeService.Create(metbeeModelAdd);
                            metbeeService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }

                    }
                    break;
            }
            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
        }

        
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Region(string selectedValue, string writtenValue, string submitType)
        {
            string msgProqram = null;
            IRegionService regionService = new RegionManager();
            regionService.Instance(true);

            int id = 0;

            switch (submitType)
            {
                case ("Sil"):
                    if (selectedValue != null)
                    {
                        try
                        {
                            id = Convert.ToInt32(selectedValue);
                            RegionModel regionModelDel = regionService.Take(c => c.Id == id);
                            regionService.Delete(regionModelDel);
                            regionService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "* Silmək istədiyiniz məlumat artıq istifadə olunub. İstifadə olunmuş məlumatlar silinə bilməz!";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);

                        }

                    }
                    break;
                case ("Dəyişdir"):
                    if (selectedValue != null && writtenValue != null)
                    {
                        id = Convert.ToInt32(selectedValue);
                        RegionModel regionModelUpd = regionService.Take(c => c.Id == id);
                        regionModelUpd.Region = writtenValue;

                        try
                        {
                            regionService.Update(regionModelUpd);
                            regionService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                        
                    }
                    break;
                case ("Əlavə et"):
                    if (writtenValue != null)
                    {
                        RegionModel regionModelAdd = new RegionModel
                        {
                            Region = writtenValue
                        };

                        try
                        {
                            regionService.Create(regionModelAdd);
                            regionService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }

                    }
                    break;
            }
            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaxlanmaVeziyyeti(string selectedValue, string writtenValue, string submitType)
        {
            string msgProqram = null;
            ISaxlanmaVeziyyetiService saxlanmaVeziyyetiService = new SaxlanmaVeziyyetiManager();
            saxlanmaVeziyyetiService.Instance(true);

            int id = 0;

            switch (submitType)
            {
                case ("Sil"):
                    if (selectedValue != null)
                    {
                        try
                        {
                            id = Convert.ToInt32(selectedValue);
                            SaxlanmaVeziyyetiModel saxlanmaVeziyyetiModel = saxlanmaVeziyyetiService.Take(c => c.Id == id);
                            saxlanmaVeziyyetiService.Delete(saxlanmaVeziyyetiModel);
                            saxlanmaVeziyyetiService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "* Silmək istədiyiniz məlumat artıq istifadə olunub. İstifadə olunmuş məlumatlar silinə bilməz!";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);

                        }

                    }
                    break;
                case ("Dəyişdir"):
                    if (selectedValue != null && writtenValue != null)
                    {
                        id = Convert.ToInt32(selectedValue);
                        SaxlanmaVeziyyetiModel saxlanmaVeziyyetiModelUpd = saxlanmaVeziyyetiService.Take(c => c.Id == id);
                        saxlanmaVeziyyetiModelUpd.SaxlanmaVeziyyeti = writtenValue;

                        try
                        {
                            saxlanmaVeziyyetiService.Update(saxlanmaVeziyyetiModelUpd);
                            saxlanmaVeziyyetiService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                        
                    }
                    break;
                case ("Əlavə et"):
                    if (writtenValue != null)
                    {
                        SaxlanmaVeziyyetiModel saxlanmaVeziyyetiModelAdd = new SaxlanmaVeziyyetiModel
                        {
                            SaxlanmaVeziyyeti = writtenValue
                        };

                        try
                        {
                            saxlanmaVeziyyetiService.Create(saxlanmaVeziyyetiModelAdd);
                            saxlanmaVeziyyetiService.Save();
                        }
                        catch (Exception)
                        {
                            msgProqram = "Məlumat yüklənərkən problem yaşandı. Yüklənən məlumatın hal-hazırda mövcud olmadığından əmin olun";
                            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
                        }
                        
                    }
                    break;
            }
            return Redirect("~/Parametrler/Index?msgProqram=" + msgProqram);
        }

    }
}