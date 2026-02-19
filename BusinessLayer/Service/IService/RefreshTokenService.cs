using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.IService
{
    public class RefreshTokenService : IRefreshTokenService
    {


        private readonly IUnitOfWork _unitOfWork;

        private readonly IRefreshTokenRepository _refreshTokenRepository;


        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository,IUnitOfWork unitOfWork) 
        {

            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;


        }


        public Task<string> GenerateRefreshTokenAsync(int userId)
        {



            throw new NotImplementedException();



        }



        public Task RevokeRefreshTokenAsync(int refreshTokenId)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken?> ValidateRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
