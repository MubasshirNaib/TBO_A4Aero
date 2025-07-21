using Backend.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Interfaces
{
    public interface IAgencyValidationService
    {
        Task<ValidateAgencyResponse> ValidateAgencyAsync(ValidateAgencyRequest request);
    }
}
