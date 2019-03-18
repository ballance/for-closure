using Dapper.Contrib.Extensions;

namespace ballance.it.for_closure.common.models
 {
    public class PropertyModel
    {
        [Key]
        public int Id { get; set; }     
        public string PropertyId { get; set; }
        public string SpNumber { get; set; }
        public string County { get; set; }
        public string SaleDateTime { get; set; }
        public string Address { get; set; }
        public string CityStateZip { get; set; }
        public string DeedOfTrust { get; set; }
        public string Bid { get; set; }
    }
 }