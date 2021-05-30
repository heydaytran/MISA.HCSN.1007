using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces;
using MISA.QLTS.Core.Interfaces.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Service
{
    public class AssetIncreaseService:BaseService<AssetIncrease>, IAssetIncreaseService
    {
        IUnitOfWork _unitOfWork;
        public AssetIncreaseService(IUnitOfWork unitOfWork, IBaseRepository<AssetIncrease> baseRepository) : base(baseRepository)
        {
            _unitOfWork = unitOfWork;
        }




    }
}
