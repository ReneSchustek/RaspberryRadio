using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class AppInfoModel : AppBaseModel
    {
        //Software Info
        [MaxLength(255)]
        public String Info { get; set; }

        public string Software { get; set; }

        #region Constructor
        public AppInfoModel()
        {

        }
        #endregion
    }
}
