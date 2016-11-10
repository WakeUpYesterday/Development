

using Microsoft.SqlServer.Management.Smo;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.ServiceProcess;

using DataAccess.Model;

namespace DevConsoleApp2
{


    class Programs
    {

        private static double DetailFactor = 300;

        private static double Detail2Factor = 0.000173;

        static List<LatLng> LatLngList = new List<LatLng>();

        static List<LatLng> LatLngDetailList = new List<LatLng>();

        static void Main(string[] args)
        {
            FillLatLngList();
            SetDetailLatLonList();
            SaveDataTable();
            Console.ReadLine();
        }

        private static void FillLatLngList()
        {
            LatLngList.Add(new LatLng(39.887249, 32.688219));
            LatLngList.Add(new LatLng(39.894624, 32.716801));
            LatLngList.Add(new LatLng(39.882441, 32.693369));
            LatLngList.Add(new LatLng(39.859714, 32.767355));
            LatLngList.Add(new LatLng(39.829525, 32.697598)); 

        }
        /*  Haymana
         *  LatLngList.Add(new LatLng(39.605021, 32.676067)); // Güney alt
            LatLngList.Add(new LatLng(39.605707, 32.676464)); // Güney üst
            LatLngList.Add(new LatLng(39.606930, 32.673020)); // Kuzey üst
            LatLngList.Add(new LatLng(39.606277, 32.672591)); // Kuzey alt 
         */
        public static void SetDetailLatLonList()
        {
            for (int i = 0; i < LatLngList.Count; i++)
            {

                if (i < LatLngList.Count - 1)
                {
                    PerformDetailToLocation(LatLngList[i], LatLngList[i + 1]);
                }
                else
                {
                    PerformDetailToLocation(LatLngList[i], LatLngList[0]);
                }
            }
        }

        private static void PerformDetailToLocation(LatLng l1, LatLng l2)
        {

            double changeLat = (l1.Lat - l2.Lat) / DetailFactor;
            double changeLng = (l1.Lng - l2.Lng) / DetailFactor;

            for (int i = 0; i < DetailFactor; i++)
            {
                LatLngDetailList.Add(new LatLng(l1.Lat - changeLat * i, l1.Lng - changeLng * i ));
            }

        }

        private static void SaveDataTable()
        {
            try
            {

                using (var db = new OtakEntities())
                {

                    db.GpsDataGenerated.RemoveRange(db.GpsDataGenerated);

                    db.SaveChanges();

                    GpsDataGenerated gpsDataGenerated = null;

                    for (int i = 0; i < LatLngDetailList.Count; i++)
                    {
                        gpsDataGenerated = new GpsDataGenerated();
                        gpsDataGenerated.Latitude = LatLngDetailList[i].Lat;
                        gpsDataGenerated.Longitude = LatLngDetailList[i].Lng;
                        gpsDataGenerated.Id = i + 1;
                        db.GpsDataGenerated.Add(gpsDataGenerated);
                        db.SaveChanges();
                    }
                    Console.WriteLine("Db Save Changes Suuccessfully...");
                }
            }
            catch (Exception ex)
            { 
               Console.WriteLine(ex.Message);
            }

        }

        private class LatLng
        {

            public LatLng(double lat,double lng)
            {
                this.Lat = lat;
                this.Lng = lng;

            }

            public  double Lat { set; get; }
            public  double Lng { set; get; }
        }

    }
}
