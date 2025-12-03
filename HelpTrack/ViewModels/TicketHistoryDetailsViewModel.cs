using HelpTrack.Application.DTOs;
using System.Collections.Generic;

namespace HelpTrack.Web.ViewModels
{
    public class TicketHistoryDetailsViewModel
    {
        public TicketHistoryDTO TicketInfo { get; set; }
        public IEnumerable<HistorialTicketDTO> HistoryLog { get; set; }
    }
}
