
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWPF.Functions
{
    using global::DataAccess.Model;

    using TestWPF.Classes;
    public static class GpsOperations
    {

        public static List<PointLatLon> GetPointLatLonList()
        {

            List<PointLatLon> retList = new List<PointLatLon>();
            List<GpsDataGenerated> gpsList = DataAccess.GetPointList();
            retList = ConvertPointLatLonList(gpsList);


            return retList;


        }


        private static List<PointLatLon> ConvertPointLatLonList(List<GpsDataGenerated> gpsList)
        {

            List<PointLatLon> retList = new List<PointLatLon>();

            foreach (var item in gpsList)
            {
                retList.Add(GetPointLatLon(item));
            }

            return retList;
        }


        public static PointLatLon GetPointLatLon (GpsDataGenerated gpsData)
        {
            PointLatLon pointLatLon = new PointLatLon();
            pointLatLon.Latitude = gpsData.Latitude;
            pointLatLon.Longitude = gpsData.Longitude;
            return pointLatLon;
        }

    }
}
