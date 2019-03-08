using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Model
{
    public class RadioFavModel : AppBaseModel
    {
        //Position auf den Favoriten
        public Int32 Pos { get; set; }

        //Bezeichner für den Sender
        [MaxLength(255)]
        public String Name { get; set; }

        //Stream


    }
}
