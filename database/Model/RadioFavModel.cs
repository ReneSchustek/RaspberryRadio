using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Database.Model
{
    public class RadioFavModel : AppBaseModel
    {
        //Position auf den Favoriten
        [DataMember]
        [Required]
        public Int32 Pos { get; set; }

        //Bezeichner für den Sender
        [DataMember]
        [Required]
        [MaxLength(255)]
        public String Name { get; set; }

        //AnzeigeName
        [DataMember]
        [Required]
        public String FavoriteName { get; set; }

        //Stream
        [DataMember]
        [Required]
        public String Url { get; set; }

        //Image
        [DataMember]
        public String ImageUrl { get; set; }


    }
}
