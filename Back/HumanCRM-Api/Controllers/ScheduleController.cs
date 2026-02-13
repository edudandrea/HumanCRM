using HumanCRM_Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace HumanCRM_Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly DataContext _context;
        public ScheduleController(DataContext context)
        {
            _context = context;
        }
    }
}