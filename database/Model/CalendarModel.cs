using System;
using System.ComponentModel.DataAnnotations;

namespace Database.Model
{
    public class CalendarModel : AppBaseModel
    {
        //Name
        [MaxLength(255)]
        public String Name { get; set; }

        //Benutzername
        [MaxLength(255)]
        public String Username { get; set; }

        //Passwort
        [MaxLength(255)]
        public String Password { get; set; }

        //CalDav-Url
        [MaxLength(255)]
        public String Url { get; set; }

        #region Constructor
        public CalendarModel()
        {

        }
        #endregion
    }
}
