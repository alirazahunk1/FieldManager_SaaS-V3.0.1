using ESSDataAccess.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESSDataAccess.Models.Form
{
    public class FormFieldModel : BaseModel
    {
        [ForeignKey(nameof(Form))]
        public int FormId { get; set; }

        public FormModel? Form { get; set; }

        public int Order { get; set; }

        [Display(Name = "Field Type")]
        public FormFieldType FieldType { get; set; }

        [MaxLength(100)]
        public string Label { get; set; }

        [MaxLength(100)]
        public string? Placeholder { get; set; }

        [Display(Name = "Is Required")]
        public bool IsRequired { get; set; }

        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }

        [Display(Name = "Default Value")]
        [MaxLength(500)]
        public string? DefaultValues { get; set; }

        [Display(Name = "Values")]
        [MaxLength(500)]
        public string? Values { get; set; }

        public bool IsEnabled { get; set; } = true;

        public List<FormEntryFieldModel>? FormEntryFields { get; set; }
    }

    public enum FormFieldType
    {
        Text,
        Number,
        Date,
        Time,
        Boolean,
        Select,
        MultiSelect,
        Url,
        Email,
        Address,
        /*
        File,
        Image,
        Video,
        Audio,
        RichText,
        Signature,
        Barcode,
        Location,*/
    }
}
