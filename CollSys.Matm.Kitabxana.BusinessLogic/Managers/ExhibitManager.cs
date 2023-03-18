using CollSys.Matm.Kitabxana.BusinessLogic.Managers.Base;
using CollSys.Matm.Kitabxana.BusinessLogic.Services;
using CollSys.Matm.Kitabxana.DataAccess.DataAccess.EntityFrameworkCore;
using CollSys.Matm.Kitabxana.Entities.Tables;

namespace CollSys.Matm.Kitabxana.BusinessLogic.Managers
{
    public class ExhibitManager : ManagerRepository<ExhibitModel, ExhibitEfDal>, IExhibitService
    {
    }
}
