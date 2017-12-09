using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Stations.DataProcessor.Dto.Import
{
    public class TicketCardDto
    {
        [Required]
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}