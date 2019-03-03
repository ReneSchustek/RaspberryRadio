using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Database.Model
{
    public class DailyScriptureLanguageModel : AppBaseModel
    {
        [DataMember]
        [Display(Name = "Sprache")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Sprache ist ein Pflichtfeld")]
        public String Language { get; set; }

        [DataMember]
        [Display(Name = "wol.jw.org-Link")]
        [DataType(DataType.Url)]
        [Required(ErrorMessage = "wol.jw.org-Link ist ein Pflichtfeld")]
        public String Url { get; set; }

        #region Constructor
        public DailyScriptureLanguageModel()
        {

        }
        #endregion
    }
}
