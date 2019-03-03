using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Database.Model
{
    [DataContract]
    public class DailyScriptureModel : AppBaseModel
    {
        [DataMember]
        [Display(Name = "Title")]
        [DataType(DataType.Text)]
        public String Title { get; set; }

        [DataMember]
        [Display(Name = "Text")]
        [DataType(DataType.Text)]
        public String Text { get; set; }

        [DataMember]
        [Display(Name = "Comment")]
        [DataType(DataType.Text)]
        public String Comment { get; set; }

        [DataMember]
        [Display(Name = "Language")]
        [DataType(DataType.Text)]
        public String Language { get; set; }

        [DataMember]
        [Display(Name = "Publication")]
        [DataType(DataType.Text)]
        public String Publication { get; set; }

        #region Constructor
        public DailyScriptureModel()
        {

        }
        #endregion
    }
}
