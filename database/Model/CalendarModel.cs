using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Database.Model
{
    public class CalendarModel : AppBaseModel
    {
        //Name
        [DataMember]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public String Name { get; set; }

        //Benutzername
        [DataMember]
        [Display(Name = "Username")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public String Username { get; set; }

        //Passwort
        [DataMember]
        [Display(Name = "Password")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public String Password { get; set; }

        //CalDav-Url
        [DataMember]
        [Display(Name = "Url")]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        public String Url { get; set; }

        #region Constructor
        public CalendarModel()
        {

        }
        #endregion
    }
}
