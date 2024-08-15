using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Portal
{
    [Table("RequirementPurchaseOrder")]
    public class RequirementPurchaseOrder
    {
        [Key]
        public int ID { get; set; }
        public int PurchaseOrderID { get; set; }
        public string CeCo { get; set; }
        public int ProviderID { get; set; }
        public string Company { get; set; }
        public int RequirementID { get; set; }
        public string RequirementValue { get; set; }
        public string UUID { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public int? RolUserSavedFile { get; set; }
    }
}
