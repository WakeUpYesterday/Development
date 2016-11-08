//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class GpsData
    {
        public int Id { get; set; }
        public string HumanTimestamp { get; set; }
        public Nullable<double> UnixTime { get; set; }
        public Nullable<double> Microseconds { get; set; }
        public Nullable<double> SystemFailure { get; set; }
        public Nullable<double> AccelerometerFailure { get; set; }
        public Nullable<double> GyroscopeFailure { get; set; }
        public Nullable<double> MagnetometerFailure { get; set; }
        public Nullable<double> PressureFailure { get; set; }
        public Nullable<double> GNSSFailure { get; set; }
        public Nullable<double> AccelerometerOverrange { get; set; }
        public Nullable<double> GyroscopeOverrange { get; set; }
        public Nullable<double> MagnetometerOverrange { get; set; }
        public Nullable<double> PressureOverrange { get; set; }
        public Nullable<double> MinimumTemperature { get; set; }
        public Nullable<double> MaximumTemperature { get; set; }
        public Nullable<double> LowVoltage { get; set; }
        public Nullable<double> HighVoltage { get; set; }
        public Nullable<double> GNSSAntennaDisconnected { get; set; }
        public Nullable<double> DataOverflow { get; set; }
        public Nullable<double> OrientationReady { get; set; }
        public Nullable<double> NavigationReady { get; set; }
        public Nullable<double> HeadingReady { get; set; }
        public Nullable<double> TimeReady { get; set; }
        public Nullable<double> GNSSFixType { get; set; }
        public Nullable<double> Event1Flag { get; set; }
        public Nullable<double> Event2Flag { get; set; }
        public Nullable<double> InternalGNSSEnabled { get; set; }
        public Nullable<double> DualAntennaHeadingActive { get; set; }
        public Nullable<double> VelocityHeadingEnabled { get; set; }
        public Nullable<double> AtmosphericAltitudeEnabled { get; set; }
        public Nullable<double> ExternalPositionActive { get; set; }
        public Nullable<double> ExternalVelocityActive { get; set; }
        public Nullable<double> ExternalHeadingActive { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> VelocityNorth { get; set; }
        public Nullable<double> VelocityEast { get; set; }
        public Nullable<double> VelocityDown { get; set; }
        public Nullable<double> AccelerationX { get; set; }
        public Nullable<double> AccelerationY { get; set; }
        public Nullable<double> AccelerationZ { get; set; }
        public Nullable<double> GForce { get; set; }
        public Nullable<double> Roll { get; set; }
        public Nullable<double> Pitch { get; set; }
        public Nullable<double> Heading { get; set; }
        public Nullable<double> AngularVelocityX { get; set; }
        public Nullable<double> AngularVelocityY { get; set; }
        public Nullable<double> AngularVelocityZ { get; set; }
        public Nullable<double> LatitudeError { get; set; }
        public Nullable<double> LongitudeError { get; set; }
        public Nullable<double> HeightError { get; set; }
    }
}