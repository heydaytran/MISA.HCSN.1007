using Dapper;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Infrastructure.Repository
{
    public class AssetRepository:BaseRepository<Asset>, IAssetRepository
    {
        public bool CheckStoreCode(Guid? assetId, string assetCode, string functionName)
           
        {
            //var sqlCheckExistCode = "";
            //if (functionName == "Insert")
            //{
            //    // Thực hiện lấy số lượng bản ghi có mã khách hàng trùng với mã khách hàng truyền vào
            //    sqlCheckExistCode = $"Select AssetCode from Asset where Asset.AssetCode = '{assetCode}'";
            //}
            //else if (functionName == "Update")
            //{
            //    // Thực hiện lấy số lượng bản ghi có mã khách hàng trùng với mã khách hàng truyền vào, loại bỏ bản ghi hiện tại
            //    sqlCheckExistCode = $"Select AssetCode from Asset where Asset.AssetCode = '{assetCode}' and  Asset.AssetId != '{assetId}'";
            //}

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("assetId", assetId);
            parameters.Add("assetCode", assetCode);
            parameters.Add("functionName", functionName);
            // thực hiện truy vấn 
            var storeExistCode = _dbConnection
                .Query<string>(
                    $"Proc_DuplicateAsset",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

            // kiểm tra kết quả:
            if (storeExistCode.AsList().Count > 0)
            {
                return true;
            }

            return false;
        }

        public IEnumerable<Asset> GetEntitiesFilter(string input, int recordAmount, int pageNumber, string departmentId, string typeId)
        {
            string query;
            if (departmentId != null)
            {
                query = "SELECT * FROM Asset_Type_Department WHERE (AssetCode LIKE '%" + input + "%' " +
                "or  AssetName LIKE '%" + input + "%' )and DepartmentId = '" + departmentId + "' order by CreatedDate Desc";
            }
            else if(typeId != null)
            {
                query = " select * FROM Asset_Type_Department WHERE (AssetCode LIKE '%" + input + "%' " +
               "or  AssetName LIKE '%" + input + "%' )and AssetTypeId = '"+typeId+"' order by CreatedDate Desc";
            } 
            else
             query = "SELECT * FROM Asset_Type_Department WHERE AssetCode LIKE '%" + input + "%' " +
                "or  AssetName LIKE '%" + input + "%' order by CreatedDate Desc";
            // Thực hiện lấy dữ liệu từ Database
            var entities = _dbConnection.Query<Asset>(query, commandType: CommandType.Text).OrderByDescending(s => s.CreatedDate).ToList();

            return entities;    
        }
    }
}
