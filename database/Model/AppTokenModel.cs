using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    /// <summary>
    /// TokenList
    /// </summary>
    public class AppTokenModel : AppBaseModel
    {
        //DirbleToken
        [MaxLength(255)]
        public String DirbleToken { get; set; }

        //OpenWeatherToken
        [MaxLength(255)]
        public String OpenWeatherToken { get; set; }

        #region Constructor
        public AppTokenModel()
        {

        }
        #endregion
    }
}
