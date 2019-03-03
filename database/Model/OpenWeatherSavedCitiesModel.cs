using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class OpenWeatherSavedCitiesModel : AppBaseModel
    {
        [Required]
        public int WeatherCitiesId { get; set; }

        #region Constructor
        public OpenWeatherSavedCitiesModel()
        {

        }
        #endregion

    }
}
