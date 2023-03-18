using CollSys.Matm.Kitabxana.DataAccess.Connections.EntityFrameworkCore;
using CollSys.Matm.Kitabxana.DataAccess.DataAccess.EntityFrameworkCore.Base;
using CollSys.Matm.Kitabxana.DataAccess.Interfaces;
using CollSys.Matm.Kitabxana.Entities.Tables;

namespace CollSys.Matm.Kitabxana.DataAccess.DataAccess.EntityFrameworkCore
{
    public class MeasurementUnitEfDal : DalRepository<MeasurementUnitModel, SqlDbContext>, IMeasurementUnitDal
    {
    }
}
