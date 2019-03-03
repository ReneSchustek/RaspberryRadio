using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Database.Model
{
    [DataContract]
    public class OpenWeatherCityModel : AppBaseModel
    {
        [DataMember]
        [Required]
        public Int32 CityId { get; set; }

        [DataMember]
        [Required]
        public String Name { get; set; }

        [DataMember]
        [Required]
        public String Country { get; set; }

        [DataMember]
        public Decimal Lon { get; set; }

        [DataMember]
        public Decimal Lat { get; set; }

        #region Constructor
        public OpenWeatherCityModel()
        {

        }
        #endregion
    }
}
