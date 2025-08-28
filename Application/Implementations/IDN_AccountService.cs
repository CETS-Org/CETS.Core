using Application.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Interfaces;
using Domain.Interfaces.CORE;
using Domain.Interfaces.IDN;
using DTOs.IDN_Account.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations
{
    public class IDN_AccountService : IIDN_AccountService
    {
        private readonly IIDN_AccountRepository _accountRepository;
        private readonly ICORE_LookUpRepository _lookUpRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IDN_AccountService(IIDN_AccountRepository accountRepository, ICORE_LookUpRepository lookUpRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _lookUpRepository = lookUpRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<AccountStatusDto>> GetAccountStatusesAsync()
        {
            var lookupEntities = await _lookUpRepository.GetByTypeAsync(LookUpTypes.AccountStatus);

            return _mapper.Map<IReadOnlyList<AccountStatusDto>>(lookupEntities);
        }
    }
}
