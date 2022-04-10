using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            if (compensation != null)
            {
                _employeeRepository.Add(compensation);
                _employeeRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return null;
            }

            return _employeeRepository.GetByEmployeeId(employeeId);
        }
    }
}
