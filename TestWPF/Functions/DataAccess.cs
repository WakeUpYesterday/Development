using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWPF.Functions
{
    using System.Windows;

    using global::DataAccess.Model;

    using TestWPF.Classes;

    public static class DataAccess
    {

        public static List<GpsDataGenerated> GetPointList()
        {
            List<GpsDataGenerated> retList = new List<GpsDataGenerated>();
            try
            {
                using (var db = new OtakEntities())
                {
                    retList = db.GpsDataGenerated.OrderBy(o => o.Id).ToList();
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
            return retList;
        }


        

    }
}
